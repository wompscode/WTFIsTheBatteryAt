using System.Reflection;
using WTFIsTheBatteryAt.Plugins;
using static WTFIsTheBatteryAt.Logging;

namespace WTFIsTheBatteryAt
{
    internal static class PluginLoader
    {
        public const int APILevel = 1;
        public static List<IPlugin> Loaded = new List<IPlugin>();
        public static List<string> FailedToLoad = new List<string>();

        public static void LoadPlugins()
        {
            FailedToLoad.Clear();
            Type[] externals = GetPluginExternalClasses("plugins");

            if(externals.Length == 0)
            {
                Log("No plugins to load.", "[plugins]");
                return;
            }

            foreach (var type in externals) { 
                var instance = Activator.CreateInstance(type) as IPlugin;

                if(instance != null)
                {
                    if(Loaded.Count > 0 && Loaded.First(x=>x.Information.Name == instance.Information.Name) != null)
                    {
                        Log($"Cannot load two plugins with the same name. {instance.Information.Name} already exists in loaded plugin list.", "[plugins]");
                        return;
                    }
                    instance.Init();
                    Log($"{instance.Information.Name}:\nAuthor: {instance.Information.Author}\nDevice: {instance.Information.Device}\nOS Support: W: {instance.Information.OSSupport.Windows}, M: {instance.Information.OSSupport.Mac}, L: {instance.Information.OSSupport.Linux}", "[plugins]");
                    Loaded.Add(instance);
                }
            }
        }

        public static bool UnloadPlugin(IPlugin plugin)
        {
            try
            {
                plugin.Dispose();
                Loaded.Remove(plugin);
                return true;
            } catch (Exception ex) {
                Log($"Failed to unload {plugin.Information.Name}: {ex.Message}", "[plugins]");
                return false;
            }
        }

        private static Type[] GetPluginExternalClasses(string path)
        {
            var externs = new List<Type>();

            IEnumerable<string> assemblies = FindPlugins(path);
            foreach (string assemblyName in assemblies)
            {
                try
                {
                    var assembly = Assembly.LoadFile(assemblyName);
                    var exported = assembly.GetExportedTypes();
                    
                    var externalClasses = exported.Where(x => x.IsAssignableTo(typeof(IPlugin))).ToArray();
                    if (externalClasses.Length > 0)
                        foreach (var _class in externalClasses)
                            externs.Add(_class);
                    
                }
                catch (Exception ex)
                {
                    Log($"{assemblyName} failed to load:\n{ex.Message}", "[plugins]");
                    FailedToLoad.Add(assemblyName);
                }
            }

            return externs.ToArray();
        }

        private static IEnumerable<string> FindPlugins(string path)
        {
            if (!Directory.Exists(path)) yield break;
            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.dll");
            foreach (var item in files)
                yield return Path.GetFullPath(item);
        }

    }
}
