using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.DTOs.Goal;

namespace CaloryCalculation.Application.Interfaces;

public interface IGoalService
{
    Task<Goal> GetGoalByIdAsync(int goalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Goal> CreateGoalAsync(GoalCreationDto dto, CancellationToken cancellationToken = default);
    Task<Goal> UpdateGoalAsync(Goal updatedGoal, CancellationToken cancellationToken = default);
    Task DeleteGoalAsync(int goalId, CancellationToken cancellationToken = default);

    Task<Goal> UpdateGoalByUserIdAsync(int userId, GoalUpdateDto updateDto,
        CancellationToken cancellationToken = default);
}