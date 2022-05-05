﻿using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using PeterLeslieMorris.Blazor.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PeterLeslieMorris.Blazor.Validation.Extensions;

namespace PeterLeslieMorris.Blazor.FluentValidation
{
	public class FluentValidationValidatorProvider : IValidationProvider
	{
		public Task ValidationComplete => ValidationCompleteTaskSource.Task;

		private SpinLock Locker = new SpinLock();
		private TaskCompletionSource<object> ValidationCompleteTaskSource = new TaskCompletionSource<object>();

		public void InitializeEditContext(
			EditContext editContext,
			IServiceProvider serviceProvider)
		{
			if (editContext == null)
				throw new ArgumentNullException(nameof(editContext));
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			ValidationCompleteTaskSource.SetResult(true);

			var messages = new ValidationMessageStore(editContext);
			editContext.OnValidationRequested +=
				(sender, eventArgs) =>
				{
					_ = KeepCountAsync(() => ValidateModelAsync((EditContext)sender, messages, serviceProvider));
				};

			editContext.OnFieldChanged +=
				(sender, eventArgs) =>
				{
					_ = KeepCountAsync(() => ValidateFieldAsync(editContext, messages, eventArgs.FieldIdentifier, serviceProvider));
				};
		}

		private int EntryCount;

		private async Task KeepCountAsync(Func<Task> validation)
		{
			bool validationWasAlreadyRunning = false;
			Task<object> completionTask = null;
			Locker.ExecuteLocked(() =>
			{
				completionTask = ValidationCompleteTaskSource.Task;
				EntryCount++;
				Console.WriteLine($"Starting validation level {EntryCount}");
				if (EntryCount > 1)
					validationWasAlreadyRunning = true;
				else
				{
					ValidationCompleteTaskSource = new TaskCompletionSource<object>();
					completionTask = ValidationCompleteTaskSource.Task;
				}
			});

			if (validationWasAlreadyRunning)
			{
				await completionTask;
				return;
			}

			try
			{
				await validation();
			}
			finally
			{
				TaskCompletionSource<object> completionSource = ValidationCompleteTaskSource;
				Locker.ExecuteLocked(() =>
				{
					Console.WriteLine($"Ending validation level {EntryCount}");
					EntryCount--;
					if (EntryCount == 0)
						completionSource.SetResult(null);
				});
			}
		}

		private async Task ValidateModelAsync(
			EditContext editContext,
			ValidationMessageStore messages,
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

			messages.Clear();
			editContext.NotifyValidationStateChanged();

			IEnumerable<IValidator> validators = GetValidatorsForObject(editContext.Model, serviceProvider);

			var validationContext = new ValidationContext<object>(editContext.Model);

			var validationResults = new List<ValidationResult>();
			foreach (IValidator validator in validators)
			{
				var validationResult = await validator.ValidateAsync(validationContext);
				validationResults.Add(validationResult);
			}

			IEnumerable<ValidationFailure> validationFailures = validationResults.SelectMany(x => x.Errors);
			foreach (var validationError in validationFailures)
			{
				GetParentObjectAndPropertyName(editContext.Model, validationError.PropertyName, out object parentObject, out string propertyName);
				if (parentObject != null)
					messages.Add(new FieldIdentifier(parentObject, propertyName), validationError.ErrorMessage);
			}

			editContext.NotifyValidationStateChanged();
		}

		private void GetParentObjectAndPropertyName(
			object model,
			string propertyPath,
			out object parentObject,
			out string propertyName)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			var propertyPathParts = new Queue<string>(propertyPath.Split('.'));
			Type modelType = model.GetType();
			while (propertyPathParts.Count > 1)
			{
				var name = propertyPathParts.Dequeue();

				string propertyIndexString = null;
				int bracketIndex = name.IndexOf('[');
				if (bracketIndex > 0)
				{
					propertyIndexString = name.Substring(bracketIndex + 1, name.Length - bracketIndex - 2);
					name = name.Remove(bracketIndex);
				}

				PropertyInfo propertyInfo = modelType.GetProperty(name);
				model = model == null
					? null
					: propertyInfo.GetValue(model, null);

				if (propertyIndexString == null)
					modelType = propertyInfo.PropertyType;
				else
				{
					List<object> collection = ((IEnumerable<object>)model).ToList();
					int propertyIndex = int.Parse(propertyIndexString);
					model = collection[propertyIndex];
				}
			}

			parentObject = model;
			propertyName = propertyPathParts.Dequeue();
		}

		private async Task ValidateFieldAsync(
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
				new ValidationContext<object>(
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

		private static IEnumerable<IValidator> GetValidatorsForObject(
			object model,
			IServiceProvider serviceProvider)
		{
			var validatorTypesRepository = (FluentValidationRepository)serviceProvider.GetService(typeof(FluentValidationRepository));
			IEnumerable<Type> validatorTypes = validatorTypesRepository.GetValidatorTypesForObject(model);
			IEnumerable<IValidator> validators = validatorTypes.Select(x => (IValidator)serviceProvider.GetService(x));
			return validators;
		}
	}
}
