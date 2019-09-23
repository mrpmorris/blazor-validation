using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using PeterLeslieMorris.Blazor.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterLeslieMorris.Blazor.FluentValidation
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

			messages.Clear();
			editContext.NotifyValidationStateChanged();

			IEnumerable<IValidator> validators = GetValidatorsForObject(editContext.Model, serviceProvider);

			var validationResults = new List<ValidationResult>();
			foreach (IValidator validator in validators)
			{
				var validationResult = await validator.ValidateAsync(editContext.Model);
				validationResults.Add(validationResult);
			}

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
					propertyChain: new PropertyChain(),
					validatorSelector: new MemberNameValidatorSelector(propertiesToValidate)
				);

			messages.Clear(fieldIdentifier);
			editContext.NotifyValidationStateChanged();

			IEnumerable<IValidator> validators = GetValidatorsForObject(fieldIdentifier.Model, serviceProvider);
			var validationResults = new List<ValidationResult>();

			foreach (IValidator validator in validators)
			{
				var validationResult = await validator.ValidateAsync(fluentValidationContext);
				validationResults.Add(validationResult);
			}

			IEnumerable<string> errorMessages =
				validationResults
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.Distinct();

			foreach (string errorMessage in errorMessages)
				messages.Add(fieldIdentifier, errorMessage);

			editContext.NotifyValidationStateChanged();
		}

		private static IEnumerable<IValidator> GetValidatorsForObject(object model, IServiceProvider serviceProvider)
		{
			var validatorTypesRepository = (FluentValidationRepository)serviceProvider.GetService(typeof(FluentValidationRepository));
			IEnumerable<Type> validatorTypes = validatorTypesRepository.GetValidatorTypesForObject(model);
			IEnumerable<IValidator> validators = validatorTypes.Select(x => (IValidator)serviceProvider.GetService(x));
			return validators;
		}
	}
}
