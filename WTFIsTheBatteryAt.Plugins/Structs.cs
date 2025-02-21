namespace WTFIsTheBatteryAt.Plugins
{
    public class Structs
    {
        public struct DeviceBattery
        {
            public string Device { get; init; }
            public int Percentage { get; init; }
        }

        public struct OSSupport
        {
            public bool Windows { get; init; }
            public bool Mac { get; init; }
            public bool Linux { get; init; }
        }
    }
}
