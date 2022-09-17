using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Middlewares;

public class ScaleAndMoveMiddleware
{
    private const string TopKey = "top";
    private const string LeftKey = "left";
    private const string ScaleKey = "scale";
    private readonly RequestDelegate _next;

    public ScaleAndMoveMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        var service = context.RequestServices
            .GetRequiredService<IScaleAndMoveService>();
        var query = context.Request.Query;

        if (!int.TryParse(query[TopKey].ToString(), out int top)) top = 0;
        if (!int.TryParse(query[LeftKey].ToString(), out int left)) left = 0;
        if (!int.TryParse(query[ScaleKey].ToString(), out int scale)) scale = 1;

        service.Set(new(top, left, scale));

        return _next(context);
    }
}


public static class ScaleAndMoveMiddlewareExtensions
{
    public static IApplicationBuilder UseScaleAndMove(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ScaleAndMoveMiddleware>();
    }
}

