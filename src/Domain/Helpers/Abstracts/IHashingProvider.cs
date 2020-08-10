namespace Domain.Helpers
{
    public interface IHashingProvider
    {
        string GenerateSalt();
        string GenerateHash(string plainText, string salt);
        bool Validate(string plainText, string salt, string hash);
    }
}
