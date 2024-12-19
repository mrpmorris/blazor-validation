# Blazor-Validation


Blazor-Validation is a validation agnostic library for validating forms in Blazor- [Microsoft aspdotnet Blazor project]. 

[![Join the chat at https://gitter.im/mrpmorris/blazor-validation](https://badges.gitter.im/mrpmorris/blazor-validation.svg)](https://gitter.im/mrpmorris/blazor-validation?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) 

## Installation
You can download the latest release / pre-release NuGet packages from the official NuGet pages:
- [Blazor-Validation] [![NuGet version (Morris.Blazor.Validation)](https://img.shields.io/nuget/v/Morris.Blazor.Validation.svg?style=flat-square)](https://www.nuget.org/packages/Morris.Blazor.Validation/)
- [Blazor-FluentValidation] [![NuGet version (Morris.Blazor.FluentValidation)](https://img.shields.io/nuget/v/Morris.Blazor.FluentValidation.svg?style=flat-square)](https://www.nuget.org/packages/Morris.Blazor.FluentValidation/) 

## Getting started
 1. Add a reference to Morris.Blazor.Validation
 2. Inside the `<EditForm>` in your razor files, add `<Morris.Blazor.Validation.Validate/>`
 3. In startup.cs add `using Morris.Blazor.Validation` and then add the relevant validation in the `ConfigureServices` method.

-  `services.AddFormValidation(config => config.AddDataAnnotationsValidation());`
-  `services.AddFormValidation(config => config.AddFluentValidation(typeof(SomeValidator).Assembly));`

It is possible to add as many validation providers as you wish
```c#
services.AddFormValidation(config => 
  config
    .AddDataAnnotationsValidation()
    .AddFluentValidation(typeof(SomeValidator).Assembly)
);
```

Also you can have the `FluentValidation` extension scan multiple assemblies

```c#
services.AddFormValidation(config => 
  config
    .AddFluentValidation(
      typeof(SomeValidator).Assembly,
      typeof(ClassInAnotherDll).Assembly,
      andAnotherAssembly,
      andYetAnotherAssembly));
```

The standard Blazor components `<ValidationSummary>` and `<ValidationMessage>` will now work with your selected validation options.

### Sample projects
More sample projects will be added as the framework develops.
 - [Data Annotations Sample]- Shows how to use DataAnnotations to validate.
 - [FluentValidation Sample]- Shows how to use the [FluentValidation.com] library to validate.

## What's new

### New in 3.1.1
- Support for .net 8 and 9 only

### New in 3.1.0
- Support for .Net 8

### New in 3.0.0 
- Add OnTransformModel delegate to allow the model to be transformed before validation (This is useful when using endpoint-centric APIs)
- Major change because it changes the signature of the public IValidationProvider interface.

### New in 2.0.0
- Add net7.0 target framework.

### New in 1.8.0
- Use `Services.TryAddScoped` instead of `Services.AddScoped` for validators, in case 
  the consuming app has already registered validators with a different lifetime.

### New in 1.7.0
- Upgrade to FluentValidation V10
- Prevent ValidateObjectTree from visiting struct properties [Bug #33](https://github.com/mrpmorris/blazor-validation/issues/33)

### New in 1.6.0
- Suport FluentValidation's RuleForEach and ChildRules

### New in 1.5.0
- Support .NET 5.0

### New in 1.4.0
- Upgrade to FluentValidation 9

### New in 1.3.1
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
   [Blazor-Validation]: <https://www.nuget.org/packages/Morris.Blazor.Validation/>
   [Blazor-FluentValidation]: <https://www.nuget.org/packages/Morris.Blazor.FluentValidation/>
   [Data Annotations Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/Source/Samples/01-DataAnnotationsValidation//>
   [FluentValidation Sample]: <https://github.com/mrpmorris/blazor-validation/tree/master/Source/Samples/02-FluentValidation/>
   [Blazored FluentValidation]: <https://github.com/Blazored/FluentValidation>
