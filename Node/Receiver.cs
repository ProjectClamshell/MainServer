using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

interface IReceiver
{
  public byte[] read();
}

class DummyNMEA2000Receiver : IReceiver
{
  private bool hasGeneratedAddrClaim = false;
  private Random rand = new Random();

  // These are the only PGNs that will be used when generating mock data
  private byte[][] pgns =
  {
    [0x01, 0xF1, 0x12], // Vessel Heading
    [0x01, 0xF2, 0x00], // Engine Paremeters (Rapid)
    [0x01, 0xF5, 0x03], // Speed
    [0x01, 0xF5, 0x0B], // Water Depth
    [0x01, 0xF8, 0x01], // Position Update (Rapid)
    [0x01, 0xFD, 0x02], // Wind Data
  };

  // TODO: possibly implement unimplemented fields, and add more PGNs

  public byte[] read()
  {
    // In this case it would be ideal to create a struct to make it easy to understand, however this simulates packets right from a NMEA2000 sensor, so directly using bits will be handled.
    // NOTE: for messages with 0xFF..0xFF as their value, assume it is unavailable
    List<byte> nmeaMessage = new List<byte>();

    // First message will be a Address Claim to simulating powering on a sensor
    if (!hasGeneratedAddrClaim)
    {
      nmeaMessage.AddRange([0x00, 0xEE, 0x00]);
      byte[] address = new byte[8];
      RandomNumberGenerator.Create().GetBytes(address);
      nmeaMessage.AddRange(address);
      hasGeneratedAddrClaim = true;
    } else
    {
      byte[] pgn = pgns[rand.Next(pgns.Length)];
      nmeaMessage.AddRange(pgn);
      switch (pgn)
      {
        case [0x01, 0xF1, 0x12]:
          double deg = rand.NextDouble() * 360;
          double rad = deg * Math.PI / 180;
          ushort heading = (ushort)(rad * 10000);
          nmeaMessage.AddRange([0x01, (byte)(heading & 0xFF), (byte)(heading >> 8), 0xFF, 0xFF, 0xFF, 0xFF, 0x00]);
          break;
        case [0x01, 0xF2, 0x00]:
          ushort engineSpeed = (ushort)((2750 + (rand.NextDouble() * 750))*4);
          nmeaMessage.AddRange([0x00, (byte)(engineSpeed & 0xFF), (byte)(engineSpeed >> 8), 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]);
          break;
        case [0x01, 0xF5, 0x03]:
          ushort waterSpeed = (ushort)(800 + (rand.NextDouble() * 400));
          nmeaMessage.AddRange([(byte)(waterSpeed & 0xFF), (byte)(waterSpeed >> 8), 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0xFF]);
          break;
        case [0x01, 0xF5, 0x0B]:
          uint depth = (uint)(500 + (rand.NextDouble() * 3500));
          nmeaMessage.AddRange([(byte)(depth & 0xFF), (byte)((depth >> 8) & 0xFF), (byte)((depth >> 16) & 0xFF), (byte)(depth >> 24), 0xFF, 0xFF, 0xFF, 0xFF]); 
          break;
        case [0x01, 0xF8, 0x01]:
          int lat = (int)((double)(47.56 + rand.NextDouble() * 0.02) * 1e7);
          int lon = (int)((double)(-52.72 + rand.NextDouble() * 0.02) * 1e7);
          nmeaMessage.AddRange([(byte)(lat & 0xFF), (byte)((lat >> 8) & 0xFF), (byte)((lat >> 16) & 0xFF), (byte)((lat >> 24) & 0xFF), (byte)(lon & 0xFF), (byte)((lon >> 8) & 0xFF), (byte)((lon >> 16) & 0xFF), (byte)((lon >> 24) & 0xFF)]);
          break;
        case [0x01, 0xFD, 0x02]:
          ushort speed = (ushort)(rand.NextDouble() * 1500);
          ushort angle = (ushort)(rand.NextDouble() * 2 * Math.PI * 10000);
          nmeaMessage.AddRange([0x01, (byte)(speed & 0xFF), (byte)(speed >> 8), (byte)(angle & 0xFF), (byte)(angle >> 8), 0x00, 0xFF, 0xFF]);
          break;
        default:
          throw new Exception("Invalid PGN selected when picking mock PGN");
      }
    }

    return nmeaMessage.ToArray();
  }
}