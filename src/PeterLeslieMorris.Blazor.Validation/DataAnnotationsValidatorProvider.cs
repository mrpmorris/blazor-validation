using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class DataAnnotationsValidatorProvider : IValidationProvider
	{
		public void InitializeEditContext(EditContext editContext)
		{
			editContext.AddDataAnnotationsValidation();
		}
	}

	public static class ValidationConfigurationDataAnnotationsExtensions
	{
		public static ValidationConfiguration AddDataAnnotationsValidation(this ValidationConfiguration config)
		{
			config.Services.AddScoped<DataAnnotationsValidatorProvider>();
			config.Repository.Add(typeof(DataAnnotationsValidatorProvider));
			return config;
		}
	}
}
