using Microsoft.Extensions.DependencyInjection;
using Thesaurus.DAL.Interfaces;
using Thesaurus.DAL.Reposirories;

namespace Thesaurus.DAL
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ISynonymRepository, SynonymRepository>();
            services.AddTransient<IWordRepository, WordRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IDalSession, DalSession>();
        }
    }
}
