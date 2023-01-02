using FluentValidation;
using Morris.Blazor.Validation;

namespace Morris.Blazor.FluentValidation
{
	public static class FluentValidationPropertiesExtensions
	{
		public const string FluentValidatorKey = "FluentValidatorKey";

		public static ValidationProperties FluentValidator<T>(this ValidationProperties properties)
			where T : IValidator
		{
			return properties.Value("x", "y");
		}
	}
}
