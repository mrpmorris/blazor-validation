using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PeterLeslieMorris.Blazor.Validation.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace PeterLeslieMorris.Blazor.Validation
{
	public static class ValidationConfigurationFluentValidationExtensions
	{
		public static ValidationConfiguration AddFluentValidation(this ValidationConfiguration config, Assembly assemblyToScan, params Assembly[] additionalAssembliesToScan)
		{
			if (assemblyToScan == null)
				throw new ArgumentNullException(nameof(assemblyToScan));

			var allAssembliesToScan = new List<Assembly>();
			allAssembliesToScan.Add(assemblyToScan);
			if (additionalAssembliesToScan != null)
				allAssembliesToScan.AddRange(additionalAssembliesToScan);

			ScanForValidators(config.Services, allAssembliesToScan);
			config.Services.AddScoped<FluentValidationValidatorProvider>();
			config.Repository.Add(typeof(FluentValidationValidatorProvider));
			return config;
		}

		private static void ScanForValidators(IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
		{
			if (assembliesToScan == null || !assembliesToScan.Any())
				throw new ArgumentNullException(nameof(assembliesToScan));

			var validatorsByType = new Dictionary<Type, List<Type>>();

			IEnumerable<Type> validatorTypesInAssembly = assembliesToScan
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsClass)
				.Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition)
				.Where(x => 
					x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
				);

			System.Diagnostics.Debug.WriteLine("Found " + validatorTypesInAssembly.Count());

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

			validatorsByType
				.SelectMany(x => x.Value)
				.Distinct()
				.ToList()
				.ForEach(x => services.AddScoped(x));

			services.AddSingleton(repository);
		}
	}
}
