using CaloryCalculation.Domain.Entities;

namespace CaloryCalculatiom.Domain.Entities
{
    public class DailyLog
    {
        public DailyLog()
        {
            FoodConsumptions = new List<FoodConsumption>();
            DailyLogExercises = new List<DailyLogExercise>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public virtual ICollection<FoodConsumption> FoodConsumptions { get; set; }
        public virtual ICollection<DailyLogExercise> DailyLogExercises { get; set; }
    }
}
