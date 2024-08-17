using CaloryCalculation.Application.Commands.Product;
using CaloryCalculation.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Handlers.Products
{
    public class DeleteProductHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<DeleteProductCommand, bool>
    {
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (dbContext.Products.Any(x => x.Id != request.Id))
                {
                    return false;
                }

                var isDeleted = await dbContext.Products.Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

                return isDeleted == 1;
            }
            catch
            {
                throw;
            }
        }
    }
}
