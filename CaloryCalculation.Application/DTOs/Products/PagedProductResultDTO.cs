namespace CaloryCalculation.Application.DTOs.Products;

public class PagedProductResultDTO
{
    public List<ProductDTO> Products { get; set; }
    public int TotalPages { get; set; }
}