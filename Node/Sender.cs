using System.Net.Sockets;

interface SenderConfig {};

struct TCPConfig : SenderConfig {
  public readonly string host;
  public readonly ushort port;

  public TCPConfig(string host, ushort port)
  {
    this.host = host;
    this.port = port;
  }
};

interface ISender
{
  public bool send(byte[] data);
}

class TCPSender : ISender
{
  private readonly TCPConfig cfg;

  private readonly TcpClient client;
  private readonly NetworkStream stream;

  public TCPSender(TCPConfig cfg)
  {
    if (cfg.host == "")
    {
      throw new Exception("Invalid TCP Host");
    }
    if (cfg.port == 0)
    {
      throw new Exception("Invalid TCP Port");
    }

    this.cfg = cfg;

    client = new TcpClient(cfg.host, cfg.port);

    if (!client.Connected)
    {
      throw new Exception("Couldn't connect to client");
    }

    stream = client.GetStream();

    if (!stream.CanWrite)
    {
      throw new Exception("Unable to write to TCP stream");
    }
  }

  public bool send(byte[] data)
  {
    try
    {
      stream.Write(data, 0, data.Length);
      return true;
    } catch
    {
      return false;
    }
  }

  public void Close()
  {
    stream.Dispose();
    client.Close();
  }
}