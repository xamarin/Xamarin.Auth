/*
	MacOSX 
	
		mono tools/Cake/Cake.exe --verbosity=diagnostic --target=libs
		mono tools/Cake/Cake.exe --verbosity=diagnostic --target=nuget
		
	Windows
		tools\Cake\Cake.exe --verbosity=diagnostic --target=libs
		tools\Cake\Cake.exe --verbosity=diagnostic --target=nuget
		tools\Cake\Cake.exe --verbosity=diagnostic --target=samples
*/	
#addin "Cake.Xamarin"

var TARGET = Argument ("t", Argument ("target", Argument ("Target", "Default")));

FilePath nuget_tool_path = null;
FilePath cake_tool_path = null;

Task ("nuget-fixes")
	.Does 
	(
		() => 
		{
			if( ! IsRunningOnWindows() )
			{
				/*
   					Executing: /Users/builder/Jenkins/workspace/Components-Generic-Build-Mac/CI/tools/Cake/../
					nuget.exe restore "/Users/builder/Jenkins/workspace/Components-Generic-Build-Mac/CI/Xamarin.Auth/source/source/Xamarin.Auth-Library.sln" -Verbosity detailed -NonInteractive
    				MSBuild auto-detection: using msbuild version '4.0' from '/Library/Frameworks/Mono.framework/Versions/4.4.1/lib/mono/4.5'. 
					Use option -MSBuildVersion to force nuget to use a specific version of MSBuild.
    				MSBuild P2P timeout [ms]: 120000
    				System.AggregateException: One or more errors occurred. 
					---> 
					NuGet.CommandLineException: MsBuild.exe does not exist at '/Library/Frameworks/Mono.framework/Versions/4.4.1/lib/mono/4.5/msbuild.exe'.
 				
					NuGet Version: 3.4.4.1321

					https://dist.nuget.org/index.html

					Xamarin CI MacOSX bot uses central cake folder
						.Contains("Components-Generic-Build-Mac/CI/tools/Cake");
				*/
				nuget_tool_path = GetToolPath ("../nuget.exe");
				cake_tool_path = GetToolPath ("./Cake.exe");

				bool runs_on_xamarin_ci_macosx_bot = false;
				string path_xamarin_ci_macosx_bot = "Components-Generic-Build-Mac/CI/tools/Cake"; 
				Information("cake_tool_path = {0} ", cake_tool_path);

				string nuget_location = null;
				if (cake_tool_path.ToString().Contains(path_xamarin_ci_macosx_bot))
				{
					runs_on_xamarin_ci_macosx_bot = true;
					Information("Running on Xamarin CI MacOSX bot");
				}
				{
					Information("NOT Running on Xamarin CI MacOSX bot");				
				}
				
				nuget_location = "../nuget.2.8.6.exe";
				if ( ! FileExists (nuget_location))
				{
					DownloadFile
					(
						@"https://dist.nuget.org/win-x86-commandline/v2.8.6/nuget.exe",
						nuget_location
					);
				}
				nuget_tool_path = GetToolPath (nuget_location);

			}

			Information("nuget_tool_path = {0}", nuget_tool_path);

			return;
		}
	);

RunTarget("nuget-fixes");	// fix nuget problems on MacOSX

NuGetRestoreSettings nuget_restore_settings = new NuGetRestoreSettings 
		{ 
			ToolPath = nuget_tool_path,
			Verbosity = NuGetVerbosity.Detailed
		};

