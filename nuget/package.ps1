$rootPath = Split-Path -Path $MyInvocation.MyCommand.Path
$targetNugetExe = "$rootPath\nuget.exe"

if(-not (Test-Path($targetNugetExe))){
	$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}
Set-Alias nuget $targetNugetExe -Scope Global -Verbose

nuget pack Xamarin.Auth.nuspec