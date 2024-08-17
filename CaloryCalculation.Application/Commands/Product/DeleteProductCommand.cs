using MediatR;

namespace CaloryCalculation.Application.Commands.Product
{
    public record DeleteProductCommand(int Id) : IRequest<bool>
    {
    }
}
