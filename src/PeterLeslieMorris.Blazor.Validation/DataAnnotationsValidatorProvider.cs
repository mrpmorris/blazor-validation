using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class DataAnnotationsValidatorProvider : IValidationProvider
	{
		public void InitializeEditContext(
			EditContext editContext,
			IServiceProvider serviceProvider)
		{
#if NET6_0_OR_GREATER
			editContext.EnableDataAnnotationsValidation();
#else
			editContext.AddDataAnnotationsValidation();
#endif
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
