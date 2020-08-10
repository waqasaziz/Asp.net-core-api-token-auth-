namespace Domain.Helpers
{
    public interface IEncryptionKeyProvider
    {
        string PublicKey { get; }

        string PrivateKey { get; }

    }
}
