namespace QuickFixAPI.Controllers
{
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	namespace QuickFix.Controllers
	{
		[ApiController]
		public class ErrorController : ControllerBase
		{
			private readonly ILogger<ErrorController> _logger;

			public ErrorController(ILogger<ErrorController> logger)
			{
				_logger = logger;
			}

			[Route("/Error")]
			[ApiExplorerSettings(IgnoreApi = true)]
			public IActionResult Error()
			{
				var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

				// Log the exception or perform other error handling tasks
				_logger.LogError($"An error occurred at {exceptionHandlerPathFeature.Path}: {exceptionHandlerPathFeature.Error}");

				// Return a custom error response
				return BadRequest("An unexpected error occurred. Please try again later.");
			}
		}
	}

}
