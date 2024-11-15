using CaloryCalculatiom.Domain.Entities;
using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.DTOs.Products;
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
    
    public async Task<PagedProductResultDTO> GetSuggestedProducts(int userId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        
        var goalTask = dbContext.Goals
            .Where(g => g.UserId == userId && g.StartDate <= DateTime.UtcNow && g.EndDate == null)
            .FirstOrDefaultAsync(cancellationToken);

        var dailyLogsTask = dbContext.DailyLogs
            .Where(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date)
            .Include(d => d.FoodConsumptions)
            .ThenInclude(fc => fc.Product)
            .ToListAsync(cancellationToken);
        
        var goal = await goalTask;
        var dailyLogs = await dailyLogsTask;

        if (goal == null)
        {
            throw new ArgumentException("Goal not found");
        }
        
        (double totalProtein, double totalFat, double totalCarbohydrate) = CalculateTotalNutrients(dailyLogs);
        
        var (deficitProtein, deficitFat, deficitCarbohydrates) = CalculateNutrientDeficit(goal, totalProtein, totalFat, totalCarbohydrate);

        var query = dbContext.Products
            .Where(p =>
                (deficitProtein > 0 && p.Protein > ApplicationConstants.MinProteinThreshold) ||
                (deficitFat > 0 && p.Fat > ApplicationConstants.MinFatThreshold) ||
                (deficitCarbohydrates > 0 && p.Сarbohydrate > ApplicationConstants.MinCarbohydrateThreshold));
        
        int totalProducts = await query.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
        
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Protein = p.Protein,
                Fat = p.Fat,
                Carb = p.Сarbohydrate,
                Gram = p.PerGram,
                Calories = p.Calories,
            })
            .ToListAsync(cancellationToken);
        
        var result = new PagedProductResultDTO
        {
            Products = products,
            TotalPages = totalPages
        };

        return result;
    }
    
    private (double totalProtein, double totalFat, double totalCarbohydrate) CalculateTotalNutrients(List<DailyLog> dailyLogs)
    {
        double totalProtein = 0;
        double totalFat = 0;
        double totalCarbohydrate = 0;
        
        foreach (var log in dailyLogs)
        {
            foreach (var foodConsumption in log.FoodConsumptions)
            {
                totalProtein += (foodConsumption.Product.Protein * foodConsumption.Quantity / foodConsumption.Product.PerGram);
                totalFat += (foodConsumption.Product.Fat * foodConsumption.Quantity / foodConsumption.Product.PerGram);
                totalCarbohydrate += (foodConsumption.Product.Сarbohydrate * foodConsumption.Quantity / foodConsumption.Product.PerGram);
            }
        }

        return (totalProtein, totalFat, totalCarbohydrate);
    }

    private (double deficitProtein, double deficitFat, double deficitCarbohydrates) CalculateNutrientDeficit(Goal goal, double totalProtein, double totalFat, double totalCarbohydrate)
    {
        var deficitProtein = goal.DailyProteinGoal - totalProtein;
        var deficitFat = goal.DailyFatGoal - totalFat;
        var deficitCarbohydrates = goal.DailyCarbohydratesGoal - totalCarbohydrate;

        return (deficitProtein, deficitFat, deficitCarbohydrates);
    }
}