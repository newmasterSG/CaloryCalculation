using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.DTOs.User;

namespace CaloryCalculation.Application.Mappers;

public static class RegisterUserMapperExtensions
{
    public static RegisterUserCommand ToCommand(this RegisterUserDTO dto)
    {
        return new RegisterUserCommand(dto);
    }
}