using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.DTOs.Goal;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkQueryableExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace CaloryCalculation.Application.Handlers.User;

public class UpdateUserCommandHandler(CaloryCalculationDbContext dbContext, IGoalService goalService) : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.upd.UserId is null)
        {
            throw new ArgumentNullException(nameof(request.upd.UserId), "User id was not founded");
        }

        var user = await EntityFrameworkQueryableExtensions
            .Include(dbContext.Users.Include(x => x.Goals), user => user.Goals)
            .FirstOrDefaultAsync(x => x.Id == request.upd.UserId, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(user);

        if (request.upd.Gender != user.Gender)
        {
            user.Gender = request.upd.Gender;
            dbContext.SaveChanges();
        }
        
        var updatedOrCreatedGoal = await goalService.UpdateGoalByUserIdAsync(user.Id, new GoalUpdateDto()
        {
            Gender = request.upd.Gender,
            TargetWeight = request.upd.Weight,
            Goal = request.upd.Goal,
            ActivityLevel = request.upd.ActivityLevel
        }, cancellationToken);
        
        return updatedOrCreatedGoal != null;
    }
}