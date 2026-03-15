using Sodium;

interface IDecryption
{
    public byte[] decrypt(byte[] data);
}

class XChaCha20Poly1305Decryption : IDecryption
{
    private readonly byte[] key, nonce;

    public XChaCha20Poly1305Decryption(byte[] key, byte[] nonce)
    {
        if (key.Length != 32)
        {
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

    public byte[] decrypt(byte[] data)
    {
        return SecretAeadXChaCha20Poly1305.Decrypt(data, nonce, key);
    }
}