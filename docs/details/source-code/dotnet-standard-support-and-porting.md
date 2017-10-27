# Building Xamarin.Auth 

## Preprocessor defines

In order to simplify task for some projects (Microsoft and Open Source) Xamarin.Auth
has introduced several preprocessor conditional `#defines` to hide Xamarin.Auth
API by making it `internal`.

One of those projects is Azure Mobile Services Client which uses Xamarin.Auth as a
code base. 

Those defines are:

*   `XAMARIN_AUTH_INTERNAL`

    This preprocessor define is used to hide Xamarin.Auth public classes by making them
    internal.

*   `XAMARIN_CUSTOM_TABS_INTERNAL`

    This preprocessor define is used to hide Xamarin.Android.Support.CustomTabs additional
    utilities which are used for Xamarin.Android Native UI support


## Troubleshooting

Preprocessor defines brings additional level of complexity and can cause unexpected compile
time errors. Generally this approach is not considered to be a good cross-platform programming
practice, but this was the only way to reduce burdain of changing code for teams/projects
that use Xamarin.Auth.

Sometimes during development those defines might introduce compile time errors, so here are
some code snippets for commandline builds (CI servers) for localizing projects that do not 
compile.

Build with msbuild on Mac with preprocessor parameters:

Delimiter character for preprocessor defines:

    %3B = ';'	

Building with msbuild specific solution:

    /Library/Frameworks/Mono.framework/Commands/msbuild \
        /target:ReBuild \
        "/p:DefineConstants=XAMARIN_AUTH_INTERNAL%3BXAMARIN_CUSTOM_TABS_INTERNAL" \
        /verbosity:minimal \
        /consoleloggerparameters:ShowCommandLine \
        ./source/Xamarin.Auth-Library-MacOSX-Xamarin.Studio.sln 

or for single project:

    /Library/Frameworks/Mono.framework/Commands/msbuild \
        /target:ReBuild \
        "/p:DefineConstants=XAMARIN_AUTH_INTERNAL%3BXAMARIN_CUSTOM_TABS_INTERNAL" \
        /verbosity:minimal \
        /consoleloggerparameters:ShowCommandLine \
        ./source/Xamarin.Auth.Portable/Xamarin.Auth.Portable.csproj 

NOTE: seems like msbuild needs additional `__UNIFIED__` define for Xamarin.iOS projects

    /Library/Frameworks/Mono.framework/Commands/msbuild \
        /target:ReBuild \
        "/p:DefineConstants=XAMARIN_AUTH_INTERNAL%3BXAMARIN_CUSTOM_TABS_INTERNAL%3B__UNIFIED__" \
        /verbosity:minimal \
        /consoleloggerparameters:ShowCommandLine \
