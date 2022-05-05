using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace PeterLeslieMorris.Blazor.Validation
{
	public interface IValidationProvider
	{
		void InitializeEditContext(EditContext editContext, IServiceProvider serviceProvider);
		Task ValidationComplete { get; }
	}
}
