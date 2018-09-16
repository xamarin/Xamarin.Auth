var target = Argument("target", "Default");

Task("Default")
    .Does(() =>
{
    MSBuild("source/Xamarin.Auth.sln", new MSBuildSettings {
        Configuration = "Release",
        Restore = true,
        Verbosity = Verbosity.Minimal,
    });
});

RunTarget(target);
