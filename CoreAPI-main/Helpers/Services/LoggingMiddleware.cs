using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Helpers.Services
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> loggery)
        {
            _next = next;
            _logger = loggery;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            string body = "";
            using (var reader = new StreamReader(
            context.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            using (var swapStream = new MemoryStream())
            {
                var originalResponseBody = context.Response.Body;

                context.Response.Body = swapStream;

                await _next(context);

                swapStream.Seek(0, SeekOrigin.Begin);
                string responseBody = new StreamReader(swapStream).ReadToEnd();
                swapStream.Seek(0, SeekOrigin.Begin);

                await swapStream.CopyToAsync(originalResponseBody);
                context.Response.Body = originalResponseBody;

                string path = context.Request.Path.Value.ToLower();

                if (!path.Contains("/coreapi/") && !path.Contains("/favicon.ico") && !path.Contains("/swagger-ui") && !path.ToUpper().Contains("/DXXRDV"))
                {
                    string request = "";
                    foreach (var key in context.Request?.Headers.Keys)
                        request += key + "=" + context.Request?.Headers[key] + Environment.NewLine;

                    string response = "";
                    foreach (var key in context.Response?.Headers.Keys)
                        response += key + "=" + context.Response?.Headers[key] + Environment.NewLine;


                    if (path.Contains("login") || path.Contains("auth") || path.Contains("password"))
                    {
                        body = "This request is secured.";
                    }

                    _logger.LogInformation(
                    "Logging Resquest and response\r\nRequest with method: " + context.Request?.Method + 
                    " and URL: "+ context.Request?.Path.Value + 
                    "\r\nRequest Header: "+ request + 
                    "\r\nRequest Body: "+ body + 
                    "\r\nResponse with status: "+ context.Response?.StatusCode + 
                    "\r\nResponse Header: "+ response + "\r\nResponse Body: " + responseBody);
                }
            }
        }
    }
}
