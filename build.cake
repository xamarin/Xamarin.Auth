/*
#########################################################################################
Installing

	Windows - powershell
		
        Invoke-WebRequest http://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1
        .\build.ps1

	Windows - cmd.exe prompt	
	
        powershell ^
			Invoke-WebRequest http://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1
        powershell ^
			.\build.ps1
	
	Mac OSX 

        curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/osx
        chmod +x ./build.sh && ./build.sh

	Linux

        curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/linux
        chmod +x ./build.sh && ./build.sh

Running Cake to Build Xamarin.Auth targets

	Windows

		tools\Cake\Cake.exe --verbosity=diagnostic --target=libs
		tools\Cake\Cake.exe --verbosity=diagnostic --target=nuget
		tools\Cake\Cake.exe --verbosity=diagnostic --target=samples

	Mac OSX 
	
		mono tools/Cake/Cake.exe --verbosity=diagnostic --target=libs
		mono tools/Cake/Cake.exe --verbosity=diagnostic --target=nuget
		
NuGet Publish patterns

		BEFORE PASTING:
		NOTE: ** / 
		** /output/Xamarin.Auth.1.5.0-alpha-12.nupkg,
		** /output/Xamarin.Auth.XamarinForms.1.5.0-alpha-12.nupkg,
		** /output/Xamarin.Auth.Extensions.1.5.0-alpha-12.nupkg
		
#########################################################################################
*/	
#addin "Cake.Xamarin"
#addin nuget:?package=Cake.FileHelpers
/*
-----------------------------------------------------------------------------------------
	choco install -y gitlink
	
-----------------------------------------------------------------------------------------
*/
#tool nuget:?package=gitlink

var TARGET = Argument ("t", Argument ("target", Argument ("Target", "Default")));

FilePath nuget_tool_path = null;
FilePath cake_tool_path = null;

