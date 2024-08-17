using CaloryCalculation.Application.DTOs.Products;
using MediatR;

namespace CaloryCalculation.Application.Queries.Products
{
    public record GetProductQuery(int Id) : IRequest<ProductDTO>
    {

    }
}
