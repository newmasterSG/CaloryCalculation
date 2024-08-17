using CaloryCalculation.Application.DTOs.Products;
using MediatR;

namespace CaloryCalculation.Application.Commands.Product
{
    public record CreateProductCommand(CreateProductDTO ProductDTO) : IRequest<ProductDTO>
    {
    }
}
