namespace Infrastructure.Authentication;

public class JwtSettings
{
    public static readonly string SectionName = "JwtSettings";
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiresIn = 10;
    public string SecretKey { get; set; } = null!;
}
