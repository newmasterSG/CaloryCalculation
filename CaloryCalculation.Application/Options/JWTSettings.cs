namespace CaloryCalculation.Application.Options;

public class JwtSettings
{
    public const string Location = "JwtSettings";
    
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
    public int RefreshTokenExpiresInDays { get; set; }
}