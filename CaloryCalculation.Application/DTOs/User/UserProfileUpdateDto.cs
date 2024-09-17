using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.DTOs.User;

public class UserProfileUpdateDto
{
    public Gender Gender { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public GoalType Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    
    public int? UserId { get; set; }
}