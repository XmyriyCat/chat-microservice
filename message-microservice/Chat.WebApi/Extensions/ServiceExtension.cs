using Chat.Data.Context;
using Microsoft.EntityFrameworkCore;

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
    }
}