Task ("libs")
	.IsDependentOn ("nuget-fixes")
	.Does 
	(
		() => 
		{	
			CreateDirectory ("./output/");
			CreateDirectory ("./output/pcl/");
			CreateDirectory ("./output/android/");
			CreateDirectory ("./output/ios-unified/");
			CreateDirectory ("./output/ios/");
			CreateDirectory ("./output/wp80/");
			CreateDirectory ("./output/wp81/");
			CreateDirectory ("./output/wpa81/");
			CreateDirectory ("./output/win81/");
			CreateDirectory ("./output/winrt/");
			CreateDirectory ("./output/uwp/");

			Information("libs nuget_restore_settings.ToolPath = {0}", nuget_restore_settings.ToolPath);

			NuGetRestore 
				(
					"./source/Xamarin.Auth-Library.sln",
					nuget_restore_settings
				);
			NuGetRestore 
				(
					"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
					nuget_restore_settings
				);
			NuGetRestore 
				(
					"./source/XamarinForms-Xamarin.Auth-Library.sln",
					nuget_restore_settings
				);
			NuGetRestore 
				(
					"./source/XamarinForms-Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
					nuget_restore_settings
				);
			
			if (IsRunningOnWindows ()) 
			{	
				MSBuild 
					(
						"./source/Xamarin.Auth-Library.sln",
						c => 
						{
							c.SetConfiguration("Release")
							.SetPlatformTarget(PlatformTarget.x86);
						}
					);
				MSBuild 
					(
						"./source/Xamarin.Auth-Library.sln",
						c => 
						{
							c.SetConfiguration("Debug")
							.SetPlatformTarget(PlatformTarget.x86);
						}
					);
				MSBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Debug");
						}
					);
				MSBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.LinkSource/Xamarin.Auth.LinkSource.csproj", 
						c => 
						{
							c.SetConfiguration("Debug");
						}
					);
				MSBuild
					(
						"./source/Xamarin.Auth.LinkSource/Xamarin.Auth.LinkSource.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.Portable/Xamarin.Auth.Portable.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.Portable/**/Release/Xamarin.Auth.dll", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.XamarinAndroid/Xamarin.Auth.XamarinAndroid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinAndroid/**/Release/Xamarin.Auth.dll", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.XamarinIOS/Xamarin.Auth.XamarinIOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS/**/Release/Xamarin.Auth.dll", 
						"./output/ios-unified/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.XamarinIOS-Classic/Xamarin.Auth.XamarinIOS-Classic.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS-Classic/**/Release/Xamarin.Auth.dll", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.WindowsPhone8/Xamarin.Auth.WindowsPhone8.csproj", 
						c => 
						{
							c.SetConfiguration("Release")
							.SetPlatformTarget(PlatformTarget.x86);
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WindowsPhone8/**/Release/Xamarin.Auth.dll", 
						"./output/wp80/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.WindowsPhone81/Xamarin.Auth.WindowsPhone81.csproj", 
						c => 
						{
							c.SetConfiguration("Release")
							.SetPlatformTarget(PlatformTarget.x86);
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WindowsPhone81/**/Release/Xamarin.Auth.dll", 
						"./output/wp81/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.WinRTWindows81/Xamarin.Auth.WinRTWindows81.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/**/Release/Xamarin.Auth.dll", 
						"./output/win81/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/Xamarin.Auth.WinRTWindowsPhone81.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/**/Release/Xamarin.Auth.dll", 
						"./output/wpa81/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/Xamarin.Auth.UniversalWindowsPlatform.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/**/Release/Xamarin.Auth.dll", 
						"./output/uwp/"
					);
				//-------------------------------------------------------------------------------------




				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/Xamarin.Auth.Extensions.Portable.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/Xamarin.Auth.Extensions.XamarinAndroid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/Xamarin.Auth.Extensions.XamarinIOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/ios-unified/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS-Classic/Xamarin.Auth.Extensions.XamarinIOS-Classic.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS-Classic/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
			} 
			else	// ! IsRunningOnWindows() 
			{
				XBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				XBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Debug");
						}
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.LinkSource/Xamarin.Auth.LinkSource.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				XBuild
					(
						"./source/Xamarin.Auth.LinkSource/Xamarin.Auth.LinkSource.csproj", 
						c => 
						{
							c.SetConfiguration("Debug");
						}
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.Portable/Xamarin.Auth.Portable.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.Portable/**/Release/Xamarin.Auth.dll", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.XamarinAndroid/Xamarin.Auth.XamarinAndroid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinAndroid/**/Release/Xamarin.Auth.dll", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.XamarinIOS/Xamarin.Auth.XamarinIOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS/**/Release/Xamarin.Auth.dll", 
						"./output/ios-unified/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.XamarinIOS-Classic/Xamarin.Auth.XamarinIOS-Classic.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS-Classic/**/Release/Xamarin.Auth.dll", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------






				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/Xamarin.Auth.Extensions.Portable.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/Xamarin.Auth.Extensions.XamarinAndroid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/Xamarin.Auth.Extensions.XamarinIOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/ios-unified/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS-Classic/Xamarin.Auth.Extensions.XamarinIOS-Classic.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS-Classic/**/Release/Xamarin.Auth.Extensions.dll", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------

			}

			return;
		}
	);

Task ("nuget")
	.IsDependentOn ("libs")
	.Does 
	(
		() => 
		{
			NuGetPack 
				(
					"./nuget/Xamarin.Auth.nuspec", 
					new NuGetPackSettings 
					{ 
						Verbosity = NuGetVerbosity.Detailed,
						OutputDirectory = "./output/",        
						BasePath = "./",
						ToolPath = nuget_tool_path
					}
				);                
			NuGetPack 
				(
					"./nuget/Xamarin.Auth.Extensions.nuspec", 
					new NuGetPackSettings 
					{ 
						Verbosity = NuGetVerbosity.Detailed,
						OutputDirectory = "./output/",        
						BasePath = "./",
						ToolPath = nuget_tool_path
					}
				);                
		}
	);

Task ("externals")
	.Does 
	(
		() => 
		{
			return;
		}
	);


FilePath GetToolPath (FilePath toolPath)
{
    var appRoot = Context.Environment.GetApplicationRoot ();
     var appRootExe = appRoot.CombineWithFilePath (toolPath);
     if (FileExists (appRootExe))
	 {
         return appRootExe;
	 }
    throw new FileNotFoundException ("Unable to find tool: " + appRootExe); 
}

if 
	(
		! FileExists(@"./source/Xamarin.Auth-Library.sln")
		&&
		! FileExists(@"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln")
	)
{
	RunTarget("externals");
}

RunTarget (TARGET);