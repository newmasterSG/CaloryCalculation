using CaloryCalculation.Application.DTOs.Products;
using MediatR;

namespace CaloryCalculation.Application.Queries.Products
{
    public record GetProductsQuery(GetAllProduct Get) : IRequest<PagedProductResultDTO>
    {
    }
}
