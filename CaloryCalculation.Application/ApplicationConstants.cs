namespace CaloryCalculation.Application
{
    public static class ApplicationConstants
    {
        public const int caloryPerOneGramProtein = 4;
        public const int caloryPerOneGramCarb = 4;
        public const int caloryPerOneGramFat = 9;

        public const string refreshTokenKeyDb = "RefreshToken";
        
        // Physical activity constants
        /// <summary>
        /// Minimal activity (sedentary lifestyle): No or minimal exercise, mainly sitting. 
        /// </summary>
        public const double MinimalActivity = 1.2;
    
        /// <summary>
        /// Light activity (1-3 workouts per week): Light exercise or sports 1–3 days a week.
        /// </summary>
        public const double LightActivity = 1.375;
    
        /// <summary>
        /// Moderate activity (3-5 workouts per week): Moderate exercise or sports 3–5 days a week.
        /// </summary>
        public const double ModerateActivity = 1.55;
    
        /// <summary>
        /// High activity (6-7 workouts per week): Intense exercise or sports almost every day.
        /// </summary>
        public const double HighActivity = 1.725;
    
        /// <summary>
        /// Very high activity (twice-daily training or physical labor): Very intense physical activity or manual labor.
        /// </summary>
        public const double VeryHighActivity = 1.9;
        
        
        public const double MinProteinThreshold = 10;
        public const double MinFatThreshold = 5;
        public const double MinCarbohydrateThreshold = 15;
    }
}
