using CaloryCalculation.Application.DTOs.User;
using MediatR;

namespace CaloryCalculation.Application.Queries.User;

public record GetUserInformationById(int Id) : IRequest<UserDTO>;