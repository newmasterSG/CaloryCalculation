namespace CaloryCalculation.Application.DTOs.DailyLogs;

public class DeleteProductDTO
{
    public int ProductId { get; set; }
    public int? UserId { get; set; }
    public int MealType { get; set; }
    public DateTime CreationDate { get; set; }
}