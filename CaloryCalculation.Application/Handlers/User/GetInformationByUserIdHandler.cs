using CaloryCalculation.Application.DTOs.User;
using CaloryCalculation.Application.Queries.User;
using CaloryCalculation.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Handlers.User;

public class GetInformationByUserIdHandler(CaloryCalculationDbContext dbContext) : IRequestHandler<GetUserInformationById, UserDTO>
{
    public async Task<UserDTO> Handle(GetUserInformationById request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.Goals)
            .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

        ArgumentNullException.ThrowIfNull(user);

        var currentGoal = user.Goals.FirstOrDefault(g => g.StartDate <= DateTime.UtcNow && g.EndDate is null);

        return new UserDTO()
        {
            Id = user.Id,
            Email = user.Email,
            Gender = user.Gender,
            Weight = (float)user.Weight,
            ActivityLevel = currentGoal.ActivityLevel,
            Goal = currentGoal.Type
        };
    }
}