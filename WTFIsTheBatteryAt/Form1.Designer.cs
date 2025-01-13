namespace WTFIsTheBatteryAt
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            label1 = new Label();
            updateTimer = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            colorDialog1 = new ColorDialog();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            linkLabel1 = new LinkLabel();
            tabPage2 = new TabPage();
            groupBox1 = new GroupBox();
            label8 = new Label();
            label5 = new Label();
            label7 = new Label();
            pictureBox2 = new PictureBox();
            numericUpDown6 = new NumericUpDown();
            button2 = new Button();
            label6 = new Label();
            numericUpDown5 = new NumericUpDown();
            checkBox1 = new CheckBox();
            label4 = new Label();
            numericUpDown3 = new NumericUpDown();
            label3 = new Label();
            numericUpDown2 = new NumericUpDown();
            debugTimer = new System.Windows.Forms.Timer(components);
            connectionTimer = new System.Windows.Forms.Timer(components);
            windowUpdateTimer = new System.Windows.Forms.Timer(components);
            textBox1 = new TextBox();
            numericUpDown4 = new NumericUpDown();
            fontDialog1 = new FontDialog();
            colorDialog2 = new ColorDialog();
            checkBox2 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.InterceptArrowKeys = false;
            numericUpDown1.Location = new Point(6, 6);
            numericUpDown1.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(39, 23);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // button1
            // 
            button1.Location = new Point(51, 6);
            button1.Name = "button1";
            button1.Size = new Size(103, 23);
            button1.TabIndex = 1;
            button1.Text = "Reconnect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 32);
            label1.Name = "label1";
            label1.Size = new Size(151, 15);
            label1.TabIndex = 2;
            label1.Text = "Battery: 000% [discharging]";
            // 
            // updateTimer
            // 
            updateTimer.Interval = 12000;
            updateTimer.Tick += updateTimer_Tick;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "WTFITBA: disconnected";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(6, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(151, 50);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 10);
            label2.Name = "label2";
            label2.Size = new Size(90, 15);
            label2.TabIndex = 4;
            label2.Text = "Lightbar Colour";
            label2.Click += label2_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(338, 210);
            tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(linkLabel1);
            tabPage1.Controls.Add(numericUpDown1);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(330, 182);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Main";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(277, 161);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(45, 15);
            linkLabel1.TabIndex = 11;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "GitHub";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(checkBox1);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(numericUpDown3);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(numericUpDown2);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(pictureBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(330, 182);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Settings";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(pictureBox2);
            groupBox1.Controls.Add(numericUpDown6);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(numericUpDown5);
            groupBox1.Location = new Point(168, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(156, 149);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tray Icon Settings";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 91);
            label8.Name = "label8";
            label8.Size = new Size(63, 15);
            label8.TabIndex = 19;
            label8.Text = "Text Offset";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 24);
            label5.Name = "label5";
            label5.Size = new Size(67, 15);
            label5.TabIndex = 12;
            label5.Text = "Tray Colour";
            label5.Click += label5_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(76, 111);
            label7.Name = "label7";
            label7.Size = new Size(13, 15);
            label7.TabIndex = 18;
            label7.Text = "y";
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.BackColor = Color.Black;
            pictureBox2.Location = new Point(17, 20);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(126, 39);
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // numericUpDown6
            // 
            numericUpDown6.Location = new Point(89, 109);
            numericUpDown6.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            numericUpDown6.Minimum = new decimal(new int[] { 16, 0, 0, int.MinValue });
            numericUpDown6.Name = "numericUpDown6";
            numericUpDown6.Size = new Size(37, 23);
            numericUpDown6.TabIndex = 17;
            numericUpDown6.ValueChanged += numericUpDown6_ValueChanged;
            // 
            // button2
            // 
            button2.Location = new Point(17, 65);
            button2.Name = "button2";
            button2.Size = new Size(109, 23);
            button2.TabIndex = 13;
            button2.Text = "Set Tray Font";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 111);
            label6.Name = "label6";
            label6.Size = new Size(13, 15);
            label6.TabIndex = 16;
            label6.Text = "x";
            // 
            // numericUpDown5
            // 
            numericUpDown5.Location = new Point(33, 109);
            numericUpDown5.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            numericUpDown5.Minimum = new decimal(new int[] { 16, 0, 0, int.MinValue });
            numericUpDown5.Name = "numericUpDown5";
            numericUpDown5.Size = new Size(37, 23);
            numericUpDown5.TabIndex = 14;
            numericUpDown5.ValueChanged += numericUpDown5_ValueChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(11, 37);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(15, 14);
            checkBox1.TabIndex = 9;
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 107);
            label4.Name = "label4";
            label4.Size = new Size(101, 15);
            label4.TabIndex = 8;
            label4.Text = "Update Rate (ms):";
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(6, 125);
            numericUpDown3.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(120, 23);
            numericUpDown3.TabIndex = 7;
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 60);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 6;
            label3.Text = "Warning %:";
            label3.Click += label3_Click;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(6, 78);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 5;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // debugTimer
            // 
            debugTimer.Interval = 250;
            debugTimer.Tick += debugTimer_Tick;
            // 
            // connectionTimer
            // 
            connectionTimer.Enabled = true;
            connectionTimer.Interval = 2500;
            connectionTimer.Tick += connectionTimer_Tick;
            // 
            // windowUpdateTimer
            // 
            windowUpdateTimer.Enabled = true;
            windowUpdateTimer.Tick += windowUpdateTimer_Tick;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(338, 24);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(134, 155);
            textBox1.TabIndex = 6;
            // 
            // numericUpDown4
            // 
            numericUpDown4.Location = new Point(4, 216);
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(120, 23);
            numericUpDown4.TabIndex = 7;
            numericUpDown4.ValueChanged += numericUpDown4_ValueChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(130, 225);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(15, 14);
            checkBox2.TabIndex = 20;
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(338, 209);
            Controls.Add(checkBox2);
            Controls.Add(numericUpDown4);
            Controls.Add(textBox1);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(500, 300);
            MinimumSize = new Size(354, 248);
            Name = "Form1";
            Text = "WTFIsTheBatteryAt 0.0.0";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private Button button1;
        private Label label1;
        private System.Windows.Forms.Timer updateTimer;
        private NotifyIcon notifyIcon1;
        private ColorDialog colorDialog1;
        private PictureBox pictureBox1;
        private Label label2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label3;
        private NumericUpDown numericUpDown2;
        private Label label4;
        private NumericUpDown numericUpDown3;
        private System.Windows.Forms.Timer debugTimer;
        private CheckBox checkBox1;
        private System.Windows.Forms.Timer connectionTimer;
        private System.Windows.Forms.Timer windowUpdateTimer;
        private TextBox textBox1;
        private NumericUpDown numericUpDown4;
        private LinkLabel linkLabel1;
        private Button button2;
        private Label label5;
        private PictureBox pictureBox2;
        private FontDialog fontDialog1;
        private ColorDialog colorDialog2;
        private Label label7;
        private NumericUpDown numericUpDown6;
        private Label label6;
        private NumericUpDown numericUpDown5;
        private GroupBox groupBox1;
        private Label label8;
        private CheckBox checkBox2;
    }
}
