using System;
using System.Reflection;

namespace CreatorForge.CommandManager.Core
{
    internal static class UiVersion
    {
        public static string Display
        {
            get
            {
                Version version = typeof(UiVersion).Assembly.GetName().Version;
                return version == null ? "0.0.0" : version.ToString(3);
            }
        }
    }
}
