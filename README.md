# Blazor-Validation


Blazor-Validation is a validation agnostic library for validating forms in Blazor - [Microsoft aspdotnet Blazor project]. 

## Installation
You can download the latest release / pre-release NuGet packages from the official NuGet pages:
 - [Blazor-Validation]
 - [Blazor-FluentValidation]

## Getting started
 1. Add a reference to PeterLeslieMorris.Blazor.Validation
 2. Inside the `<EditForm>` in your razor files, add `<PeterLeslieMorris.Blazor.Validation.Validate/>`
 3. In startup.cs add `using PeterLeslieMorris.Blazor.Validation` and then add the relevant validation in the `ConfigureServices` method.

 -  `services.AddFormValidation(config => config.AddDataAnnotationsValidation());`
 -  `services.AddFormValidation(config => config.AddFluentValidation();`

It is possible to add as many validation providers as you wish
```
services.AddFormValidation(config => 
  config
    .AddDataAnnotationsValidation()
    .AddFluentValidation()
);
```

The standard Blazor components `<ValidationSummary>` and `<ValidationMessage>` will now work with your selected validation options.



### Sample projects
More sample projects will be added as the framework develops.
  - [Data Annotations Sample] - Shows how to use DataAnnotations to validate.
  - [FluentValidation Sample] - Shows how to use the [FluentValidation.com] library to validate.

   [Microsoft aspdotnet blazor project]: <https://github.com/aspnet/Blazor>
   [Blazor-Validation]: <https://www.nuget.org/packages/PeterLeslieMorris.Blazor.Validation/>
   [Blazor-FluentValidation]: <https://www.nuget.org/packages/PeterLeslieMorris.Blazor.FluentValidation/>
   [Data Annotations Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/samples/01-DataAnnotationsValidation/>
   [FluentValidation Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/samples/02-FluentValidation/>>
