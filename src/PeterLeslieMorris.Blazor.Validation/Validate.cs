using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class Validate : ComponentBase
	{
		[CascadingParameter] EditContext CurrentEditContext { get; set; }
		[Inject] IValidationProviderRepository Repository { get; set; }
		[Inject] IServiceProvider ServiceProvider { get; set; }

		private List<IValidationProvider> ValidationProviders = new List<IValidationProvider>();

		public async ValueTask<bool> ValidateAsync()
		{
			CurrentEditContext.Validate();
			foreach (IValidationProvider validationProvider in ValidationProviders)
			{
				await validationProvider.ValidationComplete;
				Console.WriteLine("Provider completed validation");
			}
			return !CurrentEditContext.GetValidationMessages().Any();
		}

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			EditContext previousEditContext = CurrentEditContext;

			await base.SetParametersAsync(parameters);

			if (CurrentEditContext == null)
				throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
						$"parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} " +
						$"inside an {nameof(EditForm)}.");

			if (CurrentEditContext != previousEditContext)
				EditContextChanged();
		}

		private void EditContextChanged()
		{
			foreach (Type providerType in Repository.All)
			{
				var validationProvider = (IValidationProvider)ServiceProvider.GetService(providerType);
				ValidationProviders.Add(validationProvider);
				validationProvider.InitializeEditContext(CurrentEditContext, ServiceProvider);
			}
		}
	}
}
