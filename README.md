### NOTE: This project was built for .NET Core 3.0, if you want to target .NET framework [click here](https://github.com/Ciastex/Atlas.UI)

[![Build status](https://ci.appveyor.com/api/projects/status/nbyxtgdgyxj71aq4?svg=true)](https://ci.appveyor.com/project/Ciastex/atlas-ui-core)
[![NuGet Release](https://img.shields.io/nuget/v/Atlas.UI.Core.svg?colorB=%23007ACC)](https://www.nuget.org/packages/Atlas.UI)
[![MIT License](https://img.shields.io/github/license/Ciastex/Atlas.UI.Core.svg)](https://github.com/Ciastex/Atlas.UI.Core/blob/master/LICENSE.md)
### Atlas.UI
Dark and elegant UI toolkit inspired by Visual Studio design for WPF. Tries to extend on the design by chipping in its own features.

### Installation instructions
1. Add a reference to the library in your project.
2. In your `App.xaml`, in `<Application.Resources />` tag, add this:
```
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="pack://application:,,,/Atlas.UI;component/Themes/Atlas.xaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```
This will override styles of `ScrollViewer`, `TextBlock`, `DataGrid` and a few other built-in controls that Atlas does not extend - because of that, the toolkit assumes you won't be using other (including the default) styles in other parts of your application.

The namespace for all controls is `Atlas.UI`. Whenever you want to use an extended control from the toolkit, add this to your namespace declarations: 
```
xmlns:atlasw="clr-namespace:Atlas.UI.Windows;assembly=Atlas.UI"
xmlns:atlasc="clr-namespace:Atlas.UI.Controls;assembly=Atlas.UI"
```
