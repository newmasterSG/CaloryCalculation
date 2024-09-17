using CaloryCalculatiom.Domain.Entities.Enums;

namespace CaloryCalculation.Application.DTOs.Goal;

public class GoalUpdateDto
{
    public GoalType Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public float? TargetWeight { get; set; } 
    public DateTime? EndDate { get; set; }
    public Gender Gender { get; set; } 
}