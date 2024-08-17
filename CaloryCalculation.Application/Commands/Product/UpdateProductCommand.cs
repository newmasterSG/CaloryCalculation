using CaloryCalculation.Application.DTOs.Products;
using MediatR;

namespace CaloryCalculation.Application.Commands.Product
{
    public record UpdateProductCommand(UpdateProductDTO DTO) : IRequest<ProductDTO>
    {

    }
}
