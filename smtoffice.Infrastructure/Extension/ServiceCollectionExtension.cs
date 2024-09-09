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

            var basePath = AppContext.BaseDirectory;

            var relativeConnectionString = configuration.GetConnectionString("LocalDbConnection");

            if (string.IsNullOrEmpty(relativeConnectionString))
            {
                throw new InvalidOperationException("Connection string 'LocalDbConnection' is not configured.");
            }
            var dbFullPath = Path.Combine(basePath, relativeConnectionString.Replace("Data Source=", ""));
            var connectionString = $"Data Source={dbFullPath}";


            //var connectionString = configuration.GetConnectionString("LocalDbConnection");
            Console.WriteLine(connectionString);
            services.AddScoped<ISqlConnectionFactory>(provider => new SqlConnectionFactory(connectionString!));
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IPasswordHasher, BcryptPasswordHasher>();
            services.AddTransient<DataSeeder>();
        }
    }
}
