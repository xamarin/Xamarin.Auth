namespace Xamarin.Auth
{
    public partial class AccountStoreException : AuthException
    {
        public string Operation { get; } = "N/A";

        public AccountStoreException(string operation)
            : base(operation)
        {
            Operation = operation;
        }

        public AccountStoreException(string operation, System.Exception exc)
            : base(operation, exc)
        {
        }
    }
}
