using Chat.Application.SignalR.Hubs;
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
            builder.Services.ConfigureAutoMapper();
            builder.Services.ConfigureServiceScrutor();
            builder.Services.ConfigureSignalR();
            builder.Services.ConfigureCorsPolicy();

            var app = builder.Build();

            app.UseCors("ClientPermission");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            await app.MigrateDbAsync();
            app.UseAuthorization();
            
            app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/hubs/chat");
            });

            await app.RunAsync();
        }
    }
}