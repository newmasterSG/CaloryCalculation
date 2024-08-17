using CaloryCalculation.Domain.Entities;

namespace CaloryCalculatiom.Domain.Entities
{
    public class Exercise
    {
        public Exercise() 
        {
            DailyLogExercises = new List<DailyLogExercise>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double CaloriesBurnedPerHour { get; set; }
        
        public int CreatedUserId { get; set; }

        public virtual User CreatedUser { get; set; }

        public virtual ICollection<DailyLogExercise> DailyLogExercises { get; set; }
    }
}
