using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Helpers;

namespace CaloryCalculation.Application.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProduct(this CreateProductDTO createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name ?? string.Empty,
                Protein = createProductDto.Protein,
                Fat = createProductDto.Fat,
                Сarbohydrate = createProductDto.Carb,
                Calories = CalculationHelper.CalculateCalories(createProductDto.Protein, createProductDto.Fat, createProductDto.Carb)
            };
        }

        public static Product ToProduct(this UpdateProductDTO updateProductDto)
        {
            return new Product
            {
                Id = updateProductDto.Id,
                Name = updateProductDto.Name,
                Protein = updateProductDto.Protein,
                Fat = updateProductDto.Fat,
                Сarbohydrate = updateProductDto.Carb,
                Calories = CalculationHelper.CalculateCalories(updateProductDto.Protein, updateProductDto.Fat, updateProductDto.Carb)
            };
        }

        public static ProductDTO ToProductDTO(this Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Protein = product.Protein,
                Fat = product.Fat,
                Carb = product.Сarbohydrate,
                Calories = product.Calories
            };
        }

        public static Product ToProduct(this ProductDTO productDto)
        {
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Protein = productDto.Protein,
                Fat = productDto.Fat,
                Сarbohydrate = productDto.Carb,
                Calories = productDto.Calories,
                FoodConsumptions = new List<FoodConsumption>()
            };
        }
    }
}
