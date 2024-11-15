using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.DTOs.User;

public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public float Weight { get; set; }
    public GoalType Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
}