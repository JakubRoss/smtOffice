using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace smtoffice.Infrastructure.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            var connectionString = configuration.GetConnectionString("LocalDbConnection");
        }
    }
}
