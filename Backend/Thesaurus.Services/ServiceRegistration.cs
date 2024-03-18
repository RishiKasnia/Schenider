using Microsoft.Extensions.DependencyInjection;

namespace Thesaurus.Services
{
    public static class ServiceRegistration
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddTransient<IWordService, WordService>();
            services.AddTransient<ISynonymService, SynonymService>();

        }
    }
}
