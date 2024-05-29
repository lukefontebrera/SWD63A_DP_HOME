using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Middleware;

public class JwtMiddleware : IMiddleware
{
    private readonly IJwtBuilder _jwtBuilder;

	public JwtMiddleware(IJwtBuilder jwtBuilder)
    {
        _jwtBuilder = jwtBuilder;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var bearer = context.Request.Headers["Authorization"].ToString();
        var token = bearer.Replace("Bearer ", string.Empty);

        if (!string.IsNullOrEmpty(token))
        {
            var userId = _jwtBuilder.ValidateToken(token, out var email);

            if (ObjectId.TryParse(userId, out _))
            {
                context.Items["userId"] = userId;
                context.Items["email"] = email;
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }

        await next(context);
    }
}