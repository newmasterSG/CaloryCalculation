using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Mappers;
using CaloryCalculation.Application.Queries.Products;
using CaloryCalculation.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class GetProductsHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<GetProductsQuery, PagedProductResultDTO>
    {
        public async Task<PagedProductResultDTO> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = dbContext.Products.AsNoTracking();

                var totalCount = await query.CountAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling(totalCount / (double)request.Get.PageSize);
                
                var productsDb = await query
                    .Skip((request.Get.Page - 1) * request.Get.PageSize)
                    .Take(request.Get.PageSize)
                    .ToListAsync(cancellationToken);

                var productsDto = productsDb.Select(p => p.ToProductDTO()).ToList();

                return new PagedProductResultDTO
                {
                    Products = productsDto,
                    TotalPages = totalPages
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
