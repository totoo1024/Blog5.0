using System;
using System.Collections.Generic;
using System.Linq;
using App.Framwork.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace App.Framwork.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 自动注入（和AutofacModule二选一即可）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoDependencyInjection(this IServiceCollection services)
        {
            //获取所有程序集
            var assemblys = Storage.Assemblys;

            //获取继承需要依赖注入的类型
            var types = assemblys.SelectMany(x => x.GetTypes()).Where(x => (typeof(IScopedDependency).IsAssignableFrom(x)
                                                                           || typeof(ISingletonDependency).IsAssignableFrom(x)
                                                                           || typeof(ITransientDependency).IsAssignableFrom(x))
                                                                           && x.IsClass && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types)
            {
                if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    services.TryAddTransient(type);
                    var interfaceType = type.GetInterfaces().FirstOrDefault(x => typeof(ITransientDependency) != x);
                    if (interfaceType != null)
                    {
                        services.TryAddTransient(interfaceType, type);
                    }
                    continue;
                }
                if (typeof(IScopedDependency).IsAssignableFrom(type))
                {
                    services.TryAddScoped(type);
                    var interfaceTypes = type.GetInterfaces().Where(x => typeof(IScopedDependency) != x);
                    foreach (var iType in interfaceTypes)
                    {
                        services.TryAddScoped(iType, type);
                    }
                    continue;
                }
                if (typeof(ISingletonDependency).IsAssignableFrom(type))
                {
                    services.TryAddSingleton(type);
                    var interfaceType = type.GetInterfaces().FirstOrDefault(x => typeof(ISingletonDependency) != x);
                    if (interfaceType != null)
                    {
                        services.TryAddSingleton(interfaceType, type);
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// 注入配置(支持热更新)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var types = Storage.Assemblys.SelectMany(x => x.GetTypes().Where(t => typeof(IConfig).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract && !t.IsGenericType));
            var method = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(nameof(OptionsConfigurationServiceCollectionExtensions.Configure), new[] { typeof(IServiceCollection), typeof(IConfiguration) });
            foreach (var item in types)
            {
                method?.MakeGenericMethod(item).Invoke(null, new object[] { services, configuration.GetSection(item.Name) });

                //var attr = item.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(SectionAttribute)) as SectionAttribute;
                //if (attr == null)
                //{

                //    var config = configuration.GetSection(item.Name).Get(item);
                //    services.AddScoped(config.GetType(), x => config);

                //}
                //else
                //{
                //    var config = configuration.GetSection(attr.Section).Get(item);
                //    switch (attr.Lifetime)
                //    {
                //        case Lifetime.Transient:
                //            services.AddTransient(config.GetType(), x => config);
                //            break;
                //        case Lifetime.Scoped:
                //            services.AddScoped(config.GetType(), x => config);
                //            break;
                //        case Lifetime.Singleton:
                //            services.AddSingleton(config.GetType(), x => config);
                //            break;
                //    }
                //}
            }
            return services;
        }
    }
}