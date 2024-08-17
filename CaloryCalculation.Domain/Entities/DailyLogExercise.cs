using CaloryCalculatiom.Domain.Entities;

namespace CaloryCalculation.Domain.Entities
{
    public class DailyLogExercise
    {
        public int DailyLogId { get; set; }
        public DailyLog DailyLog { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }
}
