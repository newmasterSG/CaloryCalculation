using CaloryCalculation.Application.Commands.Product;
using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Helpers;
using CaloryCalculation.Application.Mappers;
using CaloryCalculation.Db;
using MediatR;
using System.Data.Entity;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class UpdateProductHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<UpdateProductCommand, ProductDTO>
    {
        public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (dbContext.Products.Any(x => x.Id != request.DTO.Id))
            {
                return await Task.FromResult(new ProductDTO());
            }

            try
            {
                var productDb = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.DTO.Id, cancellationToken);

                var updProduct = request.DTO.ToProduct();

                if (productDb.Protein != updProduct.Protein)
                {
                    productDb.Protein = updProduct.Protein;
                }

                if (productDb.Сarbohydrate != updProduct.Сarbohydrate)
                {
                    productDb.Сarbohydrate = updProduct.Сarbohydrate;
                }

                if (productDb.Fat != updProduct.Fat)
                {
                    productDb.Fat = updProduct.Fat;
                }

                if (!productDb.Name.Equals(updProduct.Name))
                {
                    productDb.Name = updProduct.Name;
                }

                if (productDb.Calories != updProduct.Calories)
                {
                    productDb.Calories = updProduct.Calories;
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                return productDb.ToProductDTO();
            }
            catch
            {
                throw;
            }
        }
    }
}
