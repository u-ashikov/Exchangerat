namespace Exchangerat.Clients.Common.Constants
{
    public static class WebConstants
    {
        public class Messages
        {
            public const string TransactionCreated = "Transaction successfully created.";
            public const string UserIsNotAClient = "Sorry, this user is not a client.";
            public const string YouAreNotAClient = "Sorry, you are not a client of the platform!";
            public const string NoAccountsFound = "The are no accounts found.";
            public const string AccountNotFound = "Exchange account not found.";
            public const string TransactionFailure = "Transaction cannot be finished.";
            public const string SenderAccountDoesNotExist = "The Sender Account does not exist.";
            public const string InsufficientAmount = "Insufficient amount for this transaction.";
            public const string ReceiverAccountDoesNotExist = "The Receiver Account does not exist.";
            public const string TransactionBetweenSameAccounts = "You cannot create transaction between same accounts.";
            public const string SenderAccountIsNotActive = "Your account is inacive.";
            public const string ReceiverAccountIsNotActive = "The receiver account is inactive.";
        }
    }
}
