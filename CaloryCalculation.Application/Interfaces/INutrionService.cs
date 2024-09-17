using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.Interfaces;

public interface INutrionService
{
    double CalculateBMR(double weight, double height, int age, Gender gender);
    double CalculateDailyCalories(double weight, double height, int age, Gender gender, ActivityLevel activityLevel);
    (double protein, double fat, double carbs) CalculateMacronutrients(double weight, GoalType goal, double dailyCalories);
    Task<(double protein, double fat, double carbs)> GetNutritionPlanAsync(int userId);
}