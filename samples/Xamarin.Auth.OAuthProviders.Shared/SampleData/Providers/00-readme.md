# InSecure / inSensitive / Public Data Folder

This folder consists of:

*	c# files with public/insenstive/open/exposed data		
	in this case for OAtuh providers for this sample	
    *   c# code is 
        *   included into project in project file (csproj)        
        *   code can be pushed to the repo
        *   this code calls to partial method implementation[s]
*   c# files in secure/ folder with private/sensitive/secret/hidden data        
    *   c# code is 
        *   included into project in project file (csproj) by the means of wildcards        
        *   excluded/ignored for source versioning by the means of .gitignore
        *   code contains sensitive data in partial methods



## Goal and Implementation

*	goal		
	*	make samples compile clean
	*	hide private/sensitive/secret data like API keys (Client Key, Customer key)



## Project File (csproj)

    <Compile Include="Data\SecurePrivateSecretHidden\*.cs" />



		//---------------------------------------------------------------------------------
		// methods implemented in other file which is included in project through wildcard
		// in this case files are in folder
		//			SecureSensitivePrivateData/
		//	patterns
		//			*.secure.cs
		//			*.sensitive.cs
		//			*.sensitive.cs
		//
		//		<Compile Include="SecureSensitivePrivateData/*.secure.cs" />
		//		<Compile Include="SecureSensitivePrivateData/*.secret.cs" />
		//		<Compile Include="SecureSensitivePrivateData/*.sensitive.cs" />
		//
		// csproj include


		// real data (presonal, hidden, secret, sensitive)
		// in csproj included as wildcard:
		//			    <Compile Include="Data\SecurePrivateSecretHidden\*.cs" />
		// data set in PublicDemo is overriden in those methods
		//---------------------------------------------------------------------------------




## .gitignore

igonre patterns (personal):


	**/*.hidden.cs
	**/*.hidden.md
	**/*.hidden.sh
	**/*.hidden.bat
	**/*.hidden.cmd
	**/*.secure*.cs
	**/*.secure*.md
	**/*.secure*.sh
	**/*.secure*.bat
	**/*.secure*.cmd
	**/*.secret*.cs
	**/*.secret*.md
	**/*.secret*.sh
	**/*.secret*.bat
	**/*.secret*.cmd
	**/*.sensitive.cs
	**/*.sensitive.md
	**/*.sensitive.sh
	**/*.sensitive.bat
	**/*.sensitive.cmd
	**/*.private.cs
	**/*.private.md
	**/*.private.sh
	**/*.private.bat
	**/*.private.cmd

## OAuth Providers implemented




		/*
			WORKING!!
				Facebook
					OAuth2
						scope = "";				// email, basic, ...
						uri_authorize =  new Uri("https://m.facebook.com/dialog/oauth/");
						uri_callback_redirect = new Uri("http://xamarin.com");

				Twitter
					OAuth1
						  "DynVhdIjJDZSdQMb8KSLTA",
						  "REvU5dCUQI4MvjV6aWwUWVUqwObu3tvePIdQVBhNo",
						  new Uri("https://api.twitter.com/oauth/request_token"),
						  new Uri("https://api.twitter.com/oauth/authorize"),
						  new Uri("https://api.twitter.com/oauth/access_token"),
						  new Uri("http://twitter.com"

			LINKS / REFERENCES:

				https://forums.xamarin.com/discussion/3420/xamarin-auth-with-twitter
				https://forums.xamarin.com/discussion/16100/oauth-twitter-and-xamarin-article-authenticate-with-xamarin-auth
				http://visualstudiomagazine.com/articles/2014/04/01/using-oauth-twitter-and-async-to-display-data.aspx?m=2
				https://forums.xamarin.com/discussion/15869/xamarin-auth-twitter-authentication-process-failing
				https://forums.xamarin.com/discussion/4178/does-twitter-oauth-work-with-xamarin-auth
				http://www.codeproject.com/Tips/852742/Simple-Twitter-client-using-Xamarin-Forms-Xamarin

	*/
