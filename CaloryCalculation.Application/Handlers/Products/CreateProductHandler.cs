using CaloryCalculation.Application.Commands.Product;
using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Mappers;
using CaloryCalculation.Db;
using MediatR;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class CreateProductHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductDTO>
    {
        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = request.ProductDTO.ToProduct();

            try
            {
                await dbContext.Products.AddAsync(newProduct, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                return newProduct.ToProductDTO();
            }
            catch
            {
                throw;
            }
        }
    }
}
