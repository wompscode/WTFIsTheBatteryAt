namespace WTFIsTheBatteryAt
{
    using HidSharp;


    /*
     * Thanks WujekFoliarz for your work on Wujek-Dualsense-API!
     * I picked it apart because I only needed some functionality of it, and wanted to make this lighter. Your work is well appreciated.
     */
    public partial class Form1 : Form
    {
        public static int controllerPlayer = 0;
        public static bool dualsenseStarted = false;

        public static Color dualsenseColor = Properties.Settings.Default.LightbarColour;
        public static int warningThreshold = Properties.Settings.Default.WarningThreshold;
        public static int tickRate = Properties.Settings.Default.TickRate;


        public static int devLength = 0;
        public static string devID = "";
        public static bool devROE = false;
        public static bool devBT = false;
        public static int devNum = 0;
        public static bool devBTInit = false;
        public static DeviceStream? dev;

        public static int devBatteryPercent = 0;
        public static BatteryState devBatteryState = 0;

        public static bool warned = false;
        public static bool closing = false;

        public Form1()
        {
            InitializeComponent();

            FormClosing += Form1_FormClosing;
            Resize += Form1_Resize;
            numericUpDown3.Leave += NumericUpDown3_Leave;
            numericUpDown2.Leave += NumericUpDown2_Leave;
            numericUpDown3.Enter += NumericUpDown3_Enter;
            numericUpDown2.Enter += NumericUpDown2_Enter;
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Console.WriteLine($"Form1_FormClosing(): Exiting..");
            if (dev != null)
            {
                Console.WriteLine($"Form1_FormClosing(): Triggering dispose");
                DS_Dispose(devNum);
            };
            Properties.Settings.Default.Save();

            closing = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox1.BackColor = dualsenseColor;
            checkBox1.BackColor = dualsenseColor;
            checkBox1.Checked = Properties.Settings.Default.ShouldSetLight;

            label2.BackColor = dualsenseColor;
            label2.ForeColor = IdealTextColor(dualsenseColor);

            colorDialog1.Color = dualsenseColor;

            label1.Text = "";

            numericUpDown2.Value = warningThreshold;

            numericUpDown3.Value = tickRate;
            timer1.Interval = tickRate;
#if DEBUG
            tabControl1.Dock = DockStyle.None;
            label5.Visible = true;
            timer2.Enabled = true;
#else
            tabControl1.Dock = DockStyle.Fill;
            label5.Visible = false;
            timer2.Enabled = false;
#endif
        }

        public void Tick(bool warning = false)
        {
            Console.WriteLine("Tick(): Reached.");
            if (dev == null) return; else Console.WriteLine("Tick(): Device is not null");

            DS_ReadData();

            string _batStateTextFull = "??";
            string _batStateTextSmall = "?";

            switch(devBatteryState)
            {
                case BatteryState.POWER_SUPPLY_STATUS_DISCHARGING:
                    _batStateTextFull = "discharging";
                    _batStateTextSmall = "d";
                    break;
                case BatteryState.POWER_SUPPLY_STATUS_CHARGING:
                    _batStateTextFull = "charging";
                    _batStateTextSmall = "c";
                    break;
                case BatteryState.POWER_SUPPLY_STATUS_FULL:
                    _batStateTextFull = "full";
                    _batStateTextSmall = "f";
                    break;
                case BatteryState.POWER_SUPPLY_STATUS_ERROR:
                    _batStateTextFull = "error";
                    _batStateTextSmall = "e";
                    break;
                case BatteryState.POWER_SUPPLY_TEMP_OR_VOLTAGE_OUT_OF_RANGE:
                    _batStateTextFull = "temp/volt";
                    _batStateTextSmall = "t/v";
                    break;
                case BatteryState.POWER_SUPPLY_STATUS_NOT_CHARGING:
                    _batStateTextFull = "not charging";
                    _batStateTextSmall = "nc";
                    break;
            }

            label1.Text = $"Battery: {devBatteryPercent}% [{_batStateTextFull}]";
            notifyIcon1.Text = $"WTFITBA: {devBatteryPercent}% [{_batStateTextSmall}]";

            if (devBatteryPercent < warningThreshold && warned == false && warning == true)
            {
                ShowNotification($"Battery below {warningThreshold}%, current: {devBatteryPercent}%", 2);

                warned = true;
            }

            if (checkBox1.Checked) DS_WriteData(dualsenseColor);
        }

        private void DS_GetDev(int controller = 0)
        {
            List<HidDevice> devices = new List<HidDevice>();
            foreach (var dev in DeviceList.Local.GetHidDevices())
            {
                if (dev.VendorID == 1356 && dev.ProductID == 3302)
                {
                    devices.Add(dev);
                    // DualSense
                }
                else if (dev.VendorID == 1356 && dev.ProductID == 3570)
                {
                    devices.Add(dev);
                    // DualSense Edge
                }
            }

            try
            {
                dev = devices[controller].Open();
                devID = devices[controller].DevicePath;
                devROE = devices[controller].VendorID == 1536 && devices[controller].ProductID == 3570;
                devLength = devices[controller].GetMaxOutputReportLength();
                devBTInit = devBTInit ? devBTInit : false;
                devNum = controller;

                if (!devROE)
                    devBT = devLength >= 78;
                else
                    devBT = devLength >= 94;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception("Couldn't connect controller.");
            }
        }
        public enum BatteryState
        {
            // https://github.com/WujekFoliarz/Wujek-Dualsense-API/blob/master/Wujek%20Dualsense%20API/BatteryState.cs

            POWER_SUPPLY_STATUS_DISCHARGING = 0x0,
            POWER_SUPPLY_STATUS_CHARGING = 0x2,
            POWER_SUPPLY_STATUS_FULL = 0x1,
            POWER_SUPPLY_STATUS_NOT_CHARGING = 0xb,
            POWER_SUPPLY_STATUS_ERROR = 0xf,
            POWER_SUPPLY_TEMP_OR_VOLTAGE_OUT_OF_RANGE = 0xa,
            POWER_SUPPLY_STATUS_UNKNOWN = 0x0
        }
        private void DS_ReadData()
        {
            Console.WriteLine($"DS_ReadData(): Reached.");

            try
            {
                if (dev == null) return;
                Console.WriteLine($"DS_ReadData(): dev not null");
                byte[] deviceStates = new byte[devLength];
                dev.Read(deviceStates);
                int tempOffset = 0;
                if (devBT) tempOffset = 1;

                byte faceButtons = deviceStates[8 + tempOffset];
                Console.WriteLine($"DS_ReadData(): cross: {(faceButtons & (1 << 5)) != 0}"); // For debug purposes.

                if (devBT)
                {
                    Console.WriteLine($"DS_ReadData(): {(BatteryState)((byte)(deviceStates[53 + tempOffset] & 0xF0) >> 4)}");

                    devBatteryState = (BatteryState)((byte)(deviceStates[53 + tempOffset] & 0xF0) >> 4);
                    devBatteryPercent = Math.Min((int)((deviceStates[53 + tempOffset] & 0x0F) * 10 + 5), 100);
                }
                else
                {
                    devBatteryState = BatteryState.POWER_SUPPLY_STATUS_UNKNOWN;
                    devBatteryPercent = 100;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                OnDisconnect();
                DS_Dispose(devNum);
            }
        }

        private void DS_WriteData(Color col)
        {
            Console.WriteLine("DS_WriteData(): Reached.");
            byte[] outputDevStates = new byte[devLength];

            if (devBT)
            {
                Console.WriteLine($"DS_WriteData(): devBT true");

                int[] rtf = new int[7];
                int[] ltf = new int[7];
                outputDevStates[0] = 0x31;
                outputDevStates[1] = 2;
                outputDevStates[2] = (byte)0xFC;
                if (devBTInit == false)
                {
                    outputDevStates[3] = 0x1 | 0x2 | 0x4 | 0x8 | 0x10 | 0x40;
                    devBTInit = true;
                }
                else
                    outputDevStates[3] = (byte)0x57;
                outputDevStates[4] = (byte)0; // right low freq motor 0-255
                outputDevStates[5] = (byte)0; // left low freq motor 0-255
                outputDevStates[10] = (byte)0; //microphone led
                outputDevStates[11] = (byte)0x10;
                outputDevStates[12] = (byte)0;
                outputDevStates[13] = (byte)rtf[0];
                outputDevStates[14] = (byte)rtf[1];
                outputDevStates[15] = (byte)rtf[2];
                outputDevStates[16] = (byte)rtf[3];
                outputDevStates[17] = (byte)rtf[4];
                outputDevStates[18] = (byte)rtf[5];
                outputDevStates[21] = (byte)rtf[6];
                outputDevStates[23] = (byte)0;
                outputDevStates[24] = (byte)ltf[0];
                outputDevStates[25] = (byte)ltf[1];
                outputDevStates[26] = (byte)ltf[2];
                outputDevStates[27] = (byte)ltf[3];
                outputDevStates[28] = (byte)ltf[4];
                outputDevStates[29] = (byte)ltf[5];
                outputDevStates[32] = (byte)ltf[6];
                outputDevStates[40] = (byte)0;
                outputDevStates[43] = (byte)0;
                outputDevStates[44] = (byte)0;
                outputDevStates[45] = (byte)0;
                outputDevStates[45] = (byte)0;
                outputDevStates[46] = (byte)col.R;
                outputDevStates[47] = (byte)col.G;
                outputDevStates[48] = (byte)col.B;

                uint crcChecksum = CRC32.ComputeCRC32(outputDevStates, 74);
                byte[] checksum = BitConverter.GetBytes(crcChecksum);
                Array.Copy(checksum, 0, outputDevStates, 74, 4);
            }
            else
            {
                Console.WriteLine($"DS_WriteData(): devBT false");

                int[] rtf = new int[7];
                int[] ltf = new int[7];
                outputDevStates[0] = 2;
                outputDevStates[1] = (byte)0xFC;
                outputDevStates[2] = (byte)0x57;
                outputDevStates[3] = (byte)0; // right low freq motor 0-255
                outputDevStates[4] = (byte)0; // left low freq motor 0-255
                outputDevStates[5] = 0x7C; // <-- headset volume
                outputDevStates[6] = (byte)100; // <-- speaker volume
                outputDevStates[7] = (byte)35; // <-- mic volume
                outputDevStates[8] = (byte)0x31; // <-- audio output
                outputDevStates[9] = (byte)0; //microphone led
                outputDevStates[10] = (byte)0x10;
                outputDevStates[11] = (byte)0;
                outputDevStates[12] = (byte)rtf[0];
                outputDevStates[13] = (byte)rtf[1];
                outputDevStates[14] = (byte)rtf[2];
                outputDevStates[15] = (byte)rtf[3];
                outputDevStates[16] = (byte)rtf[4];
                outputDevStates[17] = (byte)rtf[5];
                outputDevStates[20] = (byte)rtf[6];
                outputDevStates[22] = (byte)0;
                outputDevStates[23] = (byte)ltf[0];
                outputDevStates[24] = (byte)ltf[1];
                outputDevStates[25] = (byte)ltf[2];
                outputDevStates[26] = (byte)ltf[3];
                outputDevStates[27] = (byte)ltf[4];
                outputDevStates[28] = (byte)ltf[5];
                outputDevStates[31] = (byte)ltf[6];
                outputDevStates[39] = (byte)0;
                outputDevStates[41] = (byte)0;
                outputDevStates[42] = (byte)0;
                outputDevStates[43] = (byte)0;
                outputDevStates[44] = (byte)0;
                outputDevStates[45] = (byte)col.R;
                outputDevStates[46] = (byte)col.G;
                outputDevStates[47] = (byte)col.B;
            }

            try
            {
                Console.WriteLine($"DS_WriteData(): trying to write to device");

                if (dev != null) dev.WriteAsync(outputDevStates, 0, devLength); else Console.WriteLine("DS_WriteData(): device is null?");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"DS_WriteData(): failed to write to device");
                Console.WriteLine(exception.StackTrace);
            }
        }

        private void DS_Dispose(int num)
        {
            if (dev != null)
            {
                Console.WriteLine("DS_Dispose(): colour reset");
                DS_WriteData(Color.Blue);

                dev.Dispose();
                Console.WriteLine("DS_Dispose(): device disposed");
            };
        }

        private void OnDisconnect()
        {
            button1.Text = "Connect";
            dualsenseStarted = false;
            warned = false;
            label1.Text = "";
            timer1.Stop();

            notifyIcon1.Text = "WTFITBA: disconnected";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dualsenseStarted)
            {
                button1.Text = "Connect";
                dualsenseStarted = false;
                warned = false;
                label1.Text = "";
                timer1.Stop();

                notifyIcon1.Text = "WTFITBA: disconnected";
                if (dev != null) DS_Dispose(controllerPlayer);
                return;
            }
            try
            {
                DS_GetDev(controllerPlayer);
            }
            catch
            {
                ShowNotification($"Failed to find Dualsense at player {controllerPlayer}. Is there one connected?", 3);
            }

            if (dev != null)
            {

                button1.Text = "Disconnect";

                dualsenseStarted = true;
                warned = false;

                Thread.Sleep(10);
                Tick();
                if (devBTInit == false) DS_WriteData(Color.Blue); // Incase it's not been initialized.
                DS_WriteData(dualsenseColor);

                timer1.Start();
            }
        }

        private void ShowNotification(string text, int iconType = 0)
        {
            switch (iconType)
            {
                case 0:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.None;
                    break;
                case 1:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    break;
                case 2:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                    break;
                case 3:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                    break;
            }

            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = "WTFIsTheBatteryAt";
            notifyIcon1.ShowBalloonTip(0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tick(true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            controllerPlayer = (int)numericUpDown1.Value;
        }

        public Color IdealTextColor(Color bg)
        {
            // Borrowed from https://www.codeproject.com/Articles/16565/Determining-Ideal-Text-Color-Based-on-Specified-Ba (thanks guys!)
            int nThreshold = 105;
            int bgDelta = Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) +
                                          (bg.B * 0.114));

            Color foreColor = (255 - bgDelta < nThreshold) ? Color.Black : Color.White;
            return foreColor;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
                checkBox1.BackColor = colorDialog1.Color;
                label2.BackColor = colorDialog1.Color;
                label2.ForeColor = IdealTextColor(colorDialog1.Color);

                dualsenseColor = colorDialog1.Color;

                Properties.Settings.Default.LightbarColour = colorDialog1.Color;
                Properties.Settings.Default.Save();

                if (dev != null)
                {
                    if (checkBox1.Checked) DS_WriteData(dualsenseColor);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            pictureBox1_Click(sender, e);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void NumericUpDown2_Enter(object? sender, EventArgs e)
        {
            if (dualsenseStarted) timer1.Stop();
        }

        private void NumericUpDown2_Leave(object? sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            if (dualsenseStarted) { timer1.Start(); warned = false; }
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            warningThreshold = (int)numericUpDown2.Value;

            Properties.Settings.Default.WarningThreshold = (int)numericUpDown2.Value;
        }

        private void NumericUpDown3_Enter(object? sender, EventArgs e)
        {
            if (dualsenseStarted) timer1.Stop();
        }

        private void NumericUpDown3_Leave(object? sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            if (dualsenseStarted) timer1.Start();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            tickRate = (int)numericUpDown3.Value;

            timer1.Interval = tickRate;

            Properties.Settings.Default.TickRate = (int)numericUpDown3.Value;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label5.Text = $"Timer1: {timer1.Enabled}\nInterval: {timer1.Interval}\nBS: {devBatteryState}";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShouldSetLight = checkBox1.Checked;
            Properties.Settings.Default.Save();

        }
    }
}