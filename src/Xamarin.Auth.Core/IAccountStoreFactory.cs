namespace Xamarin.Auth
{
    public interface IAccountStoreFactory
    {
        IAccountStore Create(char[] password = null);
    }
}
