namespace Middleware;

public interface IJwtBuilder
{
    string GetToken(string userId, string email);
    string ValidateToken(string token, out string email);
}