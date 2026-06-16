using Application.Interfaces;
using Application.Services;
using Infrastructure.Database;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDonationHubServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString ?? string.Empty));

            return services;
        }
    }
}
