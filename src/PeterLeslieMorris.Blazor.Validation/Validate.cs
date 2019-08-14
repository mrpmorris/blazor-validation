using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace PeterLeslieMorris.Blazor.Validation
{
	public class Validate : ComponentBase
	{
		[CascadingParameter] EditContext CurrentEditContext { get; set; }
		[Inject] IValidationProviderRepository Repository { get; set; }
		[Inject] IServiceProvider ServiceProvider { get; set; }

		protected override void OnInitialized()
		{
			if (CurrentEditContext == null)
			{
				throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
						$"parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} " +
						$"inside an {nameof(EditForm)}.");
			}
			foreach (Type providerType in Repository.All)
			{
				var validationProvider = (IValidationProvider)ServiceProvider.GetService(providerType);
				validationProvider.InitializeEditContext(CurrentEditContext, ServiceProvider);
			}
		}
	}
}