Task ("nuget-fixes")
	.Does 
	(
		() => 
		{
			/*
				2016-12-19
				Fixing nuget 3.4.4 on windows - parsing solution file
				
				2016-09
				Temporary fix for nuget bug MSBuild.exe autodetection on MacOSX and Linux

				This target will be removed in the future! 

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

			if (IsRunningOnWindows())
			{
				DownloadFile
				(					
					@"https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe",
					"./tools/nuget.exe"
				);
				
			}
			nuget_tool_path = GetToolPath ("../nuget.exe");
			cake_tool_path = GetToolPath ("./Cake.exe");

			bool runs_on_xamarin_ci_macosx_bot = false;
			string path_xamarin_ci_macosx_bot = "Components-Generic-Build-Mac/CI/tools/Cake"; 

			string nuget_location = null;
			string nuget_location_relative_from_cake_exe = null;
			
			if (cake_tool_path.ToString().Contains(path_xamarin_ci_macosx_bot))
			{
				runs_on_xamarin_ci_macosx_bot = true;
			}
			else
			{
				Information("NOT Running on Xamarin CI MacOSX bot");
			}

			if (runs_on_xamarin_ci_macosx_bot)
			{
				Information("Running on Xamarin CI MacOSX bot");
				
				nuget_location = "../../tools/nuget.2.8.6.exe";
				nuget_location_relative_from_cake_exe = "../nuget.2.8.6.exe";
				
				Information("nuget_location = {0} ", nuget_location);
			}
			else
			{
				if (IsRunningOnWindows())
				{
					// new nuget is needed for UWP!
					Information("Running on Windows");
					nuget_location = "./tools/nuget.4.1.0.exe";
					nuget_location_relative_from_cake_exe = "../nuget.4.1.0.exe";
					Information("On Mac downloading 4.1.0 to " + nuget_location);				
					if ( ! FileExists(nuget_location))
					{
						DownloadFile
						(					
							@"https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe",
							nuget_location
						);
					}
				}
				else
				{
					Information("Running on MacOSX (non-Windows)");
					nuget_location = "./tools/nuget.2.8.6.exe";
					nuget_location_relative_from_cake_exe = "../nuget.2.8.6.exe";
					Information("On Mac downloading .2.8.6 to " + nuget_location);				
					//nuget_location = "./tools/nuget.4.1.0.exe";
					//nuget_location_relative_from_cake_exe = "../nuget.4.1.0.exe";
					//Information("On Mac downloading 4.1.0 to " + nuget_location);				
					if ( ! FileExists(nuget_location))
					{
						DownloadFile
						(
							@"https://dist.nuget.org/win-x86-commandline/v2.8.6/nuget.exe",
							nuget_location
						);
					}
				}
			}
		
			DirectoryPath path01 = MakeAbsolute(Directory("./"));
			string path02 = System.IO.Directory.GetCurrentDirectory();
			string path03 = Environment.CurrentDirectory;
			// Cake - WorkingDirectory??
			Information("path01         = {0} ", path01);
			Information("path02         = {0} ", path02);
			Information("path03         = {0} ", path03);
			Information("cake_tool_path = {0} ", cake_tool_path);
			
			nuget_tool_path = GetToolPath (nuget_location_relative_from_cake_exe);

			Information("nuget_tool_path = {0}", nuget_tool_path);

			return;
		}
	);

RunTarget("nuget-fixes");	// fix nuget problems on MacOSX

string github_repo_url="https://github.com/xamarin/Xamarin.Auth";

Action<string> GitLinkAction = 
	(solution_file_name) 
		=>
		{ 
			if (! IsRunningOnWindows())
			{
				// GitLink still has issues on macosx
				return;
			}
			GitLink
			(
				"./", 
				new GitLinkSettings() 
				{
					RepositoryUrl = github_repo_url,
					SolutionFileName = solution_file_name,
				
					// nb: I would love to set this to `treatErrorsAsWarnings` which defaults to `false` 
					// but GitLink trips over Akavache.Tests :/
					// Handling project 'Akavache.Tests'
					// No pdb file found for 'Akavache.Tests', is project built in 'Release' mode with 
					// pdb files enabled? 
					// Expected file is 'C:\Dropbox\OSS\akavache\Akavache\src\Akavache.Tests\Akavache.Tests.pdb'
					ErrorsAsWarnings = true, 
				}
			);
		};




// https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference#restore
// http://cakebuild.net/api/Cake.Common.Tools.NuGet.Restore/NuGetRestoreSettings/
NuGetRestoreSettings nuget_restore_settings = new NuGetRestoreSettings 
	{ 
		ToolPath = nuget_tool_path,
		Verbosity = NuGetVerbosity.Detailed,
	};

NuGetUpdateSettings nuget_update_settings = new NuGetUpdateSettings 
	{ 
		ToolPath = nuget_tool_path,
		Verbosity = NuGetVerbosity.Detailed,
		Prerelease = false,
	};

Task ("clean")
	.Does 
	(
		() => 
		{	
			// note no trailing backslash
			//DeleteDirectories (GetDirectories("./output"), recursive:true);
			// OK
			//CleanDirectories("**/obj");
			//CleanDirectories("**/Obj");
			//CleanDirectories("**/bin");
			//CleanDirectories("**/Bin");
			
			//CleanDirectories(GetDirectories("**/obj"));
			//CleanDirectories(GetDirectories("**/Obj"));
			//CleanDirectories(GetDirectories("**/bin"));
			//CleanDirectories(GetDirectories("**/Bin"));
			
			
			// OK
			DeleteDirectories(GetDirectories("**/obj"), recursive:true);
			DeleteDirectories(GetDirectories("**/Obj"), recursive:true);
			DeleteDirectories(GetDirectories("**/bin"), recursive:true);
			DeleteDirectories(GetDirectories("**/Bin"), recursive:true);
			
			// ! OK
			//DeleteDirectories("**/obj", true);
			// The best overloaded method match for 
			//		`CakeBuildScriptImpl.DeleteDirectories(System.Collections.Generic.IEnumerable<Cake.Core.IO.DirectoryPath>, bool)' 
			// has some invalid arguments
			//Information("NOGO: DeleteDirectories(\"**/obj\", true);");

		}
	);

