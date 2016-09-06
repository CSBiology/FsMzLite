namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FsMzLite")>]
[<assembly: AssemblyProductAttribute("FsMzLite")>]
[<assembly: AssemblyDescriptionAttribute("FsMzLite is a scripting friendly Wrapper for the MzLite library.")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
    let [<Literal>] InformationalVersion = "1.0"
