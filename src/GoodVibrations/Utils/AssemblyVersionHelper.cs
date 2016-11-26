using System.Reflection;

namespace GoodVibrations.Utils
{
    /// <summary>
    /// Utility to determine the current assembly version
    /// </summary>
    public static class AssemblyVersionHelper
    {
        /// <summary>
        /// Returns the current assembly version
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion ()
        {
            var assembly = typeof (AssemblyVersionHelper).GetTypeInfo ().Assembly;
            var assemblyName = new AssemblyName (assembly.FullName);
            return $"{assemblyName.Version.Major}.{assemblyName.Version.Minor}.{assemblyName.Version.Build}";
        }
    }
}
