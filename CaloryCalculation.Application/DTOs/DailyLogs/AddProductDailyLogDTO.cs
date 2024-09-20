namespace CaloryCalculation.Application.DTOs.DailyLogs
{
    public class AddProductDailyLogDTO
    {
        public int? UserId { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }

        public int MealType { get; set; }
    }
}
