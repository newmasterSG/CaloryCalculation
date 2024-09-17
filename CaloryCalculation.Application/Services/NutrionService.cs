using CaloryCalculatiom.Domain.Entities;
using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.Helpers;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Db;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Services;

public class NutrionService(CaloryCalculationDbContext dbContext) : INutrionService
{
    public double CalculateBMR(double weight, double height, int age, Gender gender)
    {
        return NutritionCalculatorHelper.CalculateBMR(weight, height, age, gender);
    }
    
    public double CalculateDailyCalories(double weight, double height, int age, Gender gender, ActivityLevel activityLevel)
    {
        double bmr = NutritionCalculatorHelper.CalculateBMR(weight, height, age, gender);
        
        return NutritionCalculatorHelper.CalculateDailyCalories(bmr, activityLevel);
    }
    
    public (double protein, double fat, double carbs) CalculateMacronutrients(double weight, GoalType goal, double dailyCalories)
    {
        return NutritionCalculatorHelper.CalculateMacros(weight, goal, dailyCalories);
    }
    
    public async Task<(double protein, double fat, double carbs)> GetNutritionPlanAsync(int userId)
    {
        var user = await dbContext.Users.AsNoTracking().Include(x => x.Goals).FirstOrDefaultAsync(x => x.Id == userId);

        var goal = user.Goals.FirstOrDefault(g => g.StartDate <= DateTime.UtcNow && g.EndDate == null);

        if (goal == null)
        {
            throw new ArgumentNullException(nameof(goal), "User goal is absent");
        }

        double dailyCalories = CalculateDailyCalories(user.Weight, user.Height, user.Age, user.Gender, goal.ActivityLevel);

        return CalculateMacronutrients(user.Weight, goal.Type, dailyCalories);
    }
    
    public async Task<(double protein, double fat, double carbs)> GetNutritionPlanByUserAsync(User user, ActivityLevel activityLevel, GoalType type)
    {
        double dailyCalories = CalculateDailyCalories(user.Weight, user.Height, user.Age, user.Gender, activityLevel);

        return CalculateMacronutrients(user.Weight, type, dailyCalories);
    }
}