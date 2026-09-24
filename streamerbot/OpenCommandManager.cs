using System;
using System.IO;
using System.Reflection;

#if COMMAND_MANAGER_LAUNCHER_SYNTAX_CHECK
public class CPHInline : CPHInlineBase
#else
public class CPHInline
#endif
{
    private const string DllName = "CreatorForge.CommandManager.dll";
    private const string EntryTypeName = "CreatorForge.CommandManager.Core.CommandManagerApp";
    private const string EntryMethodName = "Show";

    public bool Execute()
    {
        try
        {
            string root = ResolveStreamerBotRoot();
            if (string.IsNullOrWhiteSpace(root))
            {
                CPH.LogError("[CF Command Manager] Could not resolve the Streamer.bot installation folder.");
                return false;
            }

            string dllPath = Path.Combine(root, "dll", DllName);
            if (!File.Exists(dllPath))
            {
                CPH.LogError("[CF Command Manager] Native UI DLL was not found: " + dllPath);
                return false;
            }

            Assembly assembly = FindLoadedAssembly(dllPath) ?? Assembly.LoadFrom(dllPath);
            Type entryType = assembly.GetType(EntryTypeName, true);
            MethodInfo showMethod = entryType.GetMethod(
                EntryMethodName,
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(object) },
                null);

            if (showMethod == null)
            {
                CPH.LogError("[CF Command Manager] Entry method was not found: " + EntryTypeName + "." + EntryMethodName + "(object)");
                return false;
            }

            showMethod.Invoke(null, new object[] { CPH });
            CPH.LogInfo("[CF Command Manager] Native UI launch requested successfully.");
            return true;
        }
        catch (TargetInvocationException ex)
        {
            Exception cause = ex.InnerException ?? ex;
            CPH.LogError("[CF Command Manager] Native UI entry point failed: " + cause);
            return false;
        }
        catch (Exception ex)
        {
            CPH.LogError("[CF Command Manager] Native UI launch failed: " + ex);
            return false;
        }
    }

    private static Assembly FindLoadedAssembly(string dllPath)
    {
        string expectedPath = Path.GetFullPath(dllPath);

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(assembly.Location) &&
                    string.Equals(Path.GetFullPath(assembly.Location), expectedPath, StringComparison.OrdinalIgnoreCase))
                {
                    return assembly;
                }
            }
            catch
            {
                // Dynamic assemblies may not expose a usable Location.
            }
        }

        return null;
    }

    private static string ResolveStreamerBotRoot()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(baseDirectory) && Directory.Exists(baseDirectory))
                return baseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        catch
        {
        }

        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!string.IsNullOrWhiteSpace(currentDirectory) && Directory.Exists(currentDirectory))
                return currentDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        catch
        {
        }

        return null;
    }
}
