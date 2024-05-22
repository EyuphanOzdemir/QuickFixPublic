using Microsoft.AspNetCore.Mvc;


namespace QuickFixAPI.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			var exception = ex;
			var statusCode = StatusCodes.Status500InternalServerError;

			// Access endpoint information
			var endpoint = context.Request.Path;
			var method = context.Request.Method;

			// Log the exception
			// You can use a logging framework like Serilog, NLog, or ILogger to log the exception
			_logger.LogInformation($"EXCEPTION: {endpoint}-{method}: {exception.Message}");

			// Handle the exception and return a meaningful error response
			context.Response.StatusCode = statusCode;
			var response = new JsonResult(new
			{
				StatusCode = statusCode,
				Message = "An unexpected error occurred."
			});

			await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
		}
	}
}


