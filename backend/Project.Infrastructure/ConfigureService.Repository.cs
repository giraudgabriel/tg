using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Project.Infrastructure
{
    public class ConfigureServiceRepository
    {
        public static void Configure(IServiceCollection services)
        {
            var assemblyToScan = Assembly.GetExecutingAssembly();
            foreach (var type in assemblyToScan.ExportedTypes)
            {
                if (!type.IsAbstract && !type.IsGenericType && type.Name.EndsWith("Repository"))
                    services.AddScoped(type);
            }
        }
    }
}
