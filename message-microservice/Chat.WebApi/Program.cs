using Chat.WebApi.Extensions;

namespace Chat.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigurePostgresDb(builder.Configuration);

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            await app.MigrateDbAsync();
            app.UseAuthorization();
            
            app.MapControllers();

            await app.RunAsync();
        }
    }
}