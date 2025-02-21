namespace WTFIsTheBatteryAt.Plugins
{
    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public bool Connected { get; }

        public ConnectionStateChangedEventArgs (bool connected)
        {
            Connected = connected;
        }
    }

    public interface IPlugin
    {
        public delegate void ConnectionStateChangedEventHandler (object sender, ConnectionStateChangedEventArgs e);
        public event ConnectionStateChangedEventHandler ConnectionStateChanged;

        public delegate void DataChangedEventHandler(object sender, Structs.DeviceBattery[] data);
        public event DataChangedEventHandler DataChanged;

        public PluginInformation.Information Information { get; }
        
        [Obsolete] public Structs.DeviceBattery PollData();

        public void Init();

        public void Dispose();

        public void Heartbeat();

        public void OpenUI();
    }
}
