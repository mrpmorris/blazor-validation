using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Morris.Blazor.Validation
{
	public class DataAnnotationsValidatorProvider : IValidationProvider
	{
		public void InitializeEditContext
		(
			EditContext editContext,
			IServiceProvider serviceProvider,
			ValidationProperties properties,
			Func<object, object> transformModel = null
		)
		{
#if NET7_0_OR_GREATER
			editContext.EnableDataAnnotationsValidation(serviceProvider);
#else
			editContext.EnableDataAnnotationsValidation();
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
