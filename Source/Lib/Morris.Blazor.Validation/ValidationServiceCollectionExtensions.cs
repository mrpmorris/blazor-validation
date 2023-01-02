using Microsoft.Extensions.DependencyInjection;
using System;

namespace Morris.Blazor.Validation
{
	public static class ValidationServiceCollectionExtensions
	{
		public static IServiceCollection AddFormValidation(
			this IServiceCollection instance,
			Action<ValidationConfiguration> config = null)
		{
			var repository = new ValidationProviderRepository();
			instance.AddScoped<IValidationProviderRepository>((_) => repository);
			if (config != null)
			{
				var c = new ValidationConfiguration(instance, repository);
				config(c);
			}
			return instance;
		}
	}
}
