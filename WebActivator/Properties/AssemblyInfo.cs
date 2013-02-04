using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WebActivator")]
[assembly: AssemblyDescription("A NuGet package that allows other packages to execute some startup code in web apps")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("David Ebbo")]
[assembly: AssemblyProduct("WebActivator")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3bc078bd-ade4-4271-964f-1d041508c419")]

[assembly: InternalsVisibleTo("WebActivatorTest, PublicKey=00240000048000009400000006020000002400005253413100040000010" +
                                                          "00100a5c110967144cbbc9d6b7575ebd7ec573baf87095e9a57432ef4c2" +
                                                          "f3a1310b748ec315aba53565d854da9e72eca971a0a75b20c779d5e7c38" +
                                                          "8c51f9fbc5ec47ae77a9f200553388c06b36c6eb8d45c04d9171b1af166" +
                                                          "ed8f246c2ea023793ae540a0026f9f0a7539105a0c0519f0e56700f04a7" +
                                                          "ae2a919e6af7dbb528f507ec1")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.5.3")]

[assembly: PreApplicationStartMethod(typeof(WebActivator.ActivationManager), "Run")]
