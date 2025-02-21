using WTFIsTheBatteryAt.Plugins;

namespace WTFIsTheBatteryAt.TestPlugin
{
    public class Class1 : IPlugin
    {
        public PluginInformation.Information Information => new PluginInformation.Information
        {
            Name = "TestPlugin",
            Author = "wompscode",
            Device = "none",
            OSSupport = new Structs.OSSupport
            {
                Windows = true,
                Linux = false,
                Mac = false
            }
        };

        public void Dispose()
        {
            Console.WriteLine("Goodbye from TestPlugin.");
        }

        public void Init()
        {
            Console.WriteLine("Hello from TestPlugin.");
        }
        Random x = new Random();

        public event IPlugin.ConnectionStateChangedEventHandler ConnectionStateChanged;
        public event IPlugin.DataChangedEventHandler DataChanged;

        public Structs.DeviceBattery PollData()
        {
            return new Structs.DeviceBattery
            {
                Device = "none",
                Percentage = x.Next(1, 100)
            };
        }

        public void Heartbeat()
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(x.Next(0,2) == 1));
            Structs.DeviceBattery[] batteries =
            [
                new Structs.DeviceBattery
                {
                    Device = "device one",
                    Percentage = x.Next(1, 100)
                },
                new Structs.DeviceBattery
                {
                    Device = "device two",
                    Percentage = x.Next(1, 100)
                },
                new Structs.DeviceBattery
                {
                    Device = "device three",
                    Percentage = x.Next(1, 100)
                },
            ];
            DataChanged?.Invoke(this, batteries);
        }

    }
}
