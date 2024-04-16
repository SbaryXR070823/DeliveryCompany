using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/CreateAsync"))
        {
			context.Request.Method = "GET";
			context.Response.Redirect("/Orders");
            return; 
		}

        if (context.Request.Path.Equals("/undefined"))
        {
            return;
        }

        if (context.Request.Path == "/Orders" && context.Request.Method == "POST")
        {
            context.Request.Method = "GET";
            context.Response.Redirect("/Orders");
            return;
        }
		await _next(context);
    }
}
