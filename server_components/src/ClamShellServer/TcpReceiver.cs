using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;

public class TcpListenerService : BackgroundService
{
    string SharedPrivateKey = "SOME RANDOM GENERATED KEY";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new TcpListener(IPAddress.Any, 49152); // 49152 is a typically free port
        listener.Start();

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);

                _ = HandleClient(client, stoppingToken);
            }
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task HandleClient(TcpClient client, CancellationToken token)
    {
        using (client)
        await using (var stream = client.GetStream())
        {
            var message = $"📅 {DateTime.Now} 🕛";
            var bytes = Encoding.UTF8.GetBytes(message);

            await stream.WriteAsync(bytes, token);
        }
    }

    private void DecryptMessage(ref string message)
    {
        // eventually this will decrypt using the public key
        message = message;
    }
}