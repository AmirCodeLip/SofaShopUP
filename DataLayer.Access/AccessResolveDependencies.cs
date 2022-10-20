using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access
{
    public static class AccessResolveDependencies
    {
        public static IServiceCollection AddAccessServices(this IServiceCollection services)
        {
            var type = typeof(AccessResolveDependencies);
            var types = type.Assembly.GetTypes().Where(x => x.Namespace == "DataLayer.Access.Services");
            foreach (var repository in types.Where(n => n.Name.EndsWith("Repository")))
            {
                var baseName = repository.Name.Substring(1, repository.Name.Length -
                    "Repository".Length - 1);
                var serviceName = baseName + "Service";
                var service = types.FirstOrDefault(x => x.Name == serviceName);
                services.AddScoped(repository, service);
            }
            return services;
        }

    }
}
