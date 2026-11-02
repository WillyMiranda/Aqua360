using Aqua360.Data;
using Aqua360.Models;
using Microsoft.Extensions.Logging;
using Aqua360.Services;
using Aqua360.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Aqua360
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
#if DEBUG
            builder.Configuration.AddUserSecrets(typeof(MauiProgram).Assembly, optional: true);
#endif
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton<ICustomerRepository>(_ => new CustomerRepository(connectionString));
            builder.Services.AddValidatorsFromAssemblyContaining<CustomerSaveRequestValidator>();
            builder.Services.AddSingleton<ICustomersService, CustomersService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
