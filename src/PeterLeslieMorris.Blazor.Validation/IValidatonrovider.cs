using Microsoft.AspNetCore.Components.Forms;

namespace PeterLeslieMorris.Blazor.Validation
{
	public interface IValidationProvider
	{
		void InitializeEditContext(EditContext editContext);
	}
}
