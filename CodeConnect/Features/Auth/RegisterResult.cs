namespace CodeConnect.Features.Auth;

public class RegisterResult
{
    public required RegisterStatus Status { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

}
public enum RegisterStatus
{
    Successful,
    UserAlreadyExists,
    IdentityRegisterError
}
