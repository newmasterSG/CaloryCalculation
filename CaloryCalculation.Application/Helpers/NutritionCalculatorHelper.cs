using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.Helpers;

public static class NutritionCalculatorHelper
{
    private static readonly Dictionary<ActivityLevel, double> ActivityLevels = new Dictionary<ActivityLevel, double>()
    {
        { ActivityLevel.Sedentary, ApplicationConstants.MinimalActivity},
        { ActivityLevel.LightlyActive, ApplicationConstants.LightActivity},
        { ActivityLevel.ModeratelyActive, ApplicationConstants.ModerateActivity},
        { ActivityLevel.VeryActive, ApplicationConstants.HighActivity},
        { ActivityLevel.ExtremelyActive, ApplicationConstants.VeryHighActivity}
    };

    public static double CalculateBMR(double weight, double height, int age, Gender gender)
    {
        return gender switch
        {
            Gender.Male => 10 * weight + 6.25 * height - 5 * age + 5,
            Gender.Female => 10 * weight + 6.25 * height - 5 * age - 161,
            _ => throw new ArgumentException("Gender must be 'male' or 'female'.")
        };
    }
    
    
    // Calculate daily calorie needs based on BMR and activity level
    public static double CalculateDailyCalories(double bmr, ActivityLevel activityLevel)
    { 
        ActivityLevels.TryGetValue(activityLevel, out var level);
        return bmr * level;
    }

    // Calculate macronutrients in grams
    public static (double protein, double fat, double carbs) CalculateMacros(double weight, GoalType goal, double dailyCalories)
    {
        double proteinGrams;
        double fatPercent;
        double carbsPercent;

        switch (goal)
        {
            case GoalType.MaintainWeight:
                proteinGrams = 1.5 * weight;  // 1.5 grams of protein per kg
                fatPercent = 30;              // 30% from fats
                carbsPercent = 50;            // 50% from carbs
                break;

            case GoalType.GainWeight:
                proteinGrams = 2 * weight;    // 2 grams of protein per kg
                fatPercent = 25;              // 25% from fats
                carbsPercent = 55;            // 55% from carbs
                break;

            case GoalType.LoseWeight:
                proteinGrams = 1.8 * weight;  // 1.8 grams of protein per kg for cutting
                fatPercent = 35;              // 35% from fats
                carbsPercent = 30;            // 30% from carbs
                break;

            default:
                throw new ArgumentException("Invalid goal type.");
        }

        // Macronutrient percentage distribution (as a function of total daily calories)
        double caloriesFromProtein = proteinGrams * ApplicationConstants.caloryPerOneGramProtein;
        double caloriesFromFat = dailyCalories * (fatPercent / 100);
        double caloriesFromCarbs = dailyCalories * (carbsPercent / 100);

        double fatGrams = caloriesFromFat / ApplicationConstants.caloryPerOneGramFat; 
        double carbsGrams = caloriesFromCarbs / ApplicationConstants.caloryPerOneGramCarb;

        return (proteinGrams, fatGrams, carbsGrams);
    }
}