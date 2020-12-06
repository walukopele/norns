namespace skuld
{
    partial class client_control
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label_ppsr = new System.Windows.Forms.Label();
            this.label_nstatus = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_newUser = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.groupBox_connect = new System.Windows.Forms.GroupBox();
            this.comboBox_servers = new System.Windows.Forms.ComboBox();
            this.groupBox_traits = new System.Windows.Forms.GroupBox();
            this.label_Pingvalue = new System.Windows.Forms.Label();
            this.label_Ping = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            this.label_ppsn = new System.Windows.Forms.Label();
            this.richTextBox_client_debug = new System.Windows.Forms.RichTextBox();
            this.groupBox_log = new System.Windows.Forms.GroupBox();
            this.timer_guiupdate = new System.Windows.Forms.Timer(this.components);
            this.textBox_arguments = new System.Windows.Forms.TextBox();
            this.groupBox_send_command = new System.Windows.Forms.GroupBox();
            this.panel_help = new System.Windows.Forms.Panel();
            this.panel_help_desc = new System.Windows.Forms.Panel();
            this.textBox_command_help = new System.Windows.Forms.TextBox();
            this.panel_helplabel = new System.Windows.Forms.Panel();
            this.label_command_help_label = new System.Windows.Forms.Label();
            this.panel_log_and_control = new System.Windows.Forms.Panel();
            this.panel_io = new System.Windows.Forms.Panel();
            this.panel_control = new System.Windows.Forms.Panel();
            this.groupBox_login = new System.Windows.Forms.GroupBox();
            this.comboBox_pass = new System.Windows.Forms.ComboBox();
            this.panel_input = new System.Windows.Forms.Panel();
            this.groupBox_selectcom = new System.Windows.Forms.GroupBox();
            this.comboBox_command = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_service = new System.Windows.Forms.ComboBox();
            this.label_service = new System.Windows.Forms.Label();
            this.groupBox_connect.SuspendLayout();
            this.groupBox_traits.SuspendLayout();
            this.groupBox_log.SuspendLayout();
            this.groupBox_send_command.SuspendLayout();
            this.panel_help.SuspendLayout();
            this.panel_help_desc.SuspendLayout();
            this.panel_helplabel.SuspendLayout();
            this.panel_log_and_control.SuspendLayout();
            this.panel_io.SuspendLayout();
            this.panel_control.SuspendLayout();
            this.groupBox_login.SuspendLayout();
            this.panel_input.SuspendLayout();
            this.groupBox_selectcom.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_ppsr
            // 
            this.label_ppsr.AutoSize = true;
            this.label_ppsr.Location = new System.Drawing.Point(34, 16);
            this.label_ppsr.Name = "label_ppsr";
            this.label_ppsr.Size = new System.Drawing.Size(55, 13);
            this.label_ppsr.TabIndex = 61;
            this.label_ppsr.Text = "label_ppsr";
            // 
            // label_nstatus
            // 
            this.label_nstatus.AutoSize = true;
            this.label_nstatus.Location = new System.Drawing.Point(212, 16);
            this.label_nstatus.Name = "label_nstatus";
            this.label_nstatus.Size = new System.Drawing.Size(64, 13);
            this.label_nstatus.TabIndex = 60;
            this.label_nstatus.Text = "connected?";
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(140, 15);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(39, 23);
            this.button_stop.TabIndex = 56;
            this.button_stop.Text = "stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(95, 15);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(39, 23);
            this.button_start.TabIndex = 55;
            this.button_start.Text = "start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_newUser
            // 
            this.button_newUser.Location = new System.Drawing.Point(286, 19);
            this.button_newUser.Name = "button_newUser";
            this.button_newUser.Size = new System.Drawing.Size(42, 23);
            this.button_newUser.TabIndex = 53;
            this.button_newUser.Text = "New";
            this.button_newUser.UseVisualStyleBackColor = true;
            this.button_newUser.Click += new System.EventHandler(this.button_newUser_Click);
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(238, 19);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(42, 23);
            this.button_login.TabIndex = 51;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // groupBox_connect
            // 
            this.groupBox_connect.Controls.Add(this.comboBox_servers);
            this.groupBox_connect.Controls.Add(this.button_start);
            this.groupBox_connect.Controls.Add(this.button_stop);
            this.groupBox_connect.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_connect.Location = new System.Drawing.Point(0, 0);
            this.groupBox_connect.Name = "groupBox_connect";
            this.groupBox_connect.Size = new System.Drawing.Size(192, 48);
            this.groupBox_connect.TabIndex = 62;
            this.groupBox_connect.TabStop = false;
            this.groupBox_connect.Text = "connect";
            // 
            // comboBox_servers
            // 
            this.comboBox_servers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_servers.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_servers.FormattingEnabled = true;
            this.comboBox_servers.Items.AddRange(new object[] {
            "127.0.0.1",
            "192.168.0.10"});
            this.comboBox_servers.Location = new System.Drawing.Point(6, 15);
            this.comboBox_servers.Name = "comboBox_servers";
            this.comboBox_servers.Size = new System.Drawing.Size(83, 21);
            this.comboBox_servers.TabIndex = 55;
            this.comboBox_servers.DropDown += new System.EventHandler(this.comboBox_servers_DropDown);
            this.comboBox_servers.SelectedIndexChanged += new System.EventHandler(this.comboBox_servers_SelectedIndexChanged);
            // 
            // groupBox_traits
            // 
            this.groupBox_traits.Controls.Add(this.label_Pingvalue);
            this.groupBox_traits.Controls.Add(this.label_Ping);
            this.groupBox_traits.Controls.Add(this.label_state);
            this.groupBox_traits.Controls.Add(this.label_ppsn);
            this.groupBox_traits.Controls.Add(this.label_ppsr);
            this.groupBox_traits.Controls.Add(this.label_nstatus);
            this.groupBox_traits.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox_traits.Location = new System.Drawing.Point(721, 0);
            this.groupBox_traits.Name = "groupBox_traits";
            this.groupBox_traits.Size = new System.Drawing.Size(296, 48);
            this.groupBox_traits.TabIndex = 63;
            this.groupBox_traits.TabStop = false;
            this.groupBox_traits.Text = "traits";
            // 
            // label_Pingvalue
            // 
            this.label_Pingvalue.AutoSize = true;
            this.label_Pingvalue.Location = new System.Drawing.Point(129, 16);
            this.label_Pingvalue.Name = "label_Pingvalue";
            this.label_Pingvalue.Size = new System.Drawing.Size(31, 13);
            this.label_Pingvalue.TabIndex = 65;
            this.label_Pingvalue.Text = "0000";
            // 
            // label_Ping
            // 
            this.label_Ping.AutoSize = true;
            this.label_Ping.Location = new System.Drawing.Point(95, 16);
            this.label_Ping.Name = "label_Ping";
            this.label_Ping.Size = new System.Drawing.Size(28, 13);
            this.label_Ping.TabIndex = 64;
            this.label_Ping.Text = "Ping";
            // 
            // label_state
            // 
            this.label_state.AutoSize = true;
            this.label_state.Location = new System.Drawing.Point(166, 16);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(40, 13);
            this.label_state.TabIndex = 63;
            this.label_state.Text = "Rights:";
            // 
            // label_ppsn
            // 
            this.label_ppsn.AutoSize = true;
            this.label_ppsn.Location = new System.Drawing.Point(4, 16);
            this.label_ppsn.Name = "label_ppsn";
            this.label_ppsn.Size = new System.Drawing.Size(31, 13);
            this.label_ppsn.TabIndex = 62;
            this.label_ppsn.Text = "PPS:";
            // 
            // richTextBox_client_debug
            // 
            this.richTextBox_client_debug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox_client_debug.DetectUrls = false;
            this.richTextBox_client_debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_client_debug.Location = new System.Drawing.Point(3, 16);
            this.richTextBox_client_debug.Name = "richTextBox_client_debug";
            this.richTextBox_client_debug.ReadOnly = true;
            this.richTextBox_client_debug.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_client_debug.Size = new System.Drawing.Size(1011, 369);
            this.richTextBox_client_debug.TabIndex = 64;
            this.richTextBox_client_debug.Text = "";
            // 
            // groupBox_log
            // 
            this.groupBox_log.Controls.Add(this.richTextBox_client_debug);
            this.groupBox_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_log.Location = new System.Drawing.Point(0, 48);
            this.groupBox_log.Name = "groupBox_log";
            this.groupBox_log.Size = new System.Drawing.Size(1017, 388);
            this.groupBox_log.TabIndex = 65;
            this.groupBox_log.TabStop = false;
            this.groupBox_log.Text = "log";
            // 
            // timer_guiupdate
            // 
            this.timer_guiupdate.Enabled = true;
            this.timer_guiupdate.Interval = 250;
            this.timer_guiupdate.Tick += new System.EventHandler(this.timer_guiupdate_Tick);
            // 
            // textBox_arguments
            // 
            this.textBox_arguments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_arguments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_arguments.Location = new System.Drawing.Point(3, 39);
            this.textBox_arguments.Name = "textBox_arguments";
            this.textBox_arguments.Size = new System.Drawing.Size(817, 20);
            this.textBox_arguments.TabIndex = 68;
            this.textBox_arguments.TextChanged += new System.EventHandler(this.textBox_arguments_TextChanged);
            this.textBox_arguments.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_arguments_KeyPress);
            this.textBox_arguments.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_arguments_KeyUp);
            // 
            // groupBox_send_command
            // 
            this.groupBox_send_command.Controls.Add(this.panel_help);
            this.groupBox_send_command.Controls.Add(this.textBox_arguments);
            this.groupBox_send_command.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_send_command.Location = new System.Drawing.Point(194, 0);
            this.groupBox_send_command.Name = "groupBox_send_command";
            this.groupBox_send_command.Size = new System.Drawing.Size(823, 62);
            this.groupBox_send_command.TabIndex = 71;
            this.groupBox_send_command.TabStop = false;
            this.groupBox_send_command.Text = "send unhandled command";
            this.groupBox_send_command.TextChanged += new System.EventHandler(this.groupBox_send_command_TextChanged);
            // 
            // panel_help
            // 
            this.panel_help.Controls.Add(this.panel_help_desc);
            this.panel_help.Controls.Add(this.panel_helplabel);
            this.panel_help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_help.Location = new System.Drawing.Point(3, 16);
            this.panel_help.Name = "panel_help";
            this.panel_help.Size = new System.Drawing.Size(817, 23);
            this.panel_help.TabIndex = 72;
            // 
            // panel_help_desc
            // 
            this.panel_help_desc.Controls.Add(this.textBox_command_help);
            this.panel_help_desc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_help_desc.Location = new System.Drawing.Point(40, 0);
            this.panel_help_desc.Name = "panel_help_desc";
            this.panel_help_desc.Size = new System.Drawing.Size(777, 23);
            this.panel_help_desc.TabIndex = 1;
            // 
            // textBox_command_help
            // 
            this.textBox_command_help.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_command_help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_command_help.Location = new System.Drawing.Point(0, 0);
            this.textBox_command_help.Multiline = true;
            this.textBox_command_help.Name = "textBox_command_help";
            this.textBox_command_help.ReadOnly = true;
            this.textBox_command_help.Size = new System.Drawing.Size(777, 23);
            this.textBox_command_help.TabIndex = 71;
            // 
            // panel_helplabel
            // 
            this.panel_helplabel.Controls.Add(this.label_command_help_label);
            this.panel_helplabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_helplabel.Location = new System.Drawing.Point(0, 0);
            this.panel_helplabel.MinimumSize = new System.Drawing.Size(40, 0);
            this.panel_helplabel.Name = "panel_helplabel";
            this.panel_helplabel.Size = new System.Drawing.Size(40, 23);
            this.panel_helplabel.TabIndex = 0;
            // 
            // label_command_help_label
            // 
            this.label_command_help_label.AutoSize = true;
            this.label_command_help_label.Location = new System.Drawing.Point(3, 0);
            this.label_command_help_label.Name = "label_command_help_label";
            this.label_command_help_label.Size = new System.Drawing.Size(32, 13);
            this.label_command_help_label.TabIndex = 70;
            this.label_command_help_label.Text = "Help:";
            // 
            // panel_log_and_control
            // 
            this.panel_log_and_control.Controls.Add(this.panel_io);
            this.panel_log_and_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_log_and_control.Location = new System.Drawing.Point(0, 0);
            this.panel_log_and_control.Name = "panel_log_and_control";
            this.panel_log_and_control.Size = new System.Drawing.Size(1017, 498);
            this.panel_log_and_control.TabIndex = 73;
            // 
            // panel_io
            // 
            this.panel_io.Controls.Add(this.groupBox_log);
            this.panel_io.Controls.Add(this.panel_control);
            this.panel_io.Controls.Add(this.panel_input);
            this.panel_io.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_io.Location = new System.Drawing.Point(0, 0);
            this.panel_io.Name = "panel_io";
            this.panel_io.Size = new System.Drawing.Size(1017, 498);
            this.panel_io.TabIndex = 74;
            // 
            // panel_control
            // 
            this.panel_control.Controls.Add(this.groupBox_login);
            this.panel_control.Controls.Add(this.groupBox_connect);
            this.panel_control.Controls.Add(this.groupBox_traits);
            this.panel_control.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_control.Location = new System.Drawing.Point(0, 0);
            this.panel_control.Name = "panel_control";
            this.panel_control.Size = new System.Drawing.Size(1017, 48);
            this.panel_control.TabIndex = 73;
            // 
            // groupBox_login
            // 
            this.groupBox_login.Controls.Add(this.comboBox_pass);
            this.groupBox_login.Controls.Add(this.button_login);
            this.groupBox_login.Controls.Add(this.button_newUser);
            this.groupBox_login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_login.Location = new System.Drawing.Point(192, 0);
            this.groupBox_login.Name = "groupBox_login";
            this.groupBox_login.Size = new System.Drawing.Size(529, 48);
            this.groupBox_login.TabIndex = 66;
            this.groupBox_login.TabStop = false;
            this.groupBox_login.Text = "login";
            this.groupBox_login.Enter += new System.EventHandler(this.groupBox_login_Enter);
            // 
            // comboBox_pass
            // 
            this.comboBox_pass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_pass.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_pass.FormattingEnabled = true;
            this.comboBox_pass.Items.AddRange(new object[] {
            "127.0.0.1",
            "192.168.0.10"});
            this.comboBox_pass.Location = new System.Drawing.Point(6, 19);
            this.comboBox_pass.Name = "comboBox_pass";
            this.comboBox_pass.Size = new System.Drawing.Size(226, 21);
            this.comboBox_pass.TabIndex = 57;
            this.comboBox_pass.DropDown += new System.EventHandler(this.comboBox_pass_DropDown);
            this.comboBox_pass.SelectedIndexChanged += new System.EventHandler(this.comboBox_pass_SelectedIndexChanged);
            // 
            // panel_input
            // 
            this.panel_input.Controls.Add(this.groupBox_send_command);
            this.panel_input.Controls.Add(this.groupBox_selectcom);
            this.panel_input.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_input.Location = new System.Drawing.Point(0, 436);
            this.panel_input.Name = "panel_input";
            this.panel_input.Size = new System.Drawing.Size(1017, 62);
            this.panel_input.TabIndex = 72;
            // 
            // groupBox_selectcom
            // 
            this.groupBox_selectcom.Controls.Add(this.comboBox_command);
            this.groupBox_selectcom.Controls.Add(this.label2);
            this.groupBox_selectcom.Controls.Add(this.comboBox_service);
            this.groupBox_selectcom.Controls.Add(this.label_service);
            this.groupBox_selectcom.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_selectcom.Location = new System.Drawing.Point(0, 0);
            this.groupBox_selectcom.MaximumSize = new System.Drawing.Size(194, 64);
            this.groupBox_selectcom.MinimumSize = new System.Drawing.Size(194, 64);
            this.groupBox_selectcom.Name = "groupBox_selectcom";
            this.groupBox_selectcom.Size = new System.Drawing.Size(194, 64);
            this.groupBox_selectcom.TabIndex = 72;
            this.groupBox_selectcom.TabStop = false;
            this.groupBox_selectcom.Text = "select command";
            // 
            // comboBox_command
            // 
            this.comboBox_command.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_command.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_command.FormattingEnabled = true;
            this.comboBox_command.Location = new System.Drawing.Point(67, 35);
            this.comboBox_command.Name = "comboBox_command";
            this.comboBox_command.Size = new System.Drawing.Size(121, 21);
            this.comboBox_command.TabIndex = 1;
            this.comboBox_command.DropDown += new System.EventHandler(this.comboBox_command_DropDown);
            this.comboBox_command.SelectedIndexChanged += new System.EventHandler(this.comboBox_command_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "command";
            // 
            // comboBox_service
            // 
            this.comboBox_service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_service.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_service.FormattingEnabled = true;
            this.comboBox_service.Location = new System.Drawing.Point(67, 13);
            this.comboBox_service.Name = "comboBox_service";
            this.comboBox_service.Size = new System.Drawing.Size(121, 21);
            this.comboBox_service.TabIndex = 0;
            this.comboBox_service.DropDown += new System.EventHandler(this.comboBox_service_DropDown);
            this.comboBox_service.SelectedIndexChanged += new System.EventHandler(this.comboBox_service_SelectedIndexChanged);
            // 
            // label_service
            // 
            this.label_service.AutoSize = true;
            this.label_service.Location = new System.Drawing.Point(6, 16);
            this.label_service.Name = "label_service";
            this.label_service.Size = new System.Drawing.Size(41, 13);
            this.label_service.TabIndex = 2;
            this.label_service.Text = "service";
            // 
            // client_control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_log_and_control);
            this.Name = "client_control";
            this.Size = new System.Drawing.Size(1017, 498);
            this.groupBox_connect.ResumeLayout(false);
            this.groupBox_traits.ResumeLayout(false);
            this.groupBox_traits.PerformLayout();
            this.groupBox_log.ResumeLayout(false);
            this.groupBox_send_command.ResumeLayout(false);
            this.groupBox_send_command.PerformLayout();
            this.panel_help.ResumeLayout(false);
            this.panel_help_desc.ResumeLayout(false);
            this.panel_help_desc.PerformLayout();
            this.panel_helplabel.ResumeLayout(false);
            this.panel_helplabel.PerformLayout();
            this.panel_log_and_control.ResumeLayout(false);
            this.panel_io.ResumeLayout(false);
            this.panel_control.ResumeLayout(false);
            this.groupBox_login.ResumeLayout(false);
            this.panel_input.ResumeLayout(false);
            this.groupBox_selectcom.ResumeLayout(false);
            this.groupBox_selectcom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_ppsr;
        private System.Windows.Forms.Label label_nstatus;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_newUser;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.GroupBox groupBox_connect;
        private System.Windows.Forms.GroupBox groupBox_traits;
        private System.Windows.Forms.RichTextBox richTextBox_client_debug;
        private System.Windows.Forms.GroupBox groupBox_log;
        private System.Windows.Forms.Timer timer_guiupdate;
        private System.Windows.Forms.TextBox textBox_arguments;
        private System.Windows.Forms.GroupBox groupBox_send_command;
        private System.Windows.Forms.Label label_command_help_label;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Label label_ppsn;
        private System.Windows.Forms.TextBox textBox_command_help;
        private System.Windows.Forms.Label label_Pingvalue;
        private System.Windows.Forms.Label label_Ping;
        private System.Windows.Forms.Panel panel_log_and_control;
        private System.Windows.Forms.Panel panel_io;
        private System.Windows.Forms.Panel panel_help;
        private System.Windows.Forms.Panel panel_help_desc;
        private System.Windows.Forms.Panel panel_helplabel;
        private System.Windows.Forms.Panel panel_input;
        private System.Windows.Forms.GroupBox groupBox_selectcom;
        private System.Windows.Forms.ComboBox comboBox_command;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_service;
        private System.Windows.Forms.Label label_service;
        private System.Windows.Forms.GroupBox groupBox_login;
        private System.Windows.Forms.Panel panel_control;
        private System.Windows.Forms.ComboBox comboBox_servers;
        private System.Windows.Forms.ComboBox comboBox_pass;
    }
}
