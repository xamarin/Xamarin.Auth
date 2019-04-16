# Deep (App) Linking in UWP


UWP and WinRT App linking (Deep linking) Link Activation


  https://tools.ietf.org/html/draft-ietf-oauth-native-apps-03
  https://csharp.christiannagel.com/2016/11/15/deeplinking/
  https://blogs.windows.com/buildingapps/2015/09/22/using-cross-app-communication-to-make-apps-work-together-10-by-10/#GwSPr4w3618IqcaX.97
  https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/appxmanifestschema/element-1-extension
  https://docs.microsoft.com/en-us/windows/uwp/launch-resume/web-to-app-linking

  https://msdn.microsoft.com/library/windows/apps/hh452686
  https://msdn.microsoft.com/en-us/magazine/mt590971.aspx
  
  https://github.com/googlesamples/oauth-apps-for-windows
  https://stackoverflow.com/questions/45267467/redirect-to-uwp-app-from-oauth-without-ms-app
  https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/AssociationLaunching/cs/Package.appxmanifest
  
  https://stackoverflow.com/questions/14804323/custom-url-scheme-for-windows-8
  https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh779670(v=win.10).aspx  

  https://blogs.windows.com/buildingapps/2015/09/22/using-cross-app-communication-to-make-apps-work-together-10-by-10/#bFVr0icIepOfWOdg.97
  http://bilalamjad.azurewebsites.net/blog/deep-linking-auto-app-launching-in-windows-phone-8-1-rt-application/
  https://canbilgin.wordpress.com/2016/01/09/connected-apps-with-windows-10-mobile-uwp-iii/

supposed to add schema/protocol in package.appxmanifest it says that scheme must be up 
to 38-39 chars.

jeffdalby [20:00] 
using the editor or just typing it in?

moljac [20:01] 
And for Other app type I cannot define my short schema

[20:01] 
Editor in VS

jeffdalby [20:03] 
yeah I'm just editing the package.appxmanifest in raw xml

[20:03] 
can type whatever in that...seemed to build

moljac [20:03] 
cool trick

[20:03] 
I ll try that one.

jeffdalby [20:04] 
no clue is the actiovationkind.protocol will launch with it though...I'll see if I 
can get far enough on my end to get to that point

moljac [20:09] 
this is in my App node

     <Extensions>
       <uap:Extension Category="windows.protocol">
         <uap:Protocol Name="com.xamarin.auth.windows">
           <uap:DisplayName>Google OAuth</uap:DisplayName>
         </uap:Protocol>
       </uap:Extension>
     </Extensions>
	 
	 