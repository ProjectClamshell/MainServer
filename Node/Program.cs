using System.Security.Cryptography;
using dotenv.net;

class ClamshellNode
{
  private static bool debug = false;

  private static byte[] key = [];
  private static byte[] nonce = [];
  private static string host = "";
  private static ushort port;

  private static bool running = true;

  static void Main(string[] args)
  {
    if (args.Contains("--genkey"))
    {
      var key = new byte[32];
      var nonce = new byte[24];

      RandomNumberGenerator.Create().GetBytes(key);
      RandomNumberGenerator.Create().GetBytes(nonce);

      Console.WriteLine("Key: " + Convert.ToHexString(key));
      Console.WriteLine("Nonce: " + Convert.ToHexString(nonce));
      return;
    }

    debug = args.Contains("--debug");
    DotEnv.Load();
    
    key = Convert.FromHexString(Environment.GetEnvironmentVariable("XCHACHA20POLY1305_KEY") ?? "");
    nonce = Convert.FromHexString(Environment.GetEnvironmentVariable("XCHACHA20POLY1305_NONCE") ?? "");
    host = Environment.GetEnvironmentVariable("TCP_HOST") ?? "";
    if (!ushort.TryParse(Environment.GetEnvironmentVariable("TCP_PORT") ?? "0", out port))
    {
      throw new Exception("Could not cast TCP_PORT to ushort");
    }

    XChaCha20Poly1305Encyption encryption = new(key, nonce);

    DummyNMEA2000Receiver receiver = new();

    TCPConfig config = new(host, port);
    TCPSender sender = new(config);
    
    Console.CancelKeyPress += (sender, e) =>
    {
      e.Cancel = true;
      running = false;
    };

    while (running)
    {
      byte[] msg = receiver.read();
      byte[] encryptedMsg = encryption.encrypt(msg);
      if (!sender.send(encryptedMsg)) {
        Console.WriteLine("Unabled to send message");
      }
      Thread.Sleep(500); // Send messages at 2Hz
    }

    sender.Close();  
  }
}