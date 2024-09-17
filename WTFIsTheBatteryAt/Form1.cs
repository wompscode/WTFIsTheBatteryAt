namespace WTFIsTheBatteryAt
{
    using Wujek_Dualsense_API;
    public partial class Form1 : Form
    {
        public static int controllerPlayer = 0;
        public static bool dualsenseStarted = false;

        public static Color dualsenseColor = Properties.Settings.Default.LightbarColour;
        public static int warningThreshold = Properties.Settings.Default.WarningThreshold;
        public static int tickRate = Properties.Settings.Default.TickRate;

        public static Dualsense dualsense;

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
            try
            {
                dualsense = new Dualsense(controllerPlayer);
            }
            catch { }

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
            if (dualsense != null) dualsense.SetLightbar(0, 0, 255);
            if (dualsense != null) dualsense.Dispose();
            closing = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = dualsenseColor;
            
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
            if (dualsense == null) return;

            if (dualsense.Battery.Level < warningThreshold && warned == false && warning == true)
            {
                ShowNotification($"Battery below {warningThreshold}%", 2);

                warned = true;
            }

            label1.Text = $"Battery: {dualsense.Battery.Level}% [{(dualsense.Battery.State == BatteryState.State.POWER_SUPPLY_STATUS_CHARGING ? "charging" : "discharging")}]";
            notifyIcon1.Text = $"WTFITBA: {dualsense.Battery.Level}% [{(dualsense.Battery.State == BatteryState.State.POWER_SUPPLY_STATUS_CHARGING ? "c" : "d")}]";
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
                if (dualsense != null)
                {
                    dualsense.ResetSettings();
                    dualsense.Dispose();
                }
                return;
            }
            try
            {
                dualsense = new Dualsense(controllerPlayer);
            }
            catch 
            {
                ShowNotification($"Failed to find Dualsense at player {controllerPlayer}. Is there one connected?", 3);    
            }

            if (dualsense != null)
            {

                dualsense.Start();
                dualsense.SetLightbar(dualsenseColor.R, dualsenseColor.G, dualsenseColor.B);

                dualsense.Connection.ControllerDisconnected += Connection_ControllerDisconnected;

                button1.Text = "Disconnect";

                dualsenseStarted = true;
                warned = false;

                Thread.Sleep(10);
                Tick();

                timer1.Start();
            }
        }

        private void ShowNotification(string text, int iconType = 0)
        {
            switch(iconType)
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

        private void Connection_ControllerDisconnected(object? sender, ConnectionStatus.Controller e)
        {
            if(!closing) button1.Text = "Connect";
            dualsenseStarted = false;
            warned = false;

            ShowNotification($"Dualsense disconnected.", 1);

            if (!closing) notifyIcon1.Text = "WTFITBA: disconnected";

            if (!closing) label1.Text = "";
            if (!closing) timer1.Stop();

            if (dualsense != null)
            {
                dualsense.ResetSettings();
                dualsense.Dispose();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dualsense != null) Tick(true);
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
                label2.BackColor = colorDialog1.Color;
                label2.ForeColor = IdealTextColor(colorDialog1.Color);

                dualsenseColor = colorDialog1.Color;

                Properties.Settings.Default.LightbarColour = colorDialog1.Color;

                if (dualsense != null) dualsense.SetLightbar(dualsenseColor.R, dualsenseColor.G, dualsenseColor.B);
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

            if (dualsenseStarted) timer1.Start();
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
            label5.Text = $"Timer1: {timer1.Enabled}";
        }
    }
}