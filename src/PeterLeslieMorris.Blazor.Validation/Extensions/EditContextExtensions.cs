using Microsoft.AspNetCore.Components.Forms;
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

		public static void ValidateProperty(this EditContext editContext, FieldIdentifier fieldIdentifier)
		{
			if (fieldIdentifier.Model == null)
				return;

			var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(
				fieldIdentifier.FieldName,
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

			var validatedObjects = new HashSet<object>();
			ValidateProperty(editContext, fieldIdentifier.Model, propertyInfo, validatedObjects);
		}

		private static void ValidateObject(
			EditContext editContext,
			object instance,
			HashSet<object> validatedObjects)
		{
			if (instance == null)
				return;

			if (instance.GetType().Assembly == typeof(string).Assembly)
				return;

			if (validatedObjects.Contains(instance))
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
			if (GetFieldStateMethod == null)
			{
				GetFieldStateMethod = editContext.GetType().GetMethod(
					"GetFieldState",
					BindingFlags.NonPublic | BindingFlags.Instance);
			}

			var fieldIdentifier = new FieldIdentifier(instance, propertyName);
			object fieldState = GetFieldStateMethod.Invoke(editContext, new object[] { fieldIdentifier, true });

			if (IsModifiedProperty == null)
			{
				IsModifiedProperty = fieldState.GetType().GetProperty(
					"IsModified",
					BindingFlags.Public | BindingFlags.Instance);
			}

			object originalIsModified = IsModifiedProperty.GetValue(fieldState);
			editContext.NotifyFieldChanged(fieldIdentifier);
			IsModifiedProperty.SetValue(fieldState, originalIsModified);
		}

	}
}
