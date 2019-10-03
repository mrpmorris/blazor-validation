using Microsoft.AspNetCore.Components.Forms;
using System;

namespace PeterLeslieMorris.Blazor.Validation
{
	public interface IValidationProvider
	{
		void InitializeEditContext(EditContext editContext, IServiceProvider serviceProvider);
	}
}
