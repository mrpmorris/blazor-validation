namespace FluentValidationNet5
{
    using FluentValidationNet5.Models;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using PeterLeslieMorris.Blazor.Validation;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddFormValidation(config => config.AddFluentValidation(typeof(Person).Assembly));

            await builder.Build().RunAsync();
        }
    }
}
