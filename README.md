# smaug-cs
Automatically exported from code.google.com/p/smaug-cs

SmaugCS is based on SmaugFUSS 1.9 which was obtained from Nick Gammon's website (http://www.mushclient.com/downloads/dlsmaug.htm).

The project uses the following third-party libraries:
 * NCalc (v1.3.8) http://ncalc.codeplex.com/
 * Log4net (v2.0.3) http://logging.apache.org/log4net/
 * VikingErik.LuaInterface? (v1.0.42) http://www.nuget.org/packages/VikingErik.LuaInterface
 * NUnit (v2.6.3) http://nunit.org/
 * Moq (v4.2.13) https://code.google.com/p/moq/

Details of the project tools and specs:
 * Language: C# (.NET 4.5)
 * Microsoft Visual Studio 2013
 * JetBrains Resharper 7.1.2 http://www.jetbrains.com/resharper/
 * Remco Software NCrunch 2.4.0.2 http://www.ncrunch.net/
 * Notepad++ (for Lua development) http://notepad-plus-plus.org/
 * Microsoft Sql Server 2012 Express http://www.microsoft.com/en-us/download/details.aspx?id=29062

The goals of this project are:
 * Make the SmaugFUSS code easier to maintain for non-C developers
 * Organize and clean-up much of the codebase
 * Replaced out-dated code (list and string handling in particular) with .NET Library code or extensions
 * Replaced out-dated #defines and structs with class objects and enums
 * Use Windows sockets instead of the existing network layer
 * Replace file-handling with a more robust file handling system
 * Lastly, get the code back into working (and running) order

The following third-party libraries have been used (thus far) in the project.

For the Project:



