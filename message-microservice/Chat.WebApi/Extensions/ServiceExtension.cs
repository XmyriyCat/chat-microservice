using Chat.Application.Infrastructure.Mapper;
using Chat.Application.Services.Contract;
using Chat.Application.Services.Implementation;
using Chat.Data.Context;
using Chat.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Scrutor;

namespace Chat.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigurePostgresDb(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Db_Connection_String")
                                   ?? throw new ArgumentNullException($"The postgres database connection string is null");

            services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
        }

        public static async Task MigrateDbAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetService<ChatDbContext>()
                          ?? throw new ArgumentNullException($"The postgres database context is null");

            await context.Database.MigrateAsync();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }

        public static void ConfigureServiceScrutor(this IServiceCollection services)
        {
            services.Scan
            (
                scan => scan
                    // Configure data services
                    .FromAssemblyOf<IRepositoryWrapper>()
                    .AddClasses(classes => classes.InNamespaceOf<IRepositoryWrapper>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()

                    // Configure application services
                    .FromAssemblyOf<MappingProfile>()
                    .AddClasses(classes => classes.InNamespaceOf<UserCatalogService>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );
        }
    }
}
