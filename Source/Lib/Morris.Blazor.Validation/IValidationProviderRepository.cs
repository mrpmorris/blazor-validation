using System;
using System.Collections.Generic;

namespace Morris.Blazor.Validation
{
	public interface IValidationProviderRepository
	{
		IEnumerable<Type> All { get; }
		IValidationProviderRepository Add(Type providerType);
		IValidationProviderRepository Remove(Type providerType);
	}
}