Task ("distclean")
	.IsDependentOn ("clean")
	.Does 
	(
		() => 
		{				
			DeleteDirectories(GetDirectories("**/bin"), recursive:true);
			DeleteDirectories(GetDirectories("**/Bin"), recursive:true);
			DeleteDirectories(GetDirectories("**/obj"), recursive:true);
			DeleteDirectories(GetDirectories("**/Obj"), recursive:true);
			DeleteDirectories(GetDirectories("**/packages"), recursive:true);
			DeleteDirectories(GetDirectories("**/Components"), recursive:true);
		}
	);

Task ("rebuild")
	.IsDependentOn ("distclean")
	.IsDependentOn ("build")
	;	

Task ("build")
	.IsDependentOn ("libs")
	.IsDependentOn ("samples")
	;	

Task ("package")
	.IsDependentOn ("libs")
	.IsDependentOn ("nuget")
	;	

Task ("libs")
	.IsDependentOn ("nuget-fixes")
	.IsDependentOn ("libs-macosx")	
	.IsDependentOn ("libs-windows")	
	.Does 
	(
		() => 
		{	
		}
	);

string[] source_solutions = new string[]
{
	"./source/Xamarin.Auth-Library.sln",
};

string[] solutions_for_nuget_tests = new string[]
{
	"./samples/Traditional.Standard/references02nuget/Providers/old-for-backward-compatiblity/Xamarin.Auth.Sample.Android/Xamarin.Auth.Sample.Android.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/old-for-backward-compatiblity/Xamarin.Auth.Sample.iOS/Xamarin.Auth.Sample.iOS.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Sample.WindowsPhone8/Component.Sample.WinPhone8.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Sample.WindowsPhone81/Component.Sample.WinPhone81.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Sample.XamarinAndroid/Component.Sample.Android.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Sample.XamarinIOS/Component.Sample.IOS.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard.sln",	
};

string[] sample_solutions_macosx = new []
{
	"./samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",
	//"./samples/bugs-triaging/component-2-nuget-migration-ANE/ANE-MacOSX-Xamarin.Studio.sln", // could not build shared project on CI
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard.sln",
	"./samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",	
};

string[] sample_solutions_windows = new []
{
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",
	/*
	"samples/bugs-triaging/component-2-nuget-migration-ANE/ANE.sln",
	"samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard.sln", 
	"samples/Traditional.Standard/references02nuget/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",	
	"samples/Traditional.Standard/references01projects/Providers/old-for-backward-compatiblity/Xamarin.Auth.Sample.Android/Xamarin.Auth.Sample.Android.sln",
	"samples/Traditional.Standard/references01projects/Providers/old-for-backward-compatiblity/Xamarin.Auth.Sample.iOS/Xamarin.Auth.Sample.iOS.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Sample.WindowsPhone8/Component.Sample.WinPhone8.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Sample.WindowsPhone81/Component.Sample.WinPhone81.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Sample.XamarinAndroid/Component.Sample.Android.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Sample.XamarinIOS/Component.Sample.IOS.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.xxx.sln",
	"samples/Traditional.Standard/references01projects/Providers/Xamarin.Auth.Samples.TraditionalStandard.sln",
	"samples/Traditional.Standard/references02nuget/old-for-backward-compatiblity/Xamarin.Auth.Sample.Android/Xamarin.Auth.Sample.Android.sln",
	"samples/Traditional.Standard/references02nuget/old-for-backward-compatiblity/Xamarin.Auth.Sample.iOS/Xamarin.Auth.Sample.iOS.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Sample.WindowsPhone8/Component.Sample.WinPhone8.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Sample.WindowsPhone81/Component.Sample.WinPhone81.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Sample.XamarinAndroid/Component.Sample.Android.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Sample.XamarinIOS/Component.Sample.IOS.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Samples.TraditionalStandard-MacOSX-Xamarin.Studio.sln",
	"samples/Traditional.Standard/references02nuget/Xamarin.Auth.Samples.TraditionalStandard.sln",
	"samples/Traditional.Standard/WindowsPhoneCrashMissingMethod-GetUI/WP8/Demo.sln",
	"samples/Traditional.Standard/WindowsPhoneCrashMissingMethod-GetUI/WP8-XA/Demo.sln",
	"samples/Xamarin.Forms/references01project/Evolve16Labs/04-Securing Local Data/Diary.sln",
	"samples/Xamarin.Forms/references01project/Evolve16Labs/05-OAuth/ComicBook.sln",
	"samples/Xamarin.Forms/references01project/Providers/XamarinAuth.XamarinForms.sln",
	"samples/Xamarin.Forms/references02nuget/04-Securing Local Data/Diary.sln",
	*/
};

