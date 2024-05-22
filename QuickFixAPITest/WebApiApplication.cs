using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using QuickFixAPI.Data;
using MongoDB.Driver;
using QuickFixAPITest.Logger;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;

namespace QuickFixAPITest;

class WebApiApplication : WebApplicationFactory<Program>
{
	private readonly ITestOutputHelper _testOutputHelper;

	public WebApiApplication(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	protected override IHost CreateHost(IHostBuilder builder)
	{
		//builder.UseEnvironment("Development");

		builder.ConfigureLogging(loggingBuilder =>
		{
			loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
		});

		builder.ConfigureServices(services =>
		{
			var descriptors = services.Where(
							 d => d.ServiceType ==
							 typeof(IMongoDatabase) ||
							 d.ServiceType ==
							 typeof(IRepository<Fix>)
							 ).ToList();

			descriptors.ForEach(d => services.Remove(d));
			services.AddSingleton<IRepository<Fix>, TestRepositoryFix<Fix>>();
		});
		
		return base.CreateHost(builder);
	}
}
