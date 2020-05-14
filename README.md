# KMeansColorSort
Experimental color sorting demo using K-Means for banding.

# Method
Colors are clustered into bands via K-Means according to their weighted HSL color components, then colors within each band are sorted in hue-saturation-lightness priority.

# Screenshots
![Color Sorting](https://i.imgur.com/T4dfpe7.png)

## Tech Stack
* C# / .NET Core 3.1 / WPF
* [Accord.NET](http://accord-framework.net/) (K-Means)
* [Stylet](https://github.com/canton7/Stylet) (MVVM)
* [Autofac](https://github.com/autofac/Autofac) (IoC)
* [ModernWpfUI](https://github.com/Kinnara/ModernWpf)
