namespace Xamarin.Auth
{
    internal interface IConnectionCallback
    {
        void OnCustomTabsConnected();

        void OnCustomTabsDisconnected();
    }
}
