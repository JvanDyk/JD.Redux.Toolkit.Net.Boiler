using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JD.Redux.Toolkit.Net.Boiler;

public static class ServiceCollectionExtensions
{
    public static void GenerateReduxService(this IServiceCollection services)
    {
        var generateRedux = new GenerateRedux();
        generateRedux.Build();
    }
}
