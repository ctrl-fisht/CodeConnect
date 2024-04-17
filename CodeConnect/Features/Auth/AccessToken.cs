namespace CodeConnect.Features.Auth;

public class AccessToken
{
    public required string Value { get; set; }
    public DateTime Expires { get; set; }
}
