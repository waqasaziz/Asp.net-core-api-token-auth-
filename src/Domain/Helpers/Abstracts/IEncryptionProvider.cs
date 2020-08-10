namespace Domain.Helpers
{
    public interface IEncryptionProvider
    {
        string Decrypt(string cipherText);

        string Encrypt(string text);
    }
}
