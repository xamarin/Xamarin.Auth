# Desktop Console

Needed for basic [unit] testing.

Classes needed:

*	TcpListener
	
	.NET Standard 1.3
	

*	HttpListener
	
	.NET Standard 1.3
	
	using System.net.httplistener = Microsoft.AspNetCore.Server.Kestrel.Internal.Http.Listener;
	
	https://github.com/dotnet/corefx/tree/master/src/System.Net.HttpListener
	
	Microsoft.Net.Http.Server.WebListener
	
	Alternatives:
	
	*	https://github.com/StefH/NETStandard.HttpListener
	
*	for PKCE

	*	RNGCryptoServiceProvider
	
		NOT supported in .NET Standard
	
		http://packagesearch.azurewebsites.net/?q=RNGCryptoServiceProvider
		
	*	SHA256Managed
	
		NOT supported in .NET Standard
		
		http://packagesearch.azurewebsites.net/?q=SHA256Managed