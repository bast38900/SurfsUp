using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NuGet.Protocol;
using System.Net;

namespace SurfsUpMVC.Middleware

{
    public class IpRateLimiter
    {
        private readonly RequestDelegate _next;
        private Dictionary<IPAddress, List<DateTime>> _loggedRequest;
        private int _rentLimit = 3; //3 request for at oprette et rent
        private TimeSpan _limitedTime = new TimeSpan(0,3,0);

        public IpRateLimiter(RequestDelegate next)
        {
            _next = next;
            _loggedRequest = new();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            if (path.StartsWith("/Rental/Rent/"))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    IPAddress clientIp = context.Connection.RemoteIpAddress;
                    DateTime now = DateTime.Now;

                    LogRequest(clientIp, now);
                    if (BlockIp(clientIp))
                    {
                        Block(context);
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }

        private bool BlockIp(IPAddress ip)
        {
            if (ip == null)
                throw new NullReferenceException();
            if (_loggedRequest[ip].Count(dt => dt >= DateTime.Now - _limitedTime) >= _rentLimit)
                return true;
            return false;
            
        }

        private void Block(HttpContext context)
        {

            string route = "/Rental/CantRentMore";
            
            context.Response.Redirect(route);
        }

        private void LogRequest(IPAddress ip, DateTime dateTime)
        {
            if (_loggedRequest.ContainsKey(ip))
                _loggedRequest[ip].Add(dateTime);
            else
                _loggedRequest.Add(ip, new () { dateTime });
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIpRateLimiter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IpRateLimiter>();
        }
    }
}
