# Reference Assemblies with Bait-n-Switch

Generate API with GenAPI.exe

```csharp

    export timestamp=`date +"%Y%m%d-%H%M"` ; echo $timestamp

    mono \
        ./source/packages/Microsoft.DotNet.BuildTools.GenAPI.1.0.0-beta-00081/tools/GenAPI.exe \
            ./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll \
            > \
            Xamarin.Auth.ReferenceSourceByGenAPI.$timestamp.cs

    mono \
        ./source/packages/Microsoft.DotNet.BuildTools.GenAPI.1.0.0-beta-00081/tools/GenAPI.exe \
            ./source/Extensions/Xamarin.Auth.Extensions.Portable/bin/Debug/Xamarin.Auth.Extensions.dll \
            > \
            Xamarin.Auth.Extensions.ReferenceSourceByGenAPI.$timestamp.cs

    mono \
        ./source/packages/Microsoft.DotNet.BuildTools.GenAPI.1.0.0-beta-00081/tools/GenAPI.exe \
            ./source/XamarinForms/Xamarin.Auth.Forms/bin/Debug/Xamarin.Auth.XamarinForms.dll \
            > \
            Xamarin.Auth.XamarinForms.ReferenceSourceByGenAPI.$timestamp.cs

```