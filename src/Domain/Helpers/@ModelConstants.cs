namespace Domain.Helpers
{
    public static class ModelConstants
    {
        public static class Merchant
        {
            public const int NameMaxLength = 50;
            public const int UserNameMaxLength = 50;
            public const int PasswordMaxLength = 50;
            public const int SaltMaxLength = 50;

        }

        public static class Card
        {
            public const int NumberMaxLength = 344;
            public const int NameMaxLength = 344;
            public const int ExpiryDateMaxLength = 344;
        }

        public static class Amount
        {
            public const int DescriptionMaxLength = 150;
            public const string MinStartingPrice = "0.01";
            public const string MaxStartingPrice = "79228162514264337593543950335";
            public const string ErrorMessage = "The Amount field cannot be lower than 0.01";

        }
    }
}