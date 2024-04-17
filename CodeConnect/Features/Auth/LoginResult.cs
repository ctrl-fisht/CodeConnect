namespace CodeConnect.Features.Auth;

public class LoginResult
{
    
    public required LoginStatus Status { get; set; }
    public string? AccessToken { get; set; } = null;
    public DateTime? Expires { get; set; } = null;
}
public enum LoginStatus
{
    Successful,
    InvalidCredentials
}