using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

class ClamshellServer
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        builder.Services.AddHostedService<TcpListenerService>();

        var app = builder.Build();

        app.UseCors("AllowAll");

        app.MapControllers();
        app.Run();
    }
}