using CaloryCalculatiom.Domain.Entities;
using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.DTOs.DailyLogs;

namespace CaloryCalculation.Application.Interfaces
{
    public interface IDailyLogService
    {
        Task AddProductToDailyLogAsync(int userId, int productId, double quantity, DateTime date, MealType mealType, CancellationToken cancellationToken = default);
        Task<DailyLog?> GetDailyLogForUserAsync(int userId, DateTime date, CancellationToken cancellationToken = default);

        Task<bool> DeleteProductFromDailyLogByUserIdDateAsync(int userId, int productId, MealType mealType,
            DateTime creationDate, CancellationToken cancellationToken = default);

        Task<DailyLogDTO?> GetDailyLogForUserAsync(GetDailyLogUserDTO getDailyLogUserDto,
            CancellationToken cancellationToken = default);
    }
}
