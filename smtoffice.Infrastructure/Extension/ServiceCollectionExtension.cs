using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smtoffice.Infrastructure.Common;
using smtoffice.Infrastructure.Interfaces;
using smtoffice.Infrastructure.Repository;
using smtOffice.Application.Interfaces;
using smtOffice.Application.Interfaces.Repository;

namespace smtoffice.Infrastructure.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            var connectionString = configuration.GetConnectionString("LocalDbConnection");
            services.AddScoped<ISqlConnectionFactory>(provider => new SqlConnectionFactory(connectionString!));
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IPasswordHasher, BcryptPasswordHasher>();
        }
    }
}
