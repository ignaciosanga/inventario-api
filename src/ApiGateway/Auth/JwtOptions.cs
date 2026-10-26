namespace ApiGateway.Auth;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; } = 60;
}

public class AuthOptions
{
    public string Usuario { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}

public record LoginRequest(string Usuario, string Clave);