string[] sample_solutions = 
			sample_solutions_macosx
			.Concat(sample_solutions_windows)  // comment out this line if in need
			.ToArray()
			;

string[] solutions = 
			source_solutions
			.Concat(sample_solutions)  // comment out this line if in need
			.ToArray()
			;



string[] build_configurations =  new []
{
	"Debug",
	"Release",
};


Task ("nuget-restore")
	.IsDependentOn ("nuget-fixes")
	.Does 
	(
		() => 
		{	
			//RunTarget("nuget-fixes");

			Information("libs nuget_restore_settings.ToolPath = {0}", nuget_restore_settings.ToolPath);

			NuGetRestore 
				(
					"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
					nuget_restore_settings
				);

			return;

			if (IsRunningOnWindows())
			{
				foreach (string sln in solutions)
				{
					if (sln.Length < 200)
					{
						Information(" NuGetRestore = {0}", sln);
						NuGetRestore(sln, nuget_restore_settings); 
					}
					else
					{
						Information(" NuGetRestore SKIP path too long = {0}", sln);						
					}
				};
			}
			else
			{
				foreach (string sln in solutions)
				{
					Information(" NuGetRestore = {0}", sln);					
					NuGetRestore(sln, nuget_restore_settings); 
				};
			}
			
			return;
		}
	);

Task ("nuget-update")
	.IsDependentOn ("nuget-restore")
	.Does 
	(
		() => 
		{	
			FilePathCollection files_package_config = GetFiles("./**/packages.config");

			if (IsRunningOnWindows())
			{
				foreach(FilePath package_config_file in files_package_config)
				{
					if (package_config_file.ToString().Length < 200)
					{
						Information("Nuget Update W = " + package_config_file);
						NuGetUpdate(package_config_file, nuget_update_settings);
					}
				}				
			}
			else
			{
				foreach(FilePath package_config_file in files_package_config)
				{
					Information("Nuget Update W = " + package_config_file);
					NuGetUpdate(package_config_file, nuget_update_settings);
				}				
				
			}
			return;	
		}
	);

Task ("samples-nuget-restore")
	.IsDependentOn("nuget-fixes")
	.Does 
	(
		() => 
		{
			if (IsRunningOnWindows())
			{
				foreach (string sample_solution in sample_solutions)
				{
					if (sample_solution.Length < 200)
					{
						NuGetRestore(sample_solution, nuget_restore_settings); 
					}
				}
			}
			else
			{
				foreach (string sample_solution in sample_solutions)
				{
					NuGetRestore(sample_solution, nuget_restore_settings); 
				}
			}
			
			return;
		}
	);
	

