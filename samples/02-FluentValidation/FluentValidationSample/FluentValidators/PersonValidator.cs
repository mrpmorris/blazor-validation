using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidationSample.Models;

namespace FluentValidationSample.FluentValidators
{
	public class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			RuleFor(x => x.Salutation)
				.Cascade(CascadeMode.StopOnFirstFailure)
				.NotEmpty()
				.MustAsync(LongRunningAsyncMethod).WithMessage("Cannot be DR");
			RuleFor(x => x.GivenName).NotEmpty();
			RuleFor(x => x.FamilyName).NotEmpty();
			RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
			RuleFor(x => x.Addresses).NotEmpty().WithMessage("At least one address is required");
		}

		private async Task<bool> LongRunningAsyncMethod(string arg1, CancellationToken arg2)
		{
			await Task.Delay(1000);
			if ("DR".Equals(arg1, StringComparison.InvariantCultureIgnoreCase))
				return false;
			return true;
		}
	}
}
