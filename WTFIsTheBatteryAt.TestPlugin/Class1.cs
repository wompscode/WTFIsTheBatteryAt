using WTFIsTheBatteryAt.Plugins;
using System.Windows.Forms;
using System.Timers;
using static WTFIsTheBatteryAt.Logging;
namespace WTFIsTheBatteryAt.DualSensePlugin
{
    public class Class1 : IPlugin
    {
        Settings? settings = null;
        public PluginInformation.Information Information => new PluginInformation.Information
        {
            Name = "DualSensePlugin",
            InternalName = "dualsense_plugin",
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

            if(settings != null)
            {
                settings.Hide();
                settings.Dispose();
            }
        }

        public void Init()
        {
            Log("DualSense Plugin started.", $"{Information.Name}");

            _ = Loop();
        }
        private bool connectionTimer = true;
        public async Task Loop()
        {
            while (true)
            {
                if(connectionTimer == true)
                {
                    try
                    {
                        if (!DualSense.dualsenseStarted)
                        {
                            Log("connectionTimer_Tick(): Attempting to connect controller.");

                            DualSense.DS_GetDev(DualSense.controllerPlayer);
                            if (DualSense.dev == null) return;

                            Log("connectionTimer_Tick(): Controller found.");

                            DualSense.dualsenseStarted = true;

                            //Tick();

                            Log("DS_WriteData() is called twice to send the init packet, so all functionality works", "[info]");
                            if (DualSense.devBTInit == false) DualSense.DS_WriteData(Color.Blue); // In case it's not been initialized.
                            DualSense.DS_WriteData(DualSense.dualsenseColor);

                            connectionTimer = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.StartsWith("Couldn't connect controller."))
                        {
                            MessageBox.Show("Something has happened, and I have no idea what to do from here.\n" + ex.StackTrace);
                            Application.Exit();
                        }
                        else
                        {
                            Log("connectionTimer_Tick(): No controller.");
                        }
                    }
                } 
                await Task.Delay(2000);
            }
        }
        Random x = new Random();

        public event IPlugin.ConnectionStateChangedEventHandler? ConnectionStateChanged;
        public event IPlugin.DataChangedEventHandler? DataChanged;

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
            Log("heartbeat");
            if(DualSense.dev != null)
            {
                DualSense.DS_ReadData();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(true));
                Structs.DeviceBattery[] batteries =
                [
                    new Structs.DeviceBattery
                    {
                        Device = "dualsense",
                        Percentage = DualSense.devBatteryPercent
                    }
                ];
                DataChanged?.Invoke(this, batteries);

            } else
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(false));
            }


        }

        public void OpenUI()
        {
            if(settings == null || settings.IsDisposed) settings = new Settings();
            if (settings.Visible) settings.Hide(); else settings.Show();
        }
    }
}
