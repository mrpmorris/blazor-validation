using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeterLeslieMorris.Blazor.Validation.Fluent
{
	public class FluentValidationValidatorProvider : IValidationProvider
	{
		public void InitializeEditContext(EditContext editContext, IServiceProvider serviceProvider)
		{
			if (editContext == null)
				throw new ArgumentNullException(nameof(editContext));
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			var messages = new ValidationMessageStore(editContext);
			editContext.OnValidationRequested +=
				(sender, _) => ValidateModel((EditContext)sender, messages, serviceProvider);

			editContext.OnFieldChanged +=
				(sender, eventArgs) => ValidateField(editContext, messages, eventArgs.FieldIdentifier, serviceProvider);
		}

		private async void ValidateModel(EditContext editContext, ValidationMessageStore messages, IServiceProvider serviceProvider)
		{
			if (editContext == null)
				throw new ArgumentNullException(nameof(editContext));
			if (messages == null)
				throw new ArgumentNullException(nameof(messages));
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (editContext.Model == null)
				throw new NullReferenceException($"{nameof(editContext)}.{nameof(editContext.Model)}");

			IEnumerable<IValidator> validators = GetValidatorsForObject(editContext, serviceProvider);

			var validationResults = new List<ValidationResult>();
			foreach (IValidator validator in validators)
			{
				var validationResult = await validator.ValidateAsync(editContext.Model);
				validationResults.Add(validationResult);
			}

			messages.Clear();

			IEnumerable<ValidationFailure> validationFailures = validationResults.SelectMany(x => x.Errors);
			foreach (var validationError in validationFailures)
				messages.Add(editContext.Field(validationError.PropertyName), validationError.ErrorMessage);

			editContext.NotifyValidationStateChanged();
		}

		private async void ValidateField(
			EditContext editContext,
			ValidationMessageStore messages,
			FieldIdentifier fieldIdentifier,
			IServiceProvider serviceProvider)
		{

			if (editContext == null)
				throw new ArgumentNullException(nameof(editContext));
			if (messages == null)
				throw new ArgumentNullException(nameof(messages));
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (editContext.Model == null)
				throw new NullReferenceException($"{nameof(editContext)}.{nameof(editContext.Model)}");

			var propertiesToValidate = new string[] { fieldIdentifier.FieldName };
			var fluentValidationContext = 
				new ValidationContext(
					instanceToValidate: fieldIdentifier.Model,
					propertyChain: new FluentValidation.Internal.PropertyChain(),
					validatorSelector: new MemberNameValidatorSelector(propertiesToValidate)
				);


			IEnumerable<IValidator> validators = GetValidatorsForObject(editContext, serviceProvider);

			var validationResults = new List<ValidationResult>();
			foreach (IValidator validator in validators)
			{
				var validationResult = await validator.ValidateAsync(fluentValidationContext);
				validationResults.Add(validationResult);
			}

			messages.Clear(fieldIdentifier);

			IEnumerable<string> errorMessages =
				validationResults
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.Distinct();
			messages.AddRange(fieldIdentifier, errorMessages);

			editContext.NotifyValidationStateChanged();
		}

		private static IEnumerable<IValidator> GetValidatorsForObject(EditContext editContext, IServiceProvider serviceProvider)
		{
			var validatorTypesRepository = (FluentValidationRepository)serviceProvider.GetService(typeof(FluentValidationRepository));
			IEnumerable<Type> validatorTypes = validatorTypesRepository.GetValidatorTypesForObject(editContext.Model);
			IEnumerable<IValidator> validators = validatorTypes.Select(x => (IValidator)serviceProvider.GetService(x));
			return validators;
		}
	}
}
