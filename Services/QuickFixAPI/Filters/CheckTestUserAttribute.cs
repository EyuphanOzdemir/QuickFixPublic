using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuickFixAPI.Filters
{
    public class CheckTestUserAttribute : ActionFilterAttribute
		{
			private readonly ILogger<CheckTestUserAttribute> _logger;

			public CheckTestUserAttribute(ILogger<CheckTestUserAttribute> logger)
			{
				_logger = logger;
			}

			public override void OnActionExecuting(ActionExecutingContext context)
			{
				var user = context.HttpContext.User;
				var username = user.Identity?.Name;

				if (username != "Test")
				{
					_logger.LogInformation("Non-Test request for a test method!");
					context.Result = new ForbidResult(); // Return 403 Forbidden if user is not Test
				}

				base.OnActionExecuting(context);
			}
		}
	}


