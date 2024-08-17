using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculatiom.Domain.Entities
{
    public class FoodConsumption
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public virtual Product Product { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }

        public int DailyLogId { get; set; }

        public MealType MealType { get; set; }

        public virtual DailyLog DailyLog { get; set; }
    }
}
