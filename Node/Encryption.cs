using Sodium;

interface IEncryption
{
  public byte[] encrypt(byte[] data);
}

class XChaCha20Poly1305Encyption : IEncryption
{
  private readonly byte[] key, nonce;

  public XChaCha20Poly1305Encyption(byte[] key, byte[] nonce)
  {
    if (key.Length != 32) {
      throw new Exception("Key length should be 32 bytes");
    } else
    {
      this.key = key;
    }
    if (nonce.Length != 24)
    {
      throw new Exception("Nonce length should be 24 bytes");
    } else
    {
      this.nonce = nonce;
    }
  }

  public byte[] encrypt(byte[] data)
  {
    return SecretAeadXChaCha20Poly1305.Encrypt(data, nonce, key);
  }
}