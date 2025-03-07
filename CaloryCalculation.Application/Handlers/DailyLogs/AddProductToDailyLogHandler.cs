﻿using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.Commands.DailyLogs;
using CaloryCalculation.Application.Interfaces;
using MediatR;

namespace CaloryCalculation.Application.Handlers.DailyLogs
{
    public class AddProductToDailyLogHandler(IDailyLogService dailyLogService) : IRequestHandler<AddProductToDailyLogCommand>
    {
        private readonly IDailyLogService _dailyLogService = dailyLogService;

        public async Task Handle(AddProductToDailyLogCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(MealType), request.DTO.MealType))
            {
                throw new ArgumentException("Invalid MealType value", nameof(request.DTO.MealType));
            }

            var mealType = (MealType)request.DTO.MealType;

            await _dailyLogService.AddProductToDailyLogAsync(request.DTO.UserId, request.DTO.ProductId, request.DTO.Quantity, request.DTO.Date, mealType, cancellationToken);
        }
    }
}