Task ("libs-macosx")
	.IsDependentOn ("nuget-restore")	
	.Does 
	(
		() => 
		{	
			CreateDirectory ("./output/");
			CreateDirectory ("./output/pcl/");
			CreateDirectory ("./output/android/");
			CreateDirectory ("./output/ios/");

			if ( ! IsRunningOnWindows() )
			{
				XBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Debug");
						}
					);
				XBuild 
					(
						"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
					
				GitLinkAction("./source/Xamarin.Auth-Library.sln");

				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/Xamarin.Auth.LinkSource/Xamarin.Auth.LinkSource.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
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
				CopyFiles
					(
						"./source/Xamarin.Auth.Portable/**/Release/Xamarin.Auth.pdb", 
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
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinAndroid/**/Release/Xamarin.Auth.pdb", 
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
						"./output/iOS/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS/**/Release/Xamarin.Auth.pdb", 
						"./output/iOS/"
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
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/**/Release/Xamarin.Auth.Extensions.pdb", 
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
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/**/Release/Xamarin.Auth.Extensions.pdb", 
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
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/**/Release/Xamarin.Auth.Extensions.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				
				
				
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/Xamarin.Auth.Forms.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.Forms.dll", 
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.Forms.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/Xamarin.Auth.Forms.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/pcl/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/Xamarin.Auth.Forms.Droid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/android/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				XBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/Xamarin.Auth.Forms.iOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				
			}

			return;
		}
	);


