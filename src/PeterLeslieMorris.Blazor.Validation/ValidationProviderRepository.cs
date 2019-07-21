using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class ValidationProviderRepository : IValidationProviderRepository
	{
		private Type[] Providers = new Type[0];
		public IEnumerable<Type> All => Providers;

		public IValidationProviderRepository Add(Type providerType)
		{
			if (providerType == null)
				throw new ArgumentNullException(nameof(providerType));
			if (!typeof(IValidationProvider).IsAssignableFrom(providerType))
				throw new ArgumentException($"{providerType.Name} does not implement {nameof(IValidationProvider)}");

			Providers = Providers.Concat(new Type[] { providerType }).ToArray();
			return this;
		}

		public IValidationProviderRepository Remove(Type providerType)
		{
			Providers = Providers.Where(x => x != providerType).ToArray();
			return this;
		}
	}
}
