# Blazor-Validation


Blazor-Validation is a validation agnostic library for validating forms in Blazor- [Microsoft aspdotnet Blazor project]. 

## Installation
You can download the latest release / pre-release NuGet packages from the official NuGet pages:
- [Blazor-Validation] [![NuGet version (PeterLeslieMorris.Blazor.Validation)](https://img.shields.io/nuget/v/PeterLeslieMorris.Blazor.Validation.svg?style=flat-square)](https://www.nuget.org/packages/PeterLeslieMorris.Blazor.Validation/)
- [Blazor-FluentValidation] [![NuGet version (PeterLeslieMorris.Blazor.FluentValidation)](https://img.shields.io/nuget/v/PeterLeslieMorris.Blazor.FluentValidation.svg?style=flat-square)](https://www.nuget.org/packages/PeterLeslieMorris.Blazor.FluentValidation/) 

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
 - [Data Annotations Sample]- Shows how to use DataAnnotations to validate.
 - [FluentValidation Sample]- Shows how to use the [FluentValidation.com] library to validate.

## What's new
### New in 1.3.0
- Add new EditContext.ValidateProperties for validating sub-sets of an object

### New in 1.2.0
- Return `bool` from EditContext.ValidateProperty

### New in 1.0.0
- Updated FluentValidationSample
- First major release

### New in 0.10
- Remove old EditContextExtensions file
- Ensure strings are not enumerated when traversing a whole object tree to validate

### New in 0.9
- Upgraded to .NETCore 3

### New in 0.8
- Upgraded to Blazor RC1

### New in 0.7
- Added an `EditContext.ValidateObjectTree`
- Upgraded to Blazor Preview 9

### New in 0.6.1
- Fixed bug in FluentValidation that prevented objects with complex property types from being validated

### New in 0.6.0
- Upgraded to Blazor Preview 8

### New in 0.5.0
- Upgraded to Blazor Preview 7

### New in 0.4.0
- Initial public release

   [Microsoft aspdotnet blazor project]: <https://github.com/aspnet/Blazor>
   [Blazor-Validation]: <https://www.nuget.org/packages/PeterLeslieMorris.Blazor.Validation/>
   [Blazor-FluentValidation]: <https://www.nuget.org/packages/PeterLeslieMorris.Blazor.FluentValidation/>
   [Data Annotations Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/samples/01-DataAnnotationsValidation/>
   [FluentValidation Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/samples/02-FluentValidation/>
