# iOS 10.x Simulator Error on KeyChain Access -34018

## Problem

Xamarin.Auth error with messge: 

	Could not save account to KeyChain: -34018

References (duplicates):

*	139 Could not save account to KeyChain: -34018 		
	https://github.com/xamarin/Xamarin.Auth/issues/139
*	133 System.Exception: Could not save account to KeyChain: -34018 			
	on iOS Simulator 7 OS 10.1			
	https://github.com/xamarin/Xamarin.Auth/issues/133
*	128 Error: Could not save account to KeyChain -- iOS 10			
	https://github.com/xamarin/Xamarin.Auth/issues/128

## Analysis

This problem is not Xamarin related: seems like this is bug in iOS 10 simulator and XCode (8.x).
 
https://forums.developer.apple.com/thread/4743?tstart=0
https://forums.developer.apple.com/thread/51071
https://stackoverflow.com/questions/27752444/ios-keychain-writing-value-results-in-error-code-34018
https://stackoverflow.com/questions/22082996/testing-the-keychain-osstatus-error-34018
https://stackoverflow.com/questions/38456471/secitemadd-always-returns-error-34018-in-xcode-8-in-ios-10-simulator
https://stackoverflow.com/questions/29740952/osstatus-error-code-34018


## [Re]Solution / Workaround

If not present create empty Entitlements.plist 

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
</dict>
</plist>
```

Entitlements.plist must be added to iOS Bundle Signing options for the `iPhoneSimulator|Debug` 
platform.

```
iOS App Options/Properties +/ Build / iOS Bundle Signing +/ Custom Entitlements : Entitlements.plist
```

This will result in following code in *.csproj file:

```xml
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|iPhone' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\iPhone\Debug</OutputPath>
    <DefineConstants>DEBUG;ENABLE_TEST_CLOUD;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <ConsolePause>false</ConsolePause>
    <MtouchArch>ARMv7, ARM64</MtouchArch>
    <CodesignEntitlements>Entitlements.plist</CodesignEntitlements>
    <MtouchFloat32>true</MtouchFloat32>
    <CodesignKey>iPhone Developer</CodesignKey>
    <DeviceSpecificBuild>true</DeviceSpecificBuild>
    <MtouchDebug>true</MtouchDebug>
    <MtouchProfiling>true</MtouchProfiling>
    <IpaPackageName>
    </IpaPackageName>
  </PropertyGroup>
```

while default *.csproj file looks like this


```xml
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|iPhoneSimulator' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\iPhoneSimulator\Debug</OutputPath>
    <DefineConstants>DEBUG;ENABLE_TEST_CLOUD;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <CodesignKey>iPhone Developer</CodesignKey>
    <DeviceSpecificBuild>true</DeviceSpecificBuild>
    <MtouchDebug>true</MtouchDebug>
    <MtouchNoSymbolStrip>true</MtouchNoSymbolStrip>
    <MtouchFastDev>true</MtouchFastDev>
    <IOSDebuggerPort>50317</IOSDebuggerPort>
    <MtouchLink>None</MtouchLink>
    <MtouchArch>i386, x86_64</MtouchArch>
    <MtouchHttpClientHandler>HttpClientHandler</MtouchHttpClientHandler>
  </PropertyGroup>
```

NOTE: difference is in this file:

```xml
    <CodesignEntitlements>Entitlements.plist</CodesignEntitlements>
```
