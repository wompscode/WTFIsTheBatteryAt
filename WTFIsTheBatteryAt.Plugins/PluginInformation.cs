namespace WTFIsTheBatteryAt.Plugins
{
    public class PluginInformation
    {
        public struct Information
        {
            public string Name { get; init; }
            public string InternalName { get; init; }
            public string Author { get; init; }
            public string Device { get; init; }
            public Structs.OSSupport OSSupport { get; init; }
        }            
    }
}
