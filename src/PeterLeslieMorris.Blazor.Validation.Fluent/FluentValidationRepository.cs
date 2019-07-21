using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PeterLeslieMorris.Blazor.Validation.Fluent
{
	internal class FluentValidationRepository
	{
		private readonly ReadOnlyDictionary<Type, IEnumerable<Type>> ValidatorTypesByTypeToValidate;

		public FluentValidationRepository(IEnumerable<KeyValuePair<Type, IEnumerable<Type>>> typeAndValidatorTypes)
		{
			if (typeAndValidatorTypes == null)
				throw new ArgumentNullException(nameof(typeAndValidatorTypes));

			ValidatorTypesByTypeToValidate = new ReadOnlyDictionary<Type, IEnumerable<Type>>(
				typeAndValidatorTypes.ToDictionary(x => x.Key, x => x.Value)
			);
		}

		public IEnumerable<Type> GetValidatorTypesForObject(object instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			Type typeToValidate = instance.GetType();
			if (!ValidatorTypesByTypeToValidate.TryGetValue(typeToValidate, out IEnumerable<Type> validatorTypes))
				return Array.Empty<Type>();
			return validatorTypes;
		}

	}
}
