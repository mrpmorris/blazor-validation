using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using PeterLeslieMorris.Blazor.Validation;

namespace DataAnnotationsValidationSample
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddFormValidation(config => config.AddDataAnnotationsValidation());
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			app.AddComponent<App>("app");
		}
	}
}
