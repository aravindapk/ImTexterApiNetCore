using ImTexterApi.Services.ServiceAttribute;

namespace ImTexterApi.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Define types that need matching
            Type scopedRegistration = typeof(ScopedRegistrationAttribute);
            //ToDo: Add Transient and Singleton if necessary.

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsDefined(scopedRegistration, false)  && !p.IsInterface)
                .Select(s => new
                {
                    Service = s.GetInterface($"I{s.Name}"),
                    Implementation = s
                })
                .Where(x => x.Service != null);


            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(scopedRegistration, false))
                {
                    _ = services.AddScoped(serviceType: type.Service, type.Implementation);
                }

            }
        }
    }
}
