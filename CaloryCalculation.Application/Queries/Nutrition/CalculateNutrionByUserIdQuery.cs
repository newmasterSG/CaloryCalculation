using CaloryCalculation.Application.DTOs.Nutrion;
using MediatR;

namespace CaloryCalculation.Application.Commands.Nutrion;

public record CalculateNutrionByUserIdQuery(string UserId) : IRequest<NutrionDTO> { }