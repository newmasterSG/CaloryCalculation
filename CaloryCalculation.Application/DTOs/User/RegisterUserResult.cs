namespace CaloryCalculation.Application.DTOs.User;

public class RegisterUserResult
{
    public bool Success { get; set; }
    public int UserId { get; set; }
    public IEnumerable<string> Errors { get; set; }
}