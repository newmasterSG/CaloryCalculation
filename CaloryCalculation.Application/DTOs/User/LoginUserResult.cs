﻿namespace CaloryCalculation.Application.DTOs.User;

public class LoginUserResult
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}