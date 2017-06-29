using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content.PM;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle ("GoodVibrations.Droid")]
[assembly: AssemblyDescription ("")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyCompany ("h3ko GmbH")]
[assembly: AssemblyProduct ("")]
[assembly: AssemblyCopyright ("rherlt")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion ("1.0.0")]

[assembly: UsesPermission(Android.Manifest.Permission.ReceiveBootCompleted)]
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission("com.google.android.c2dm.permission.RECEIVE")]
[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE", ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission("@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission("com.google.android.c2dm.permission.SEND")]

// The following attributes are used to specify the signing key for the assembly, 
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
