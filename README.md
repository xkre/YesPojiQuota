# YesPojiQuota

[![CodeFactor](https://www.codefactor.io/repository/github/xkre/yespojiquota/badge)](https://www.codefactor.io/repository/github/xkre/yespojiquota)

YesPojiQuota is an UWP app that helps with checking Yes 4G UTM (Universiti Teknologi Malaysia) internet balance. This app can also be used to log in into Yes UTM wifi.
  
[Windows store link](https://www.microsoft.com/store/productId/9NBD0D1PBZ51)


### Compilation Instruction
1. Create a file Secrets.cs in YesPojiQuota.Core/Helpers containing the following:
    ```
    namespace YesPojiQuota.Core.Helpers
    {
        public static partial class Constants
        {
            public static string KEY = "secretkey";
        }
    }
    ```
#### Requirement
Windows 10 Anniversary Update SDK  
Visual Studio 2017
