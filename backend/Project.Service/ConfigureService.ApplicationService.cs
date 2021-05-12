using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Project.Service
{
    public class ConfigureServiceApplicationService
    {
        public static void Configure(IServiceCollection services)
        {
            var assemblyToScan = Assembly.GetExecutingAssembly();
            foreach (var type in assemblyToScan.ExportedTypes)
            {
                services.AddScoped(type);
            }
        }
    }
}
