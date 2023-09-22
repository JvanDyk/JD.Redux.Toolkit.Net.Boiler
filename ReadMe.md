

:warning: **This software is provided as is** :warning:

# JD.Redux.Toolkit.NET.Boiler

JD.Redux.Toolkit.NET.Boiler is attempts to generate boilerplate Redux store, thunks, and slices for .NET REST APIs.

## Table of Contents

- [Motivation](#motivation)
- [Installation](#installation)
- [Quick start](#quick-start)
- [License](#license)

## Motivation

After working on several React / Native projects, I fell in love with React simplicity, and soon after I started using Redux for application state management.
Now that I'm sticking with React I thought it would be a great idea to be able to generate thunks and slices for my .Net APIs so that I waste less time writing code.
This is my first Nuget package ever and I would love for you to improve on it, so feel free to contribute, thanks.

## Installation

You can grab the latest [JD.Redux.Toolkit.NET.Boiler Nuget package](https://www.nuget.org/packages/JD.Redux.Toolkit.NET.Boiler/) or from the NuGet package manager console :

    Install-Package JD.Redux.Toolkit.NET.Boiler

## Quick-start

```C#
 public void ConfigureServices(IServiceCollection services)
        {
        ...
        services.GenerateReduxService();
        }
```
Use it to generate a Redux folder in your API project. Then you can remove the library again, or write improvement code, thank you :)
Have a look at the code if you want to see what is really happening.

There are also some output examples in the Example folder.
## License

MIT
