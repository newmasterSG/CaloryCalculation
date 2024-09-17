using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.DTOs.Goal;

public class GoalCreationDto
{
    public Gender Gender { get; set; }
    public float Weight { get; set; }
    public GoalType Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public int? UserId { get; set; }
}