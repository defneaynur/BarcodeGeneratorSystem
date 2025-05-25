using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Api.Services.Secure;
using Core.Config.Config;
using Core.Config.Injection;
using System.Data;
using System.Data.SqlClient;


namespace BarcodeGeneratorSystem.Api.Base
{
    public static class ConfigureInjection
    {
        private static readonly IBaseInjection _baseInjection;
        public static void BaseInject(this WebApplicationBuilder builder)
        {
            builder.BaseDefaultInjection();

            var connectionString = builder.GetConfigFromAppSettings<ConfigProject>();
            builder.Services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString.ApiInformations.ConnectionStrings.DefaultConnection));
            builder.Services.AddScoped<IBarcodeProcessors, BarcodeProcessors>();
            builder.Services.AddScoped<IUserProcessors, UserProcessors>();
            builder.Services.AddScoped<IAuthProcessors, AuthProcessors>();
            builder.Services.AddScoped<IProductProcessors, ProductProcessors>();
            builder.Services.AddHttpClient<IErpProcessors, ErpProcessors>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7290/");
            });


        }

    }

}