Task ("libs-windows")
	.IsDependentOn ("nuget-restore")
	.Does 
	(
		() => 
		{	
			CreateDirectory ("./output/");
			CreateDirectory ("./output/pcl/");
			CreateDirectory ("./output/android/");
			CreateDirectory ("./output/ios/");
			CreateDirectory ("./output/wp80/");
			CreateDirectory ("./output/wp81/");
			CreateDirectory ("./output/wpa81/");
			CreateDirectory ("./output/wpa81/Xamarin.Auth/");
			CreateDirectory ("./output/win81/");
			CreateDirectory ("./output/win81/Xamarin.Auth/");
			CreateDirectory ("./output/uap10.0/");
			CreateDirectory ("./output/uap10.0/Xamarin.Auth/");

			
			Information("libs nuget_restore_settings.ToolPath = {0}", nuget_restore_settings.ToolPath);

			Information("nuget restore {0}", "./source/Xamarin.Auth-Library.sln");
			NuGetRestore 
				(
					"./source/Xamarin.Auth-Library.sln",
					nuget_restore_settings
				);
			Information("nuget restore {0}", "./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln");
			NuGetRestore 
				(
					"./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln",
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
					
				GitLinkAction("./source/Xamarin.Auth-Library.sln");
				GitLinkAction("./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln");

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
				CopyFiles
					(
						"./source/Xamarin.Auth.Portable/**/Release/Xamarin.Auth.pdb", 
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
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinAndroid/**/Release/Xamarin.Auth.pdb", 
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
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.XamarinIOS/**/Release/Xamarin.Auth.pdb", 
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
				CopyFiles
					(
						"./source/Xamarin.Auth.WindowsPhone8/**/Release/Xamarin.Auth.pdb", 
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
				CopyFiles
					(
						"./source/Xamarin.Auth.WindowsPhone81/**/Release/Xamarin.Auth.pdb", 
						"./output/wp81/"
					);
				//-------------------------------------------------------------------------------------
				/*
					Dependencies omitted!! 
					.
					├── Release
					│   ├── Xamarin.Auth
					│   │   ├── WebAuthenticatorPage.xaml
					│   │   ├── WebAuthenticatorPage.xbf
					│   │   └── Xamarin.Auth.xr.xml
					│   ├── Xamarin.Auth.dll
					│   ├── Xamarin.Auth.pdb
					│   └── Xamarin.Auth.pri
				*/
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
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth.dll", 
						"./output/win81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth.pdb", 
						"./output/win81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth.pri", 
						"./output/win81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth/Xamarin.Auth.xr.xml", 
						"./output/win81/Xamarin.Auth/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth/WebAuthenticatorPage.xaml", 
						"./output/win81/Xamarin.Auth/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindows81/bin/Release/Xamarin.Auth/WebAuthenticatorPage.xbf", 
						"./output/win81/Xamarin.Auth/"
					);
				//-------------------------------------------------------------------------------------
				/*
					Dependencies omitted!! 
					.
					├── Release
					│   ├── Xamarin.Auth
					│   │   ├── WebAuthenticatorPage.xaml
					│   │   ├── WebAuthenticatorPage.xbf
					│   │   └── Xamarin.Auth.xr.xml
					│   ├── Xamarin.Auth.dll
					│   ├── Xamarin.Auth.pdb
					│   └── Xamarin.Auth.pri
				*/
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
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth.dll", 
						"./output/wpa81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth.pdb", 
						"./output/wpa81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth.pri", 
						"./output/wpa81/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth/Xamarin.Auth.xr.xml", 
						"./output/wpa81/Xamarin.Auth/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth/WebAuthenticatorPage.xaml", 
						"./output/wpa81/Xamarin.Auth/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.WinRTWindowsPhone81/bin/Release/Xamarin.Auth/WebAuthenticatorPage.xbf", 
						"./output/wpa81/Xamarin.Auth/"
					);
				//-------------------------------------------------------------------------------------
				/*
					Dependencies omitted!! 
					.
					├── Release
					│   ├── Xamarin.Auth
					│   │   ├── WebAuthenticatorPage.xaml
					│   │   └── Xamarin.Auth.xr.xml
					│   ├── Xamarin.Auth.dll
					│   ├── Xamarin.Auth.pdb
					│   └── Xamarin.Auth.pri
				*/
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
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Xamarin.Auth.dll", 
						"./output/uap10.0/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Xamarin.Auth.pdb", 
						"./output/uap10.0/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Xamarin.Auth.pri", 
						"./output/uap10.0/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Xamarin.Auth/Xamarin.Auth.xr.xml", 
						"./output/uap10.0/Xamarin.Auth/"
					);
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Xamarin.Auth/WebAuthenticatorPage.xaml", 
						"./output/uap10.0/Xamarin.Auth/"
					);
				/*
					.net Native - Linking stuff - not needed
				CopyFiles
					(
						"./source/Xamarin.Auth.UniversalWindowsPlatform/bin/Release/Properties/Xamarin.Auth.rd.xml", 
						"./output/uap10.0/Properties/"
					);
				*/
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
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.Portable/**/Release/Xamarin.Auth.Extensions.pdb", 
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
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinAndroid/**/Release/Xamarin.Auth.Extensions.pdb", 
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
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/Extensions/Xamarin.Auth.Extensions.XamarinIOS/**/Release/Xamarin.Auth.Extensions.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				
				
				
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/Xamarin.Auth.Forms.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.Forms.dll", 
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.Forms.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/Xamarin.Auth.Forms.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/pcl/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/pcl/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/Xamarin.Auth.Forms.Droid.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/android/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.Droid/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/android/"
					);
				//-------------------------------------------------------------------------------------
				MSBuild
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/Xamarin.Auth.Forms.iOS.csproj", 
						c => 
						{
							c.SetConfiguration("Release");
						}
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/**/Release/Xamarin.Auth.XamarinForms.dll", 
						"./output/ios/"
					);
				CopyFiles
					(
						"./source/XamarinForms/Xamarin.Auth.Forms.iOS/**/Release/Xamarin.Auth.XamarinForms.pdb", 
						"./output/ios/"
					);
				//-------------------------------------------------------------------------------------
				
			} 

			return;
		}
	);


Task ("samples")
	.Does 
	(
		() => 
		{
			if ( IsRunningOnWindows() )
			{
				RunTarget ("samples-windows");
			}
			RunTarget ("samples-macosx");
		}
	);

