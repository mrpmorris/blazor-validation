using FluentValidationSample.Models;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using PeterLeslieMorris.Blazor.Validation;

namespace FluentValidationSample
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddFormValidation(config => config.AddFluentValidation(typeof(Person).Assembly));
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			app.AddComponent<App>("app");
		}
	}
}
