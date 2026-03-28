using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO.Hashing;

interface ReceiverConfig {};
struct TCPConfig : ReceiverConfig
{
    public readonly string host;
    public readonly ushort port;
    private byte[][] pgns;
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

    private byte[][] pgns =
  {
    [0x01, 0xF1, 0x12], // Vessel Heading
    [0x01, 0xF2, 0x00], // Engine Paremeters (Rapid)
    [0x01, 0xF5, 0x03], // Speed
    [0x01, 0xF5, 0x0B], // Water Depth
    [0x01, 0xF8, 0x01], // Position Update (Rapid)
    [0x01, 0xFD, 0x02], // Wind Data
  };

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

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
    {
        using (client)
        {
            var stream = client.GetStream();
            if (!stream.CanRead) throw new Exception("Unable to read from TCP stream");

            var buffer = new byte[4096];
            int bytesRead;
            bool validatedMessage = false;
            bool signedMessage = false;

            while ((bytesRead = await stream.ReadAsync(buffer, ct)) > 0)
            {
                var data = buffer[..bytesRead];
                byte[] decryptedData = Decryptor.decrypt(data);

                byte[] pgn = decryptedData.AsSpan(0, 3).ToArray();
                byte[] payload = decryptedData.AsSpan(3, decryptedData.Length - 3 - 8).ToArray();

                byte[] messageWithoutHash = decryptedData[..^8];
                byte[] receivedHash = decryptedData[^8..];
                byte[] computedHash = XxHash3.Hash(messageWithoutHash).Take(8).ToArray();

                signedMessage = receivedHash.SequenceEqual(computedHash);
                validatedMessage = pgns.Any(existingPgn => existingPgn.SequenceEqual(pgn));

                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Message received");
                Console.WriteLine($"PGN: {Convert.ToHexString(pgn)}");
                Console.WriteLine($"Payload: {Convert.ToHexString(payload)}");
                Console.WriteLine($"Decrypted Data: {Convert.ToHexString(decryptedData)}");
                Console.WriteLine($"MessageWithoutHash: {Convert.ToHexString(messageWithoutHash)}");
                Console.WriteLine($"ReceivedHash: {Convert.ToHexString(receivedHash)}");
                Console.WriteLine($"ComputedHash: {Convert.ToHexString(computedHash)}");
                Console.WriteLine($"signedMessage: {signedMessage}");
                Console.WriteLine("---------------------------------------------------------------");

                await _db.SaveMessageAsync(Convert.ToHexString(payload), signedMessage, validatedMessage, Convert.ToHexString(pgn));
            }
        }
    }

    public void Close()
    {
        listener.Stop();
    }
}