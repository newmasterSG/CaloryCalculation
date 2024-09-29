using CaloryCalculation.Application.DTOs.Products;

namespace CaloryCalculation.Application.DTOs.FoodConsumptions;

public class FoodConsumptionDTO
{
    public List<ProductDTO> Products { get; set; }
    public int MealType { get; set; }
    public double Quantity { get; set; }
}