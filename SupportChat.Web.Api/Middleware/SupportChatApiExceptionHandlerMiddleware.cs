using SupportChat.Core.Exceptions;

namespace SupportChat.Web.Api.Middleware;

public class SupportChatApiExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public SupportChatApiExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next.Invoke(ctx);
        }
        catch (SupportChatException ex)
        {
            var result = TypedResults.Problem(ex.Message);
            ctx.Response.ContentType = result.ContentType;
            ctx.Response.StatusCode = result.StatusCode;
            await ctx.Response.WriteAsJsonAsync(result.ProblemDetails);
        }
    }
}