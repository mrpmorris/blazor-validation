using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PeterLeslieMorris.Blazor.Validation.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PeterLeslieMorris.Blazor.Validation
{
	public static class ValidationConfigurationDataAnnotationsExtensions
	{
		public static ValidationConfiguration AddDataAnnotationsValidation(this ValidationConfiguration config, params Assembly[] assembliesToScan)
		{
			ScanForValidators(config.Services, assembliesToScan);
			config.Services.AddScoped<FluentValidationValidatorProvider>();
			config.Repository.Add(typeof(FluentValidationValidatorProvider));
			return config;
		}

		private static void ScanForValidators(IServiceCollection services, Assembly[] assembliesToScan)
		{
			if (assembliesToScan == null || assembliesToScan.Length == 0)
				throw new ArgumentNullException(nameof(assembliesToScan));

			var validatorsByType = new Dictionary<Type, List<Type>>();

			IEnumerable<Type> validatorTypesInAssembly = assembliesToScan
				.SelectMany(x => x.GetExportedTypes())
				.Where(x => typeof(IValidator<>).IsAssignableFrom(x))
				.Where(x => x.IsClass)
				.Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition);

			foreach(Type validatorType in validatorTypesInAssembly)
			{
				Type typeToValidate = validatorType.BaseType.GetGenericArguments()[0];
				List<Type> validatorTypesForTypeToValidate;
				if (!validatorsByType.TryGetValue(typeToValidate, out validatorTypesForTypeToValidate))
				{
					validatorTypesForTypeToValidate = new List<Type>();
					validatorsByType[typeToValidate] = validatorTypesForTypeToValidate;
				}
				validatorTypesForTypeToValidate.Add(validatorType);
			}

			var repository = new FluentValidationRepository(
				validatorsByType.Select(x => new KeyValuePair<Type, IEnumerable<Type>>(x.Key, x.Value))
			);

			services.AddSingleton(repository);
		}
	}
}
