using System.Text;
using System.Net;
using System.Net.Sockets;

interface ReceiverConfig {};
struct TCPConfig : ReceiverConfig
{
    public readonly string host;
    public readonly ushort port;
    public TCPConfig(string host, ushort port)
    {
        this.host = host;
        this.port = port;
    }
};

public class TcpListenerService : BackgroundService
{
    private static readonly byte[] key = Convert.FromHexString(Environment.GetEnvironmentVariable("XCHACHA20POLY1305_KEY") ?? throw new InvalidOperationException("XCHACHA20POLY1305_KEY not set"));
    private static readonly byte[] nonce = Convert.FromHexString(Environment.GetEnvironmentVariable("XCHACHA20POLY1305_NONCE") ?? throw new InvalidOperationException("XCHACHA20POLY1305_NONCE not set"));
    private static readonly string host = Environment.GetEnvironmentVariable("TCP_HOST") ?? "127.0.0.1";
    private static readonly ushort port = ushort.Parse(Environment.GetEnvironmentVariable("TCP_PORT") ?? "9000");

    private readonly TCPConfig cfg;
    private readonly TcpListener listener;
    private static readonly Database _db = new Database();
    private static readonly XChaCha20Poly1305Decryption Decryptor = new XChaCha20Poly1305Decryption(key, nonce);

    public TcpListenerService()
    {
        if (host == "") throw new Exception("Invalid TCP Host");
        if (port == 0) throw new Exception("Invalid TCP Port");

        cfg = new TCPConfig(host, port);
        listener = new TcpListener(IPAddress.Parse(cfg.host), cfg.port);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        listener.Start();
        Console.WriteLine($"TCP listener started on {cfg.host}:{cfg.port}");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                Console.WriteLine("Client connected");

                _ = HandleClientAsync(client, stoppingToken)
                    .ContinueWith(t => Console.Error.WriteLine(t.Exception),
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        listener.Stop();
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct) //probably need to change this, add support for different kinds of messages
    {
        using (client)
        {
            var stream = client.GetStream();
            if (!stream.CanRead) throw new Exception("Unable to read from TCP stream");

            var buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, ct);
            var data = buffer[..bytesRead];
            //char[] decryptedData = Decryptor.decrypt(data);
            char[] decryptedData = Encoding.UTF8.GetString(data).ToCharArray();
            char[] pgn = decryptedData.AsSpan(0, 3).ToArray();
            char[] payload = decryptedData.AsSpan(3).ToArray();
            Console.WriteLine("Message received");
            Console.WriteLine($"PGN: {new string(pgn)}");
            Console.WriteLine($"Payload: {new string(payload)}");
            await _db.SaveMessageAsync(new string(payload), false);
        }
    }

    public void Close()
    {
        listener.Stop();
    }
}