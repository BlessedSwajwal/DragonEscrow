namespace Infrastructure.Services;

public class EmailSettings
{
    public static string SectionName = "EmailDetails";
    public string? DisplayName { get; set; }
    public string? From { get; set; }
    public string? AppPassword { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public bool UseStartTls { get; set; }
}
