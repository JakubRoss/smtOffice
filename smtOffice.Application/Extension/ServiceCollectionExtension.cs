using Microsoft.Extensions.DependencyInjection;
using smtOffice.Application.Interfaces;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Application.Services;

namespace smtOffice.Application.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient(typeof(IDropDownService<>), typeof(DropDownService<>));
            services.AddTransient<IProjectService, ProjectService>();
        }
    }
}
