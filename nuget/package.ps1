$rootPath = Split-Path -Path $MyInvocation.MyCommand.Path

if(-not (Test-Path("$rootPath\nuget.exe"))){
	$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
	$targetNugetExe = "$rootPath\nuget.exe"
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
	Set-Alias nuget $targetNugetExe -Scope Global -Verbose
}

nuget pack Xamarin.Auth.nuspec