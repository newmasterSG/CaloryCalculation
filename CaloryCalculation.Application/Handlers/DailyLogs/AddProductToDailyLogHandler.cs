using CaloryCalculatiom.Domain.Entities.Enums;
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
            if (!Enum.IsDefined(typeof(MealType), (byte)request.DTO.MealType))
            {
                throw new ArgumentException("Invalid MealType value", nameof(request.DTO.MealType));
            }
            
            ArgumentNullException.ThrowIfNull(request.DTO.UserId);

            var mealType = (MealType)request.DTO.MealType;

            await _dailyLogService.AddProductToDailyLogAsync(request.DTO.UserId ?? 0, request.DTO.ProductId, request.DTO.Quantity, request.DTO.Date, mealType, cancellationToken);
        }
    }
}
