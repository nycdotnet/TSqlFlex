using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TSqlFlex.Core")]
[assembly: AssemblyDescription("Scripts SQL Server data.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("TSqlFlex.Core")]
[assembly: AssemblyCopyright("Copyright © 2015 Steve Ognibene")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("0.1.0.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]
[assembly: AssemblySemverPrereleaseTag("beta")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f28ef710-39c6-4254-a0db-e930d24f2372")]

//Enable testing internal members of classes.
[assembly: InternalsVisibleTo("TSqlFlex.Core.Tests")]

[AttributeUsage(AttributeTargets.Assembly)]
public class AssemblySemverPrereleaseTag : Attribute
{
    public readonly string tag;
    public AssemblySemverPrereleaseTag() : this(string.Empty) { }
    public AssemblySemverPrereleaseTag(string value)
    {
        tag = value;
    }
}