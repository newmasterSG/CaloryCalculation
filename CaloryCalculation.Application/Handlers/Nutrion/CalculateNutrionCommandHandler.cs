using CaloryCalculation.Application.Commands.Nutrion;
using CaloryCalculation.Application.DTOs.Nutrion;
using CaloryCalculation.Application.Interfaces;
using MediatR;

namespace CaloryCalculation.Application.Handlers.Nutrion;

public class CalculateNutrionCommandHandler(INutrionService nutrionService) : IRequestHandler<CalculateNutrionCommand, NutrionDTO>
{
    public async Task<NutrionDTO> Handle(CalculateNutrionCommand request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(request.UserId, out var id))
        {
            throw new ArgumentException("Invalid UserId");
        }
        
        var nutritionPlan = await nutrionService.GetNutritionPlanAsync(id);

        return new NutrionDTO()
        {
            Protein = nutritionPlan.protein,
            Fat = nutritionPlan.fat,
            Carb = nutritionPlan.carbs,
        };
    }
}