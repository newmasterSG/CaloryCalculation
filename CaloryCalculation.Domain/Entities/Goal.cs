using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculatiom.Domain.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public GoalType Type { get; set; }
        public double TargetWeight { get; set; } 
        public double DailyCaloriesGoal { get; set; }
        public double DailyProteinGoal { get; set; }
        public double DailyCarbohydratesGoal { get; set; }
        public double DailyFatGoal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
