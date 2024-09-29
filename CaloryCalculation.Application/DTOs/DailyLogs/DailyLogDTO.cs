using CaloryCalculation.Application.DTOs.FoodConsumptions;

namespace CaloryCalculation.Application.DTOs.DailyLogs;

public class DailyLogDTO
{
    public int Id { get; set; }
    public List<FoodConsumptionDTO> FoodConsumption { get; set; }
}