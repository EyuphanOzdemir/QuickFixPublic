namespace QuickFixAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using QuickFixAPI.Filters;

    namespace QuickFix.Controllers
    {
        [ApiController]
		public class TestController : ControllerBase
		{
			private readonly ILogger<TestController> _logger;

			public TestController(ILogger<TestController> logger)
			{
				_logger = logger;
			}

			[Route("/Test")]
			[ApiExplorerSettings(IgnoreApi = true)]
			[ServiceFilter(typeof(CheckTestUserAttribute))]
			public IActionResult Test()
			{
				return Ok("OK");	
			}
		}
	}

}
