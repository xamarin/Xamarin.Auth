using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Xamarin.Auth.ProviderSamples
{
    /// <summary>
    ///		Google:		OAuth2
    ///		Facebook:	OAuth2
    ///					https://developers.facebook.com/apps/
    ///					https://developers.facebook.com/
    ///		Twitter:	OAuth1
    ///					https://apps.twitter.com/apps
    ///					https://dev.twitter.com/
    ///					https://dev.twitter.com/oauth
    ///		LinkedIn:	OAuth1
    ///					https://www.linkedin.com/secure/developer
    ///					https://developer.linkedin.com/
    ///		Instagram:	OAuth2
    ///					https://instagram.com/developer/
    ///					http://instagram.com/developer/authentication/
    /// </summary>
    /// <see cref="http://forums.xamarin.com/discussion/36687/how-to-pass-paramaters-to-custom-renderers"/>
    /// <see cref="https://hedgehogjim.wordpress.com/2015/01/29/simplify-using-xamarin-auth-with-async-tasks-a-twitter-example/"/>
    /// <see cref="http://thirteendaysaweek.com/2013/04/25/xamarin-ios-and-authentication-in-windows-azure-mobile-services-part-i-configuration/"/>
    /// <see cref="http://chrisrisner.com/Authentication-with-Windows-Azure-Mobile-Services"/>
    /// <see cref="http://alejandroruizvarela.blogspot.com/2014/03/xamarinauth-custom-accounts.html"/>
    /// <see cref="http://blog.falafel.com/using-xamarin-forms-dependencyservice-and-azure-mobile-services-to-add-authentication-to-cross-platform-apps/"/>
    /// <see cref="https://github.com/jsauve/OAuthTwoDemo.XForms"/>
    ///
    public partial class Data
    {
        public static Dictionary<string, Helpers.OAuth> TestCases = null;

        static Data()
        {
            TestCases = new Dictionary<string, Helpers.OAuth>();

            GoogleOAuth2NativeUIAndroid google1 = new GoogleOAuth2NativeUIAndroid();
            TestCases.Add(google1.Description, google1);

            GoogleOAuth2NativeUIIOS google2 = new GoogleOAuth2NativeUIIOS();
            TestCases.Add(google2.Description, google2);

            GoogleOAuth2WebApp2015HttpXamarinCom google3 = new GoogleOAuth2WebApp2015HttpXamarinCom();
            TestCases.Add(google3.Description, google3);

            FitbitOAuth2 fitbit = new FitbitOAuth2();
            TestCases.Add(fitbit.Description, fitbit);

            FacebookOAuth2WWWAppXamarin facebook1 = new FacebookOAuth2WWWAppXamarin();
            TestCases.Add(facebook1.Description, facebook1);

            FacebookOAuth2WWWAppLocalhost facebook2 = new FacebookOAuth2WWWAppLocalhost();
            TestCases.Add(facebook2.Description, facebook2);

            FacebookOAuth2WWWApp127001 facebook3 = new FacebookOAuth2WWWApp127001();
            TestCases.Add(facebook3.Description, facebook2);

            GithubOAuth2HttpXamarinCom github1 = new GithubOAuth2HttpXamarinCom();
            TestCases.Add(github1.Description, github1);

            GithubOAuth2HttpsXamarinCom github2 = new GithubOAuth2HttpsXamarinCom();
            TestCases.Add(github2.Description, github2);

            GithubOAuth2HttpsLocalHost github3 = new GithubOAuth2HttpsLocalHost();
            TestCases.Add(github3.Description, github1);

            GithubOAuth2Https127001 github4 = new GithubOAuth2Https127001();
            TestCases.Add(github4.Description, github1);

            GithubOAuth2XamarinAuthSchemeLocalhost github5 = new GithubOAuth2XamarinAuthSchemeLocalhost();
            TestCases.Add(github5.Description, github1);

            InstagramOAuth2 instagram = new InstagramOAuth2();
            TestCases.Add(instagram.Description, instagram);

            LinkedInOAuth1 linkedin1 = new LinkedInOAuth1();
            TestCases.Add(linkedin1.Description, linkedin1);

            LinkedInOAuth2 linkedin2 = new LinkedInOAuth2();
            TestCases.Add(linkedin2.Description, linkedin2);

            MicrosoftLiveOAuth2 microsoft_live = new MicrosoftLiveOAuth2();
            TestCases.Add(microsoft_live.Description, microsoft_live);

            TwitterOAuth1 twitter = new TwitterOAuth1();
            TestCases.Add(twitter.Description, twitter);

            AmazonOAuth2 amazon = new AmazonOAuth2();
            TestCases.Add(amazon.Description, amazon);

            MeetupOAuth1 meetup_oauth1 = new MeetupOAuth1();
            TestCases.Add(meetup_oauth1.Description, meetup_oauth1);

            MeetupOAuth2 meetup_oauth2 = new MeetupOAuth2();
            TestCases.Add(meetup_oauth2.Description, meetup_oauth2);

            DropboxOAuth2 dropbox = new DropboxOAuth2();
            TestCases.Add(dropbox.Description, dropbox);

            PaypalOAuth2 paypal = new PaypalOAuth2();
            TestCases.Add(paypal.Description, paypal);

            StackoverflowOAuth2Explicit stackoverflowoauth2explicit = new StackoverflowOAuth2Explicit();
            TestCases.Add(stackoverflowoauth2explicit.Description, stackoverflowoauth2explicit);

            StackoverflowOAuth2Implicit stackoverflowoauth2implicit = new StackoverflowOAuth2Implicit();
            TestCases.Add(stackoverflowoauth2implicit.Description, stackoverflowoauth2implicit);

            SlackOAuth2 slack = new SlackOAuth2();
            TestCases.Add(slack.Description, slack);

            return;
        }
    }
}
