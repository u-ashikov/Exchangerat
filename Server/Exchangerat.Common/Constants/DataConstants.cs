namespace Exchangerat.Common.Constants
{
    public static class DataConstants
    {
        public const string DbName = "ExchangeratDbConnection";

        public const int ExchangeAccountIdentifierMaxLength = 10;

        public const int ExchangeAccountTypeNameMaxLength = 30;
        public const int ExchangeAccountTypeDescriptionMaxLength = 300;

        public const int UserFirstNameMinLength = 1;
        public const int UserFirstNameMaxLength = 20;
        public const int UserLastNameMinLength = 1;
        public const int UserLastNameMaxLength = 20;
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 20;
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 10;
        public const int UserEmailMinLength = 5;
        public const int UserEmailMaxLength = 80;
        public const int UserAddressMinLength = 4;
        public const int UserAddressMaxLength = 100;

        public const int TransactionDescriptionMaxLength = 300;
        public const double TransactionMinAmount = 1d;
        public const double TransactionMaxAmount = 10000d;

        public const string AdminUserName = "exchangerat";
        public const string AdminPass = "asddsa";
        public const string AdminEmail = "exchangerat.ms@gmail.com";
        public const string AdminAddress = "Sofia, Slivnica str. 123";
        public const string AdminFirstName = "Gergan";
        public const string AdminLastName = "Gerganov";

        public const string ModelPropertyMinLengthErrorMessage = "The {0} must be a string with minimum length of {1}.";
        public const string ModelPropertyMaxLengthErrorMessage = "The {0} must be a string with maximum length of {1}.";
    }
}
