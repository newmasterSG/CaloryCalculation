using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Mappers;
using CaloryCalculation.Application.Queries.Products;
using CaloryCalculation.Db;
using MediatR;
using System.Data.Entity;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class GetProductHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<GetProductQuery, ProductDTO>
    {
        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (dbContext.Products.Any(x => x.Id != request.Id))
                {
                    return await Task.FromResult(new ProductDTO());
                }

                var productDb = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                return productDb.ToProductDTO();
            }
            catch
            {
                throw;
            }
        }
    }
}
