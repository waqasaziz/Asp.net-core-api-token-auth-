namespace Domain.Helpers
{
    public static class ModelConstants
    {
        public static class Merchant
        {
            public const int NameMaxLength = 50;
            public const int UserNameMaxLength = 50;

            public const int PasswordMaxLength = 15;
            public const int PasswordMinLength = 6;

            public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$";

            public const string PasswordErrorMessage =
                "Password minimum length is 6 characters and it must contain at least one uppercase letter, one lowercase letter and one number.";
        }

        public static class Card
        {
            public const int NumberMaxLength = 16;
            public const int NameMaxLength = 50;
            public const int ExpiryDateMaxLength = 5;
            public const int SecurityCodeMaxLength = 3;
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