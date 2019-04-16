# Xamarin.Auth samples

Xamarin.Auth was launched in 2012 with Traditional/Standard technology support.
Xamarin.Forms support was added in 2017.

Sample projects reference Xamarin.Auth as

*	project references 
*	nuget references

Project references are used for debugging and adding new features, while NuGet
references are used solely for testing correctness of the nuget packaging.

## Folder structure

Folder structure changes because a lot - it is construction site.

There are several samples

*	Providers

	This sample is written in Xamarin Traditional/Standard tehcnology 
	(Xamarin.Android and Xamarin.iOS) and for several Windows platforms.
	
	These samples feature setup for numerous OAuth service providers (Google,
	Facebook, Microsoft, LinkedIn, Twitter and more). Each provider can have
	several samples depending what is possible to define on the server side
	(provider's console - google console or similar). Those samples demonstrate
	how to define Xamarin.Auth Authenticator objects (OAuth2Authenticator, 
	OAuth1Authenticator, etc) for particular Oauth provider.
	
*	Xamarin.Forms samples

	Samples for testing Xamarin.Auth for Xamarin.Forms. Some of the samples are
	modified Evolve16 labs samples. 
	
*	sample OAuth providers 

	Several Oauth providers with configurations can be found in 3 projects		
	Xamarin.Auth.OAuthProviders.* projects:
	
	*	Xamarin.Auth.OAuthProviders.Portable		
	*	Xamarin.Auth.OAuthProviders.Shared		
	*	Xamarin.Auth.OAuthProviders.Shared.SecureSensitive		
	
*	bug triaging samples	
	
### Details - Folder structure

### 
	
## OAuth Providers Sample[s] (Xamarin.Auth.OAuthProviders.*)

OAuth Providers' samples are moved into separeate project[s]. 

	
### Details - OAuth Providers Sample[s] (Xamarin.Auth.OAuthProviders.*)

Portable PCL version causes problems with Xamarin.Component packaging. 

Since 2017-06 PCL project 

	Xamarin.Auth.OAuthProviders.Portable
	
references 2 shared projects:

	Xamarin.Auth.OAuthProviders.Shared
	Xamarin.Auth.OAuthProviders.Shared.SecureSensitive	

`Xamarin.Auth.OAuthProviders.Shared` contains common data (not security sensitve) for 
OAuth Endpoints and similar.

`Xamarin.Auth.OAuthProviders.Shared.SecureSensitive` contains security sensitive (private)
data like (`app_id`/`client_id`, `client_secret`). Private/secure data is not commited into 
the public repo and is guarded with `.gitignore`. Source code is included in the project via
MSBuild globbing patterns:

```
  <ItemGroup>
    <!--
	Secure stuff protected by gitgnore
    <Compile Include="SampleData\Providers.Secure\**\*.secure.cs" />
    <None Include="SampleData\Providers.Secure\.gitignore" />
	-->
  </ItemGroup>
```

Public source code contains partial methods and initialization of the private data is performed
in `*.secure.cs` files with partial method implementation.

## Referencing Projects and nuget packages without duplicating projects

in 2017-09 number of samples was reduced with MsBuild trickery in project files (*.csproj).
Project files reference nuget binaries if source folder is not available check samples in
Xamarin.Auth repo and separate repo with extratcted samples (and docs) only:

*	https://github.com/xamarin/Xamarin.Auth
*	https://github.com/moljac/Xamarin.Auth.Samples.NugetReferences


NOTE: Adding nugets might remove these references from samples, so below are snippets 
to be  added to the project files (*.csproj):

After adding those snippets do following steps:

*	fix paths to project files in project references 
*	fix paths (nuget version) for nugets in nuget references	

	it may be necessary to move nuget reference around (it might be inserted at 
	the bottom)

### Xamarin Traditional/Standard Platform[s]

Xamarin Traditional/Standard platform[s] (Xamarin.Android and Xamarin.iOS) samples
will have only references to platform specific projects or nugets.

#### Xamarin.Android 

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj" 
		Condition="exists('..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj')"
		>
      <Project>{47EC107C-EBB2-4676-82DB-F77B7BFC17AC}</Project>
      <Name>Xamarin.Auth.XamarinAndroid</Name>
    </ProjectReference>
    <Reference 
		Include="Xamarin.Auth" 
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj')"
		>
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\MonoAndroid10\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```

#### Xamarin.IOS 

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj" 
		Condition="exists('..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj')"
		>
      <Project>{15BE2387-8E72-4C0B-8A6A-460EF5FA4539}</Project>
      <Name>Xamarin.Auth.XamarinIOS</Name>
    </ProjectReference>
    <Reference 
		Include="Xamarin.Auth" 
		Condition="!exists('..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj')"
		>
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\Xamarin.iOS10\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```

#### Universal Windows Platform UWP

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj" 
		Condition="exists('..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj')"
		>
      <Project>{2D712AA6-7697-4F4D-B5F1-AA03476F59A7}</Project>
      <Name>Xamarin.Auth.UniversalWindowsPlatform</Name>
    </ProjectReference>
    <Reference 
		Include="Xamarin.Auth" 
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj')"
		>
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\uap10.0\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```

#### Windows Phone WinRT 8.1

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj"
		Condition="exists('..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj')"
		>
	  <Project>{D07C6FC6-6860-495C-9BC8-0F731C74AE2F}</Project>
		<Name>Xamarin.Auth.WinRTWindowsPhone81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wpa81\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```
	
#### Windows WinRT 8.1

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj"
		Condition="exists('..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj')"
		>
	  <Project>{C4202AC1-1027-4737-8215-16182421E342}</Project>
		<Name>Xamarin.Auth.WinRTWindows81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\win81\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```

#### Windows Phone 8.1 Silverlight

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.WindowsPhone81\Xamarin.Auth.WindowsPhone81.csproj"
		Condition="exists('..\..\..\..\source\Xamarin.Auth.WindowsPhone81\Xamarin.Auth.WindowsPhone81.csproj')"
		>
	  <Project>{18B5054D-6035-40DB-B1AE-3E4FDE8DB6E9}</Project>
		<Name>Xamarin.Auth.WindowsPhone81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.WindowsPhone81\Xamarin.Auth.WindowsPhone81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wp8\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```
	
#### Windows Phone 8 Silverlight

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj"
		Condition="exists('..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj')"
		>
	  <Project>{08470E0D-EB43-4E07-92F4-020DF019F628}</Project>
		<Name>Xamarin.Auth.WindowsPhone8</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wp8\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```	
### Xamarin.Forms samples

Xamarin.Forms samples using Presenters implementations will have only references to 
platform specific projects or nugets (like Traditional/Standard), while samples based
on Xamarin.Forms Custom Renderers need additonal Xamarin.Auth.Forms references - again
project or nuget.

#### Portable

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.Portable\Xamarin.Auth.Portable.csproj"
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.Portable\Xamarin.Auth.Portable.csproj')"
		>
      <Project>{87580927-9f8e-42ae-bdfe-35f95abf17d8}</Project>
      <Name>Xamarin.Auth.Portable</Name>
      <Private>False</Private>
	</ProjectReference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.Portable\Xamarin.Auth.Portable.csproj"
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.Portable\Xamarin.Auth.Portable.csproj')"
		>
      <Project>{87580927-9f8e-42ae-bdfe-35f95abf17d8}</Project>
      <Name>Xamarin.Auth.Portable</Name>
      <Private>False</Private>
	</ProjectReference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```
 
#### Xamarin.Android

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj" 
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj')"
		>
      <Project>{47EC107C-EBB2-4676-82DB-F77B7BFC17AC}</Project>
      <Name>Xamarin.Auth.XamarinAndroid</Name>
    </ProjectReference>
    <Reference 
		Include="Xamarin.Auth.XamarinAndroid" 
		Condition="! exists('..\..\..\..\..\source\Xamarin.Auth.XamarinAndroid\Xamarin.Auth.XamarinAndroid.csproj')"
		>
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\MonoAndroid10\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.Droid\Xamarin.Auth.Forms.Droid.csproj"
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.Droid\Xamarin.Auth.Forms.Droid.csproj')"
		>
      <Project>{1B702A60-3D89-4183-B251-4A07388DCCB5}</Project>
      <Name>Xamarin.Auth.Forms.Droid</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.Droid\Xamarin.Auth.Forms.Droid.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\MonoAndroid10\Xamarin.Auth.Forms.dll</HintPath>
	</Reference>
  </ItemGroup>
```

#### Xamarin.IOS

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj" 
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj')"
		>
      <Project>{15BE2387-8E72-4C0B-8A6A-460EF5FA4539}</Project>
      <Name>Xamarin.Auth.XamarinIOS</Name>
    </ProjectReference>
    <Reference 
		Include="Xamarin.Auth" 
		Condition="!exists('..\..\..\..\..\source\Xamarin.Auth.XamarinIOS\Xamarin.Auth.XamarinIOS.csproj')"
		>
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\Xamarin.iOS10\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.iOS\Xamarin.Auth.Forms.iOS.csproj"
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.iOS\Xamarin.Auth.Forms.iOS.csproj')"
		>
      <Project>{7666F0AB-7493-49F2-82B0-7D51A0250FC9}</Project>
      <Name>Xamarin.Auth.Forms.iOS</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.iOS\Xamarin.Auth.Forms.iOS.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\Xamarin.iOS10\Xamarin.Auth.Forms.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
``` 
 
 #### Universal Windows Platform UWP

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj" 
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj')"
		>
      <Project>{2D712AA6-7697-4F4D-B5F1-AA03476F59A7}</Project>
      <Name>Xamarin.Auth.UniversalWindowsPlatform</Name>
    </ProjectReference>
    <Reference Include="Xamarin.Auth" Condition="! exists('..\..\..\..\..\source\Xamarin.Auth.UniversalWindowsPlatform\Xamarin.Auth.UniversalWindowsPlatform.csproj')">
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\uap10.0\Xamarin.Auth.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->  
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
    <ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.UniversalWindowsPlatform\Xamarin.Auth.Forms.UniversalWindowsPlatform.csproj" 
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.UniversalWindowsPlatform\Xamarin.Auth.Forms.UniversalWindowsPlatform.csproj')"
		>
      <Project>{ef012647-f313-4dbf-ba34-f8f6190e4906}</Project>
      <Name>Xamarin.Auth.Forms.UniversalWindowsPlatform</Name>
    </ProjectReference>
    <Reference Include="Xamarin.Auth.Forms" Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.UniversalWindowsPlatform\Xamarin.Auth.Forms.UniversalWindowsPlatform.csproj')">
      <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\uap10.0\Xamarin.Auth.Forms.dll</HintPath>
    </Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->    
```
 
#### Windows Phone WinRT 8.1

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj"
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj')"
		>
	  <Project>{D07C6FC6-6860-495C-9BC8-0F731C74AE2F}</Project>
		<Name>Xamarin.Auth.WinRTWindowsPhone81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\Xamarin.Auth.WinRTWindowsPhone81\Xamarin.Auth.WinRTWindowsPhone81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wpa81\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->  
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindowsPhone81\Xamarin.Auth.Forms.WinRTWindowsPhone81.csproj"
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindowsPhone81\Xamarin.Auth.Forms.WinRTWindowsPhone81.csproj')"
		>
	  <Project>{73188683-2fcc-4aee-8a30-e30e1532d6cf}</Project>
		<Name>Xamarin.Auth.Forms.WinRTWindowsPhone81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindowsPhone81\Xamarin.Auth.Forms.WinRTWindowsPhone81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wpa81\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->  
```


#### Windows WinRT 8.1

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj"
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj')"
		>
	  <Project>{C4202AC1-1027-4737-8215-16182421E342}</Project>
		<Name>Xamarin.Auth.WinRTWindows81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\Xamarin.Auth.WinRTWindows81\Xamarin.Auth.WinRTWindows81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\win81\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindows81\Xamarin.Auth.Forms.WinRTWindows81.csproj"
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindows81\Xamarin.Auth.Forms.WinRTWindows81.csproj')"
		>
	  <Project>{9ee3f977-6715-4509-8fe8-b862158aa293}</Project>
		<Name>Xamarin.Auth.Forms.WinRTWindows81</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth"
		Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WinRTWindows81\Xamarin.Auth.Forms.WinRTWindows81.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\win81\Xamarin.Auth.Forms.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->
```

#### Windows Phone 8.x Silverlight

```xml
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj"
		Condition="exists('..\..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj')"
		>
	  <Project>{08470E0D-EB43-4E07-92F4-020DF019F628}</Project>
		<Name>Xamarin.Auth.WindowsPhone8</Name>
	</ProjectReference>
		Include="Xamarin.Auth"
	<Reference 
		Condition="! exists('..\..\..\..\..\source\Xamarin.Auth.WindowsPhone8\Xamarin.Auth.WindowsPhone8.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wp8\Xamarin.Auth.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->  
  <!--
  ==================================================================================================
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  http://laurentkempe.com/2009/12/03/ProjectReference-with-Condition-in-your-MSBuild-project-files/
  msbuild Choose When ProjectReference Reference Include
  -->
  <ItemGroup>
	<ProjectReference 
		Include="..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WindowsPhone8\Xamarin.Auth.Forms.WindowsPhone8.csproj"
		Condition="exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WindowsPhone8\Xamarin.Auth.Forms.WindowsPhone8.csproj')"
		>
	  <Project>{c5dd0133-9a66-4489-91c3-745d6ed5ffe5}</Project>
		<Name>Xamarin.Auth.Forms.WindowsPhone8</Name>
	</ProjectReference>
	<Reference 
		Include="Xamarin.Auth.Forms"
		Condition="! exists('..\..\..\..\..\source\XamarinForms\Xamarin.Auth.Forms.WindowsPhone8\Xamarin.Auth.Forms.WindowsPhone8.csproj')"
		>
	  <HintPath>..\packages\Xamarin.Auth.1.5.0.3\lib\wp8\Xamarin.Auth.Forms.dll</HintPath>
	</Reference>
  </ItemGroup>
  <!--
  If projects for references can be found use ProjectReferences otherwise use NuGet references
  ==================================================================================================
  -->  
```

 
 
 
 
 
 
 
 
## Future work

There is effort to reduce number of samples with MSBuild trickery, i.e to have
project and nuget references in single project but switchable.

Something like:

```xml
<ItemGroup Condition="'$(ReferenceType)'=='Project'">
	<Reference Include="..."/>
</ItemGroup>
```

  And another ItemGroup for Sql server 2008:

```xml
<ItemGroup Condition="'$(ReferenceType)'=='NuGet'">
	<Reference Include="..."/>
</ItemGroup>
```

User should provide a default value for the property ReferenceType before 
those items are declared. 

Then at the command line you users can override default value for `ReferenceType` 
using the /p switch when invoking msbuild.exe.

```
msbuild.exe /p:ReferenceType=NuGet
msbuild.exe /p:ReferenceType=Project
```

https://msdn.microsoft.com/en-us/library/ms164311.aspx

Another possibility:

```xml
<Choose>
  <When Condition=" '$(Configuration)' == 'ProjectReferences' ">
    <ItemGroup>
        <ProjectReferenceInclude="..\client1\app.Controls\app.Controls.csproj">
        <Project>{A7714633-66D7-4099-A255-5A911DB7BED8}</Project>
        <Name>app.Controls %28Sources\client1\app.Controls%29</Name>
      </ProjectReference>
    </ItemGroup>
  </When>
  <Otherwise>
    <ItemGroup>
      <ProjectReference Include="..\app.Controls\app.Controls.csproj">
        <Project>{2E6D4065-E042-44B9-A569-FA1C36F1BDCE}</Project>
        <Name>app.Controls %28Sources\app.Controls%29</Name>
      </ProjectReference>
    </ItemGroup>
  </Otherwise>
</Choose>
```