Task ("samples-macosx")
	.IsDependentOn ("samples-nuget-restore")
	.IsDependentOn ("libs")
	.Does 
	(
		() => 
		{
			foreach (string sample_solution in sample_solutions_macosx)
			{
				foreach (string configuration in build_configurations)
				{
					if ( IsRunningOnWindows() )
					{
						MSBuild
							(
								sample_solution, 
								c => 
								{
									c.SetConfiguration(configuration);
								}
							);						
					}
					else
					{
						XBuild
							(
								sample_solution, 
								c => 
								{
									c.SetConfiguration(configuration);
								}
							);						
					}
				}
			}

			return;
		}
	);

Task ("samples-windows")
	.IsDependentOn ("samples-nuget-restore")
	.IsDependentOn ("libs")
	.Does 
	(
		() => 
		{
			foreach (string sample_solution in sample_solutions_windows)
			{
				foreach (string configuration in build_configurations)
				{
					if ( IsRunningOnWindows() )
					{
					}
					else
					{
						XBuild
							(
								sample_solution, 
								c => 
								{
									c.SetConfiguration(configuration);
								}
							);						
					}
				}
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
			if 
			(
				! FileExists("./output/wp80/Xamarin.Auth.dll")
				||
				! FileExists("./output/wp81/Xamarin.Auth.dll")
				||
				! FileExists("./output/win81/Xamarin.Auth.dll")
				||
				! FileExists("./output/wpa81/Xamarin.Auth.dll")
				||
				! FileExists("./output/uap10.0/Xamarin.Auth.dll")
			)
			{
				string msg =
				"Missing Windows dll artifacts"
				+ System.Environment.NewLine +
				"Please, build on Windows first!";

				throw new System.ArgumentNullException(msg);
			}
			
			NuGetPack 
				(
					"./nuget/Xamarin.Auth.nuspec", 
					new NuGetPackSettings 
					{ 
						Verbosity = NuGetVerbosity.Detailed,
						OutputDirectory = "./output/",        
						BasePath = "./",
						ToolPath = nuget_tool_path,
						Symbols = true
					}
				);                
			NuGetPack 
				(
					"./nuget/Xamarin.Auth.XamarinForms.nuspec", 
					new NuGetPackSettings 
					{ 
						Verbosity = NuGetVerbosity.Detailed,
						OutputDirectory = "./output/",        
						BasePath = "./",
						ToolPath = nuget_tool_path,
						Symbols = true
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
						ToolPath = nuget_tool_path,
						Symbols = true
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

Task ("component")
	.IsDependentOn ("nuget")
	.IsDependentOn ("samples")
	.Does 
	(
		() => 
		{
			var COMPONENT_VERSION = "1.3.1.1";
			var yamls = GetFiles ("./**/component.yaml");

			foreach (var yaml in yamls) 
			{
				Information("yaml = " + yaml);
				var contents = FileReadText (yaml).Replace ("$version$", COMPONENT_VERSION);
				
				var fixedFile = yaml.GetDirectory ().CombineWithFilePath ("component.yaml");
				FileWriteText (fixedFile, contents);
				
				PackageComponent 
					(
						fixedFile.GetDirectory (), 
						new XamarinComponentSettings ()
					);
			}

			if (!DirectoryExists ("./output"))
			{
				CreateDirectory ("./output");
			}

			CopyFiles ("./component/**/*.xam", "./output");		
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


//=================================================================================================
// Put those 2 CI targets at the end of the file after all targets
// If those targets are before 1st RunTarget() call following error occusrs on 
//		*	MacOSX under Mono
//		*	Windows
// 
//	Task 'ci-osx' is dependent on task 'libs' which do not exist.
//
// Xamarin CI - Jenkins job targets
Task ("ci-osx")
    .IsDependentOn ("libs")
    .IsDependentOn ("nuget")
    //.IsDependentOn ("samples")
	;
Task ("ci-windows")
    .IsDependentOn ("libs")
    .IsDependentOn ("nuget")
    //.IsDependentOn ("samples")
	;	
//=================================================================================================

Task ("Default")
    .IsDependentOn ("nuget")
	;

RunTarget (TARGET);