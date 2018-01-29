// Unable to resolve assembly 'Assembly(Name=System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth.XamarinForms, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/XamarinForms/Xamarin.Auth.Forms/bin/Debug/Xamarin.Auth.XamarinForms.dll)'.
namespace Xamarin.Auth.XamarinForms
{
    [Xamarin.Auth.XamarinForms.PreserveAttribute(AllMembers=true)]
    // Unable to resolve assembly 'Assembly(Name=Xamarin.Forms.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null)' referenced by 'Assembly(Name=Xamarin.Auth.XamarinForms, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/XamarinForms/Xamarin.Auth.Forms/bin/Debug/Xamarin.Auth.XamarinForms.dll)'.
    public partial class AuthenticatorPage : Xamarin.Forms.ContentPage
    {
        public AuthenticatorPage() { }
        public AuthenticatorPage(Xamarin.Auth.Authenticator a) { }
        public Xamarin.Auth.Authenticator Authenticator 
        { 
            [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get 
            { 
                //Unable to resolve assembly 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null)' referenced by 'Assembly(Name=Xamarin.Auth.XamarinForms, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/XamarinForms/Xamarin.Auth.Forms/bin/Debug/Xamarin.Auth.XamarinForms.dll)'.
                return default(Xamarin.Auth.Authenticator); 
            }
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void Authentication_BrowsingCompleted(object sender, System.EventArgs e) { }
        public void Authentication_Completed(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e) { }
        public void Authentication_Error(object sender, Xamarin.Auth.AuthenticatorErrorEventArgs e) { }
        protected override void OnAppearing() { }
        protected override void OnDisappearing() { }
    }
    public partial class EmbeddedWebViewConfiguration
    {
        public EmbeddedWebViewConfiguration() { }
        public bool IsUsingWKWebView { get { return default(bool); } set { } }
        public string UserAgent { get { return default(string); } set { } }
    }
    public partial interface IEmbeddedWebViewConfiguration
    {
        bool IsUsingWKWebView { get; set; }
        string UserAgent { get; set; }
    }
    public sealed partial class PreserveAttribute : System.Attribute
    {
        public bool AllMembers;
        public bool Conditional;
        public PreserveAttribute() { }
    }
}
