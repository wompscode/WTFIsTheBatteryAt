namespace WTFIsTheBatteryAt
{
    using HidSharp;
    using System.Diagnostics;
    using static Logging;
    using static Notifications;
    using static ColourFunctions;

    // WTFIsTheBatteryAt
    //  Because I didn't want to use PlayStation Accessories anymore.

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
        public static Color trayColor = Properties.Settings.Default.TrayColour;
        public static Point trayOffset = Properties.Settings.Default.TrayOffset;    
        public static Font trayFont = Properties.Settings.Default.TrayFont;
        public static IconGenerator icon = new IconGenerator();

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
        public static bool valuesLoaded = false;
        public static bool closing = false;

        public Form1()
        {
            InitializeComponent();
            Log("Form1(): Initializing events.");

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
                Log("Form1_Resize(): Hiding window.");

                Hide();
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Log($"Form1_FormClosing(): Exiting.");

            if (dev != null)
            {
                Log($"Form1_FormClosing(): Triggering dispose.");

                DS_Dispose(devNum);
            };
            Properties.Settings.Default.Save();

            closing = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"WTFIsTheBatteryAt {Program.version}";
            Log($"Form1_Load(): Generating n/a icon..");
            icon.Generate("n/a", Properties.Settings.Default.TrayFont, Properties.Settings.Default.TrayColour, Properties.Settings.Default.TrayOffset, notifyIcon1);

            Log($"Form1_Load(): Loading settings into window..");


            Log($"Form1_Load(): dualsenseColor: {dualsenseColor.Name}");
            pictureBox1.BackColor = dualsenseColor;
            checkBox1.BackColor = dualsenseColor;
            Log($"Form1_Load(): ShouldSetLight: {Properties.Settings.Default.ShouldSetLight}");
            checkBox1.Checked = Properties.Settings.Default.ShouldSetLight;

            label2.BackColor = dualsenseColor;
            label2.ForeColor = IdealTextColor(dualsenseColor);

            colorDialog1.Color = dualsenseColor;

            Log($"Form1_Load(): trayColor: {trayColor.Name}");
            label5.ForeColor = IdealTextColor(trayColor);
            label5.BackColor = trayColor;
            pictureBox2.BackColor = trayColor;
            colorDialog2.Color = trayColor;

            label1.Text = "";

            Log($"Form1_Load(): trayFont: {trayFont.Name}, {trayFont.Size}");
            fontDialog1.Font = trayFont;
            button2.Text = $"{trayFont.Name}, {trayFont.Size}";

            Log($"Form1_Load(): trayOffset: x: {trayOffset.X}, y: {trayOffset.Y}");
            numericUpDown5.Value = trayOffset.X;
            numericUpDown6.Value = trayOffset.Y;

            Log($"Form1_Load(): warningThreshold: {warningThreshold}");
            numericUpDown2.Value = warningThreshold;

            Log($"Form1_Load(): tickRate: {tickRate}");
            numericUpDown3.Value = tickRate;
            updateTimer.Interval = tickRate;
#if DEBUG
            tabControl1.Dock = DockStyle.None;
            textBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox2.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown4.Visible = true;
            debugTimer.Enabled = true;
#else
            tabControl1.Dock = DockStyle.Fill;
            textBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox2.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown4.Visible = false;
            debugTimer.Enabled = false;
#endif
            valuesLoaded = true;
        }

        public void Tick(bool warning = false)
        {
            Log("Tick(): Reached.");

            if (dev == null) return;
            else
            {
                Log("Tick(): Device is not null.");
            }
            DS_ReadData();

            string _batStateTextFull = "??";
            string _batStateTextSmall = "?";

            switch (devBatteryState)
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
            notifyIcon1.Icon = null;
            notifyIcon1.Text = $"WTFITBA: {devBatteryPercent}% [{_batStateTextSmall}]";
            if(dualsenseStarted) icon.Generate(devBatteryPercent.ToString(), Properties.Settings.Default.TrayFont, Properties.Settings.Default.TrayColour, Properties.Settings.Default.TrayOffset, notifyIcon1);

            if (devBatteryPercent < warningThreshold && warned == false && warning == true)
            {
                ShowNotification(notifyIcon1, "WTFIsTheBatteryAt", $"Battery below {warningThreshold}%, current: {devBatteryPercent}%", 2);

                warned = true;
            }

            if (checkBox1.Checked) DS_WriteData(dualsenseColor);
        }

        private void DS_GetDev(int controller = 0)
        {
            Log($"DS_GetDev(): Reached.");
            if (dev != null)
            {
                Log($"DS_GetDev(): Previous dev present - disposing.");
                dev.Dispose();
            }
            List<HidDevice> devices = new List<HidDevice>();
            foreach (var dev in DeviceList.Local.GetHidDevices())
            {
                if (dev.VendorID == 1356 && dev.ProductID == 3302)
                {
                    devices.Add(dev);
                    Log($"DS_GetDev(): DualSense detected (1356&3302)");

                    // DualSense
                }
                else if (dev.VendorID == 1356 && dev.ProductID == 3570)
                {
                    devices.Add(dev);
                    Log($"DS_GetDev(): DualSense Edge detected (1356&3570)");

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

                Log($"DS_GetDev(): New controller info: " +
                    $"{(devROE ? "DualSense Edge" : "DualSense")}: {devNum} {(devBT ? "(BT)" : "(USB)")}");
            }
            catch (Exception ex)
            {
                Log($"DS_GetDev(): Run into problem connecting controller: \n{ex.StackTrace}");
                throw new Exception("Couldn't connect controller.\n" + ex.StackTrace);
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
            Log($"DS_ReadData(): Reached.");

            try
            {
                if (dev == null) return;
                Log($"DS_ReadData(): Device is not null.");

                byte[] deviceStates = new byte[devLength];
                dev.Read(deviceStates);
                int tempOffset = devBT ? 1 : 0;

                byte faceButtons = deviceStates[8 + tempOffset];
                Log($"DS_ReadData(): X: {(faceButtons & (1 << 5)) != 0}"); // For debug purposes.


                if (devBT)
                {
                    Log($"DS_ReadData(): STATE: {(BatteryState)((byte)(deviceStates[53 + tempOffset] & 0xF0) >> 4)}");
                    Log($"DS_ReadData(): PERCENT: {Math.Min((int)((deviceStates[53 + tempOffset] & 0x0F) * 10 + 5), 100)}%");

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

                Log(exception.Message);

                OnDisconnect();
                DS_Dispose(devNum);
            }
        }

        private void DS_WriteData(Color col)
        {
            Log("DS_WriteData(): Reached.");

            byte[] outputDevStates = new byte[devLength];

            if (devBT)
            {
                Log($"DS_WriteData(): Device is connected via BT");

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

                Log($"DS_WriteData(): Device is not connected via BT");

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
                Log($"DS_WriteData(): Trying to write to device");

                if (dev != null) dev.WriteAsync(outputDevStates, 0, devLength);
                else
                {
                    Log("DS_WriteData(): Device is null?");
                }
            }
            catch (Exception exception)
            {
                Log($"DS_WriteData(): Failed to write to device");
                if (exception.StackTrace != null) Log(exception.StackTrace);
            }
        }

        private void DS_Dispose(int num)
        {
            if (dev != null)
            {
                Log("DS_Dispose(): Colour reset.");

                DS_WriteData(Color.Blue);

                dev.Dispose();
                Log("DS_Dispose(): Device disposed.");
            };
        }

        private void OnDisconnect()
        {
            Log("OnDisconnect(): Disconnected.");
            dualsenseStarted = false;
            warned = false;
            label1.Text = "";
            updateTimer.Stop();
            connectionTimer.Start();
            notifyIcon1.Icon = null;
            notifyIcon1.Text = "WTFITBA: disconnected";
            icon.Generate("n/a", Properties.Settings.Default.TrayFont, Properties.Settings.Default.TrayColour, Properties.Settings.Default.TrayOffset, notifyIcon1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dualsenseStarted)
            {
                try
                {
                    // Dispose original controller.
                    DS_Dispose(devNum);
                    // Get new controller.
                    DS_GetDev(controllerPlayer);

                    if (dev != null)
                    {
                        dualsenseStarted = true;
                        warned = false;

                        Tick();

                        Log("[info: DS_WriteData() is called twice to send the init packet, so all functionality works]");

                        if (devBTInit == false) DS_WriteData(Color.Blue); // Incase it's not been initialized.
                        DS_WriteData(dualsenseColor);

                        // Restart updateTimer
                        updateTimer.Stop();
                        updateTimer.Start();
                    }
                }
                catch
                {
                    dualsenseStarted = false;
                    warned = false;
                    updateTimer.Stop();

                    numericUpDown1.Value = 0;
                    controllerPlayer = 0;
                    connectionTimer.Start();
                }
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Tick(true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            controllerPlayer = (int)numericUpDown1.Value;
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
                Log($"pictureBox1_Click(): Changed to {colorDialog1.Color}.");

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
            Log("notifyIcon1_MouseDoubleClick(): Showing window.");

            Show();
            WindowState = FormWindowState.Normal;
        }

        private void NumericUpDown2_Enter(object? sender, EventArgs e)
        {
            if (dualsenseStarted) updateTimer.Stop();
        }

        private void NumericUpDown2_Leave(object? sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            if (dualsenseStarted) { updateTimer.Start(); warned = false; }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!valuesLoaded) return;
            warningThreshold = (int)numericUpDown2.Value;

            Properties.Settings.Default.WarningThreshold = (int)numericUpDown2.Value;

            Log($"numericUpDown2_ValueChanged(): Changed to {warningThreshold}.");

        }

        private void NumericUpDown3_Enter(object? sender, EventArgs e)
        {
            if (dualsenseStarted) updateTimer.Stop();
        }

        private void NumericUpDown3_Leave(object? sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            if (dualsenseStarted) updateTimer.Start();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!valuesLoaded) return;
            tickRate = (int)numericUpDown3.Value;

            updateTimer.Interval = tickRate;
            Log($"numericUpDown3_ValueChanged(): Changed to {tickRate}.");

            Properties.Settings.Default.TickRate = (int)numericUpDown3.Value;
        }

        private void debugTimer_Tick(object sender, EventArgs e)
        {
            textBox1.Text = $"Timer1: {updateTimer.Enabled}" +
                $"{Environment.NewLine}" +
                $"Interval: {updateTimer.Interval}" +
                $"{Environment.NewLine}" +
                $"BS: {devBatteryState}" +
                $"{Environment.NewLine}" +
                $"connectionTimer: {connectionTimer.Enabled}" +
                $"{Environment.NewLine}" +
                $"windowUpdateTimer: {windowUpdateTimer.Enabled}";

            if (checkBox2.Checked)
            {
                icon.Generate(new Random().Next(100).ToString(), trayFont, trayColor, trayOffset, notifyIcon1);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!valuesLoaded) return;
            Log($"checkBox1_CheckedChanged(): Changed to {checkBox1.Checked}.");

            Properties.Settings.Default.ShouldSetLight = checkBox1.Checked;
            Properties.Settings.Default.Save();

        }

        private void connectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!dualsenseStarted)
                {
                    Log("connectionTimer_Tick(): Attempting to connect controller.");


                    DS_GetDev(controllerPlayer);
                    if (dev == null) return;

                    Log("connectionTimer_Tick(): Controller found.");

                    dualsenseStarted = true;

                    Tick();

                    Log("[info: DS_WriteData() is called twice to send the init packet, so all functionality works]");
                    if (devBTInit == false) DS_WriteData(Color.Blue); // In case it's not been initialized.
                    DS_WriteData(dualsenseColor);

                    updateTimer.Start();
                    connectionTimer.Stop();
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

        private void windowUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (dualsenseStarted)
            {
                button1.Text = "Reconnect";
                button1.Visible = true;
                label1.Location = new Point(3, 32);

                numericUpDown1.Visible = true;
            }
            else
            {
                button1.Visible = false;
                label1.Text = "Waiting..";
                label1.Location = new Point(3, 3);

                notifyIcon1.Icon = null;
                notifyIcon1.Text = "WTFITBA: disconnected";
                icon.Generate("n/a", Properties.Settings.Default.TrayFont, Properties.Settings.Default.TrayColour, Properties.Settings.Default.TrayOffset, notifyIcon1);

                numericUpDown1.Visible = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process page = new Process();
            page.StartInfo.UseShellExecute = true;
            page.StartInfo.FileName = "https://github.com/wompscode/WTFIsTheBatteryAt";
            page.Start();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (Program.debug)
            {
                MessageBox.Show("You are already running WTFITBA in debug mode. Restart with no arguments to disable it.");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Restart WTFITBA with debug functionality?", "WTFIsTheBatteryAt", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = Application.ExecutablePath;
                process.StartInfo.Arguments = "--debug";
                process.Start();
                Environment.Exit(0);
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!Program.debug) return;
            icon.Generate(numericUpDown4.Value.ToString(), Properties.Settings.Default.TrayFont, Properties.Settings.Default.TrayColour, Properties.Settings.Default.TrayOffset, notifyIcon1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                trayFont = fontDialog1.Font;
                button2.Text = $"{trayFont.Name}, {trayFont.Size}";
                Log($"button2_Click(): Changed to {trayFont.Name}, {trayFont.Size}.");
                Properties.Settings.Default.TrayFont = fontDialog1.Font;
                Properties.Settings.Default.Save();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                trayColor = colorDialog2.Color;
                Log($"pictureBox2_Click(): Changed to {colorDialog2.Color.Name}.");
                pictureBox2.BackColor = trayColor;
                label5.BackColor = trayColor;
                label5.ForeColor = IdealTextColor(trayColor);
                Properties.Settings.Default.TrayColour = colorDialog2.Color;
                Properties.Settings.Default.Save();
            }
        }
        public void SaveAndSetOffset()
        {
            Log($"SaveAndSetOffset(): Changed to {numericUpDown5.Value}, {numericUpDown6.Value}.");
            trayOffset = new Point((int)numericUpDown5.Value, (int)numericUpDown6.Value);
            Properties.Settings.Default.TrayOffset = new Point((int)numericUpDown5.Value, (int)numericUpDown6.Value);
            Properties.Settings.Default.Save();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!valuesLoaded) return;
            SaveAndSetOffset();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!valuesLoaded) return;
            SaveAndSetOffset();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            pictureBox2_Click(sender, e);
        }
    }
}