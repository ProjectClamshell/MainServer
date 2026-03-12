using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

class ClamshellServer
{
        static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddHostedService<TcpListenerService>();

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}