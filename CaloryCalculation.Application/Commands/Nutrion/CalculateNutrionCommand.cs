using CaloryCalculation.Application.DTOs.Nutrion;
using MediatR;

namespace CaloryCalculation.Application.Commands.Nutrion;

public record CalculateNutrionCommand(string UserId) : IRequest<NutrionDTO> { }