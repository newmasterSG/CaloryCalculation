﻿using CaloryCalculation.Application.Commands.Nutrion;
using CaloryCalculation.Application.DTOs.Nutrion;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Application.Services;
using MediatR;

namespace CaloryCalculation.Application.Handlers.Nutrion;

public class CalculateNutritionByUserIdQueryHandler(IGoalService goalService,
    IDailyLogService dailyLogService) : IRequestHandler<CalculateNutrionByUserIdQuery, NutritionDTO>
{
    public async Task<NutritionDTO> Handle(CalculateNutrionByUserIdQuery request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(request.UserId, out var id))
        {
            throw new ArgumentException("Invalid UserId");
        }
        
        var nutritionPlan = await goalService.GetDailyPlanningAsync(id, cancellationToken);
         
        nutritionPlan.Streak = await dailyLogService.GetLongestStreakAsync(id, cancellationToken);
         
        return nutritionPlan;
    }
}