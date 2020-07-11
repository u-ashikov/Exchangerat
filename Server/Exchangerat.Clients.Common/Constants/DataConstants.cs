namespace Exchangerat.Clients.Common.Constants
{
    public static class DataConstants
    {
        public const int ClientFirstNameMinLength = 1;
        public const int ClientFirstNameMaxLength = 20;
        public const int ClientLastNameMinLength = 1;
        public const int ClientLastNameMaxLength = 20;
        public const int ClientAddressMinLength = 4;
        public const int ClientAddressMaxLength = 100;
        public const int ClientUserIdMaxLength = 40;

        public const int ExchangeAccountIdentifierMaxLength = 10;

        public const int ExchangeAccountTypeNameMaxLength = 30;
        public const int ExchangeAccountTypeDescriptionMaxLength = 300;

        public const int TransactionDescriptionMaxLength = 300;
        public const double TransactionMinAmount = 1d;
        public const double TransactionMaxAmount = 10000d;
    }
}
