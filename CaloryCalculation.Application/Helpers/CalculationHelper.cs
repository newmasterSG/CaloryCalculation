namespace CaloryCalculation.Application.Helpers
{
    public static class CalculationHelper
    {
        public static double CalculateCalories(double protein, double fat, double carb)
        {
            return (protein * ApplicationConstants.caloryPerOneGramProtein) + (carb * ApplicationConstants.caloryPerOneGramCarb) + (fat * ApplicationConstants.caloryPerOneGramFat);
        }
    }
}
