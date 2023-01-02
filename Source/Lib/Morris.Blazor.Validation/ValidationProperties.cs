using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Morris.Blazor.Validation
{
	public class ValidationProperties
	{
		private readonly Dictionary<string, object> MutableValues = new Dictionary<string, object>();

		public readonly IReadOnlyDictionary<string, object> Values;

		private ValidationProperties()
		{
			Values = new ReadOnlyDictionary<string, object>(MutableValues);
		}

		public static ValidationProperties Set => new ValidationProperties();

		public ValidationProperties Value(string name, object value)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException(message: "Required", paramName: nameof(name));
			if (value is null)
				throw new ArgumentNullException(nameof(value));

			MutableValues.Add(name, value);
			return this;
		}
	}

}
