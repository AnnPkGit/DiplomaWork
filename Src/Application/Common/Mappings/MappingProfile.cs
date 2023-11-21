using System.Reflection;
using Application.Common.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    private readonly IServiceProvider? _serviceProvider;

    public MappingProfile()
    {
    }

    public MappingProfile(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        
        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
        
        var argumentTypes = new Type[] { typeof(Profile) };

        var iIncludeCurrentUserService = typeof(IIncludeCurrentUserService);
        foreach (var type in types)
        {
            object? instance;
            if (_serviceProvider != null && type.IsAssignableTo(iIncludeCurrentUserService))
            {
                instance = ActivatorUtilities.CreateInstance(_serviceProvider, type);
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }
            
            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
