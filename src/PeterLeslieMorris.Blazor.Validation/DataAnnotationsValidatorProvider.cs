using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class DataAnnotationsValidatorProvider : IValidationProvider
	{
		public Task ValidationComplete => Task.CompletedTask;

		public void InitializeEditContext(
			EditContext editContext,
			IServiceProvider serviceProvider)
		{
			editContext.AddDataAnnotationsValidation();
		}
	}


	public static class ValidationConfigurationDataAnnotationsExtensions
	{
		public static ValidationConfiguration AddDataAnnotationsValidation(
			this ValidationConfiguration config)
		{
			config.Services.AddScoped<DataAnnotationsValidatorProvider>();
			config.Repository.Add(typeof(DataAnnotationsValidatorProvider));
			return config;
		}
	}
}
