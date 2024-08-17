using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Mappers;
using CaloryCalculation.Application.Queries.Products;
using CaloryCalculation.Db;
using MediatR;
using System.Data.Entity;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class GetProductsHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = dbContext.Products.AsNoTracking();

                var productsDb = await query
                    .Skip((request.Get.Page - 1) * request.Get.PageSize)
                    .Take(request.Get.PageSize)
                    .ToListAsync(cancellationToken);

                var productsDto = productsDb.Select(p => p.ToProductDTO()).ToList();

                return productsDto;
            }
            catch
            {
                throw;
            }
        }
    }
}
