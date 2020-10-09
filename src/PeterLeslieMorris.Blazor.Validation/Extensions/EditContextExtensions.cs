using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PeterLeslieMorris.Blazor.Validation.Extensions
{
	public static class EditContextExtensions
	{
		static PropertyInfo IsModifiedProperty;
		static MethodInfo GetFieldStateMethod;

		/// <summary>
		/// Validates an entire object tree
		/// </summary>
		/// <param name="editContext">The EditContext to validate the Model of</param>
		/// <returns>True if valid, otherwise false</returns>
		public static bool ValidateObjectTree(this EditContext editContext)
		{
			var validatedObjects = new HashSet<object>();
			ValidateObject(editContext, editContext.Model, validatedObjects);
			editContext.NotifyValidationStateChanged();
			return !editContext.GetValidationMessages().Any();
		}

		public static bool ValidateProperty(
			this EditContext editContext,
			FieldIdentifier fieldIdentifier)
		{
			if (fieldIdentifier.Model == null)
				return false;

			var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(
				fieldIdentifier.FieldName,
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

			var validatedObjects = new HashSet<object>();
			ValidateProperty(editContext, fieldIdentifier.Model, propertyInfo, validatedObjects);

			return !editContext.GetValidationMessages(fieldIdentifier).Any();
		}

		public static bool ValidateProperties(
			this EditContext editContext,
			params FieldIdentifier[] properties)
		{
			if (properties == null || properties.Length == 0)
				throw new ArgumentNullException(nameof(properties));

			bool valid = true;
			foreach (FieldIdentifier property in properties)
				valid &= editContext.ValidateProperty(property);
			return valid;
		}

		private static void ValidateObject(
			EditContext editContext,
			object instance,
			HashSet<object> validatedObjects)
		{
			if (instance == null)
				return;

			if (validatedObjects.Contains(instance))
				return;

			if (instance is IEnumerable && !(instance is string))
			{
				foreach (object value in (IEnumerable)instance)
					ValidateObject(editContext, value, validatedObjects);
				return;
			}

			if (instance.GetType().Assembly == typeof(string).Assembly)
				return;

			validatedObjects.Add(instance);

			var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (PropertyInfo property in properties)
				ValidateProperty(editContext, instance, property, validatedObjects);
		}

		private static void ValidateProperty(
			EditContext editContext,
			object instance,
			PropertyInfo property,
			HashSet<object> validatedObjects)
		{
			NotifyPropertyChanged(editContext, instance, property.Name);

			object value = property.GetValue(instance);
			ValidateObject(editContext, value, validatedObjects);
		}

		private static void NotifyPropertyChanged(
			EditContext editContext,
			object instance,
			string propertyName)
		{

			var fieldIdentifier = new FieldIdentifier(instance, propertyName);

			var fieldState = GetFieldState(editContext, fieldIdentifier);

			if (IsModifiedProperty == null)
			{
				IsModifiedProperty = fieldState.GetType().GetProperty(
					"IsModified",
					BindingFlags.Public | BindingFlags.Instance);
			}

			var originalIsModified = IsModifiedProperty.GetValue(fieldState);
			editContext.NotifyFieldChanged(fieldIdentifier);
			IsModifiedProperty.SetValue(fieldState, originalIsModified);
		}

		private static Object GetFieldState(EditContext editContext, FieldIdentifier fieldIdentifier)
		{
#if NETSTANDARD2_1
			Object[] parameters = new object[] { fieldIdentifier, true };
#else
			Object[] parameters = new object[] { fieldIdentifier };
#endif
			EnsureGetFieldStateMethod(editContext);
			return GetFieldStateMethod.Invoke(editContext, parameters);
		}

		private static void EnsureGetFieldStateMethod(EditContext editContext)
		{
#if NETSTANDARD2_1
			var methodname = "GetFieldState";
#else
			var methodname = "GetOrAddFieldState";
#endif

			if (GetFieldStateMethod == null)
			{
				GetFieldStateMethod = editContext.GetType().GetMethod(methodname,
					BindingFlags.NonPublic | BindingFlags.Instance);
			}
		}
	}
}
