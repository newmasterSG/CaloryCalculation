using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.DTOs.Goal;
using CaloryCalculation.Application.DTOs.Nutrion;
using CaloryCalculation.Application.Helpers;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Db;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Services;

public class GoalService(CaloryCalculationDbContext dbContext, INutrionService nutrionService)
    : IGoalService
{
    public async Task<Goal> GetGoalByIdAsync(int goalId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Goals
            .FirstOrDefaultAsync(g => g.Id == goalId, cancellationToken);
    }

    public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Goals
            .Where(g => g.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Goal> CreateGoalAsync(GoalCreationDto dto, CancellationToken cancellationToken = default)
    {
        User user = null;
        if (dto.UserId.HasValue)
        {
            user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId.Value, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id {dto.UserId.Value} not found");
            }
        }
        
        var dailyCalories = nutrionService.CalculateDailyCalories(
            dto.Weight, user?.Height ?? 0, user?.Age ?? 0, dto.Gender, dto.ActivityLevel);

        var (protein, fat, carbs) = nutrionService.CalculateMacronutrients(dto.Weight, dto.Goal, dailyCalories);

        var goal = new Goal
        {
            UserId = dto.UserId.Value,
            Type = dto.Goal,
            ActivityLevel = dto.ActivityLevel,
            TargetWeight = dto.Weight,
            DailyCaloriesGoal = dailyCalories,
            DailyProteinGoal = protein,
            DailyFatGoal = fat,
            DailyCarbohydratesGoal = carbs,
            StartDate = DateTime.UtcNow
        };

        dbContext.Goals.Add(goal);
        await dbContext.SaveChangesAsync(cancellationToken);
        return goal;
    }

    public async Task<Goal> UpdateGoalAsync(Goal updatedGoal, CancellationToken cancellationToken = default)
    {
        var existingGoal = await dbContext.Goals
            .FirstOrDefaultAsync(g => g.Id == updatedGoal.Id, cancellationToken);

        if (existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with Id {updatedGoal.Id} not found");
        }

        if (EntityHelper.UpdatePropertiesIfChanged(existingGoal, updatedGoal))
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return existingGoal;
    }

    public async Task DeleteGoalAsync(int goalId, CancellationToken cancellationToken = default)
    {
        var goal = await dbContext.Goals
            .FirstOrDefaultAsync(g => g.Id == goalId, cancellationToken);

        if (goal != null)
        {
            dbContext.Goals.Remove(goal);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Goal> UpdateGoalByUserIdAsync(int userId, GoalUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
                       .Include(u => u.Goals)
                       .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                   ?? throw new KeyNotFoundException($"User with Id {userId} not found");

        var activeGoal = user.Goals
            .FirstOrDefault(g => g.StartDate <= DateTime.UtcNow && g.EndDate == null);

        bool isModified = false;

        if (activeGoal != null)
        {
            if (activeGoal.Type == updateDto.Goal)
            {
                isModified = EntityHelper.UpdatePropertiesIfChanged(activeGoal, updateDto);
            }
            else
            {
                activeGoal.EndDate = DateTime.UtcNow;
                isModified = true;
            }
        }

        var newGoal = await CreateGoalAsync(new GoalCreationDto
        {
            UserId = userId,
            Goal = updateDto.Goal,
            Weight = user.Weight,
            ActivityLevel = updateDto.ActivityLevel,
            Gender = updateDto.Gender
        }, cancellationToken);

        user.Goals.Add(newGoal);
        isModified = true;

        if (isModified)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return newGoal;
    }

    public async Task<NutritionDTO> GetDailyPlanningAsync(int userId, CancellationToken cancellationToken = default)
    {
        var goal = await dbContext.Goals.FirstOrDefaultAsync(g => g.UserId == userId && g.StartDate <= DateTime.UtcNow && g.EndDate == null, cancellationToken);

        if (goal is null)
        {
            return new NutritionDTO();
        }

        return new NutritionDTO()
        {
            DailyCalories = goal.DailyCaloriesGoal,
            Protein = goal.DailyProteinGoal,
            Fat = goal.DailyFatGoal,
            Carb = goal.DailyCarbohydratesGoal
        };
    }
}