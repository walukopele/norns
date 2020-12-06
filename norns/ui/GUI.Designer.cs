using skuld;

namespace Gui
{
    partial class GUI
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabPage_service = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.richTextBox_Cache_serialized = new System.Windows.Forms.RichTextBox();
            this.groupBox_knownsvc = new System.Windows.Forms.GroupBox();
            this.listBox_known_Workers = new System.Windows.Forms.ListBox();
            this.groupBox_activesvc = new System.Windows.Forms.GroupBox();
            this.listBox_activeservices = new System.Windows.Forms.ListBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.listBox_sessions2 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_accountid = new System.Windows.Forms.TextBox();
            this.label_accountid = new System.Windows.Forms.Label();
            this.comboBox_accountPrivileges = new System.Windows.Forms.ComboBox();
            this.textBox_accountname = new System.Windows.Forms.TextBox();
            this.label_accountPrivileges = new System.Windows.Forms.Label();
            this.label_accountName = new System.Windows.Forms.Label();
            this.button_view_worker_data = new System.Windows.Forms.Button();
            this.button_service_delete = new System.Windows.Forms.Button();
            this.button_register_new_service = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.localservices_refresh_button = new System.Windows.Forms.Button();
            this.tabPage_intro = new System.Windows.Forms.TabPage();
            this.panel_log = new System.Windows.Forms.Panel();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox_logs = new System.Windows.Forms.GroupBox();
            this.listBox_logs = new System.Windows.Forms.ListBox();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.label_bw = new System.Windows.Forms.Label();
            this.textBox_bw = new System.Windows.Forms.TextBox();
            this.listBox_myipaddresses = new System.Windows.Forms.ListBox();
            this.label_ppsw = new System.Windows.Forms.Label();
            this.textBox_ppsw = new System.Windows.Forms.TextBox();
            this.labelClients = new System.Windows.Forms.Label();
            this.label_average = new System.Windows.Forms.Label();
            this.textBoxClients = new System.Windows.Forms.TextBox();
            this.textBox_average = new System.Windows.Forms.TextBox();
            this.maskedTextBox_server_port = new System.Windows.Forms.MaskedTextBox();
            this.label_adress = new System.Windows.Forms.Label();
            this.label_port = new System.Windows.Forms.Label();
            this.textBox_server_ip = new System.Windows.Forms.TextBox();
            this.tabControl_management = new System.Windows.Forms.TabControl();
            this.tabPage_client = new System.Windows.Forms.TabPage();
            this.client_control1 = new skuld.client_control();
            this.timer_update_gui = new System.Windows.Forms.Timer(this.components);
            this.tabPage_service.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox_knownsvc.SuspendLayout();
            this.groupBox_activesvc.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_intro.SuspendLayout();
            this.panel_log.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox_logs.SuspendLayout();
            this.groupBoxServer.SuspendLayout();
            this.tabControl_management.SuspendLayout();
            this.tabPage_client.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage_service
            // 
            this.tabPage_service.Controls.Add(this.groupBox8);
            this.tabPage_service.Controls.Add(this.localservices_refresh_button);
            this.tabPage_service.Location = new System.Drawing.Point(4, 22);
            this.tabPage_service.Name = "tabPage_service";
            this.tabPage_service.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_service.Size = new System.Drawing.Size(903, 422);
            this.tabPage_service.TabIndex = 6;
            this.tabPage_service.Text = "service";
            this.tabPage_service.UseVisualStyleBackColor = true;
            this.tabPage_service.Enter += new System.EventHandler(this.tabPage6_Enter);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.richTextBox_Cache_serialized);
            this.groupBox8.Controls.Add(this.groupBox_knownsvc);
            this.groupBox8.Controls.Add(this.groupBox_activesvc);
            this.groupBox8.Controls.Add(this.groupBox9);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(897, 416);
            this.groupBox8.TabIndex = 48;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Менеджеры";
            // 
            // richTextBox_Cache_serialized
            // 
            this.richTextBox_Cache_serialized.Location = new System.Drawing.Point(212, 212);
            this.richTextBox_Cache_serialized.Name = "richTextBox_Cache_serialized";
            this.richTextBox_Cache_serialized.Size = new System.Drawing.Size(608, 197);
            this.richTextBox_Cache_serialized.TabIndex = 55;
            this.richTextBox_Cache_serialized.Text = "";
            // 
            // groupBox_knownsvc
            // 
            this.groupBox_knownsvc.Controls.Add(this.listBox_known_Workers);
            this.groupBox_knownsvc.Location = new System.Drawing.Point(6, 19);
            this.groupBox_knownsvc.Name = "groupBox_knownsvc";
            this.groupBox_knownsvc.Size = new System.Drawing.Size(197, 171);
            this.groupBox_knownsvc.TabIndex = 53;
            this.groupBox_knownsvc.TabStop = false;
            this.groupBox_knownsvc.Text = "Known Svc";
            // 
            // listBox_known_Workers
            // 
            this.listBox_known_Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_known_Workers.FormattingEnabled = true;
            this.listBox_known_Workers.Location = new System.Drawing.Point(3, 16);
            this.listBox_known_Workers.Name = "listBox_known_Workers";
            this.listBox_known_Workers.Size = new System.Drawing.Size(191, 152);
            this.listBox_known_Workers.TabIndex = 6;
            // 
            // groupBox_activesvc
            // 
            this.groupBox_activesvc.Controls.Add(this.listBox_activeservices);
            this.groupBox_activesvc.Location = new System.Drawing.Point(3, 196);
            this.groupBox_activesvc.Name = "groupBox_activesvc";
            this.groupBox_activesvc.Size = new System.Drawing.Size(200, 216);
            this.groupBox_activesvc.TabIndex = 54;
            this.groupBox_activesvc.TabStop = false;
            this.groupBox_activesvc.Text = "Active Svc";
            // 
            // listBox_activeservices
            // 
            this.listBox_activeservices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_activeservices.FormattingEnabled = true;
            this.listBox_activeservices.Location = new System.Drawing.Point(3, 16);
            this.listBox_activeservices.Name = "listBox_activeservices";
            this.listBox_activeservices.Size = new System.Drawing.Size(194, 197);
            this.listBox_activeservices.TabIndex = 42;
            this.listBox_activeservices.SelectedIndexChanged += new System.EventHandler(this.listBox_registeredservices_SelectedIndexChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.listBox_sessions2);
            this.groupBox9.Controls.Add(this.groupBox1);
            this.groupBox9.Controls.Add(this.button_view_worker_data);
            this.groupBox9.Controls.Add(this.button_service_delete);
            this.groupBox9.Controls.Add(this.button_register_new_service);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Controls.Add(this.textBox_name);
            this.groupBox9.Location = new System.Drawing.Point(212, 19);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(617, 171);
            this.groupBox9.TabIndex = 50;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "serviceinfo";
            // 
            // listBox_sessions2
            // 
            this.listBox_sessions2.FormattingEnabled = true;
            this.listBox_sessions2.Location = new System.Drawing.Point(263, 16);
            this.listBox_sessions2.Name = "listBox_sessions2";
            this.listBox_sessions2.Size = new System.Drawing.Size(78, 147);
            this.listBox_sessions2.TabIndex = 77;
            this.listBox_sessions2.SelectedIndexChanged += new System.EventHandler(this.listBox_sessions2_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_accountid);
            this.groupBox1.Controls.Add(this.label_accountid);
            this.groupBox1.Controls.Add(this.comboBox_accountPrivileges);
            this.groupBox1.Controls.Add(this.textBox_accountname);
            this.groupBox1.Controls.Add(this.label_accountPrivileges);
            this.groupBox1.Controls.Add(this.label_accountName);
            this.groupBox1.Location = new System.Drawing.Point(347, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 147);
            this.groupBox1.TabIndex = 76;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textBox_accountid
            // 
            this.textBox_accountid.Enabled = false;
            this.textBox_accountid.Location = new System.Drawing.Point(67, 47);
            this.textBox_accountid.Name = "textBox_accountid";
            this.textBox_accountid.Size = new System.Drawing.Size(118, 20);
            this.textBox_accountid.TabIndex = 5;
            // 
            // label_accountid
            // 
            this.label_accountid.AutoSize = true;
            this.label_accountid.Location = new System.Drawing.Point(7, 50);
            this.label_accountid.Name = "label_accountid";
            this.label_accountid.Size = new System.Drawing.Size(54, 13);
            this.label_accountid.TabIndex = 4;
            this.label_accountid.Text = "accountid";
            // 
            // comboBox_accountPrivileges
            // 
            this.comboBox_accountPrivileges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_accountPrivileges.FormattingEnabled = true;
            this.comboBox_accountPrivileges.Location = new System.Drawing.Point(64, 73);
            this.comboBox_accountPrivileges.Name = "comboBox_accountPrivileges";
            this.comboBox_accountPrivileges.Size = new System.Drawing.Size(121, 21);
            this.comboBox_accountPrivileges.TabIndex = 3;
            this.comboBox_accountPrivileges.SelectedIndexChanged += new System.EventHandler(this.comboBox_accountPrivileges_SelectedIndexChanged);
            // 
            // textBox_accountname
            // 
            this.textBox_accountname.Enabled = false;
            this.textBox_accountname.Location = new System.Drawing.Point(85, 19);
            this.textBox_accountname.Name = "textBox_accountname";
            this.textBox_accountname.Size = new System.Drawing.Size(100, 20);
            this.textBox_accountname.TabIndex = 2;
            this.textBox_accountname.TextChanged += new System.EventHandler(this.textBox_accountname_TextChanged);
            this.textBox_accountname.DoubleClick += new System.EventHandler(this.textBox_accountname_DoubleClick);
            this.textBox_accountname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_accountname_KeyPress);
            // 
            // label_accountPrivileges
            // 
            this.label_accountPrivileges.AutoSize = true;
            this.label_accountPrivileges.Location = new System.Drawing.Point(7, 76);
            this.label_accountPrivileges.Name = "label_accountPrivileges";
            this.label_accountPrivileges.Size = new System.Drawing.Size(51, 13);
            this.label_accountPrivileges.TabIndex = 1;
            this.label_accountPrivileges.Text = "privileges";
            // 
            // label_accountName
            // 
            this.label_accountName.AutoSize = true;
            this.label_accountName.Location = new System.Drawing.Point(7, 22);
            this.label_accountName.Name = "label_accountName";
            this.label_accountName.Size = new System.Drawing.Size(72, 13);
            this.label_accountName.TabIndex = 0;
            this.label_accountName.Text = "accountname";
            // 
            // button_view_worker_data
            // 
            this.button_view_worker_data.Location = new System.Drawing.Point(9, 140);
            this.button_view_worker_data.Name = "button_view_worker_data";
            this.button_view_worker_data.Size = new System.Drawing.Size(118, 23);
            this.button_view_worker_data.TabIndex = 74;
            this.button_view_worker_data.Text = "ViewWorkerData";
            this.button_view_worker_data.UseVisualStyleBackColor = true;
            this.button_view_worker_data.Click += new System.EventHandler(this.button_view_worker_data_Click);
            // 
            // button_service_delete
            // 
            this.button_service_delete.Location = new System.Drawing.Point(101, 51);
            this.button_service_delete.Name = "button_service_delete";
            this.button_service_delete.Size = new System.Drawing.Size(75, 23);
            this.button_service_delete.TabIndex = 69;
            this.button_service_delete.Text = "Delete service";
            this.button_service_delete.UseVisualStyleBackColor = true;
            this.button_service_delete.Click += new System.EventHandler(this.button_service_delete_Click);
            // 
            // button_register_new_service
            // 
            this.button_register_new_service.Location = new System.Drawing.Point(9, 51);
            this.button_register_new_service.Name = "button_register_new_service";
            this.button_register_new_service.Size = new System.Drawing.Size(75, 23);
            this.button_register_new_service.TabIndex = 68;
            this.button_register_new_service.Text = "Add New";
            this.button_register_new_service.UseVisualStyleBackColor = true;
            this.button_register_new_service.Click += new System.EventHandler(this.button_register_new_service_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 21);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 13);
            this.label20.TabIndex = 60;
            this.label20.Text = "Name";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(56, 18);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(120, 20);
            this.textBox_name.TabIndex = 59;
            // 
            // localservices_refresh_button
            // 
            this.localservices_refresh_button.Location = new System.Drawing.Point(239, 82);
            this.localservices_refresh_button.Name = "localservices_refresh_button";
            this.localservices_refresh_button.Size = new System.Drawing.Size(75, 23);
            this.localservices_refresh_button.TabIndex = 47;
            this.localservices_refresh_button.Text = "Refresh";
            this.localservices_refresh_button.UseVisualStyleBackColor = true;
            // 
            // tabPage_intro
            // 
            this.tabPage_intro.Controls.Add(this.panel_log);
            this.tabPage_intro.Controls.Add(this.panel5);
            this.tabPage_intro.Location = new System.Drawing.Point(4, 22);
            this.tabPage_intro.Name = "tabPage_intro";
            this.tabPage_intro.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_intro.Size = new System.Drawing.Size(903, 422);
            this.tabPage_intro.TabIndex = 0;
            this.tabPage_intro.Text = "Сервер";
            this.tabPage_intro.UseVisualStyleBackColor = true;
            // 
            // panel_log
            // 
            this.panel_log.Controls.Add(this.richTextBox_log);
            this.panel_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_log.Location = new System.Drawing.Point(203, 3);
            this.panel_log.Name = "panel_log";
            this.panel_log.Size = new System.Drawing.Size(697, 416);
            this.panel_log.TabIndex = 49;
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_log.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox_log.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.ReadOnly = true;
            this.richTextBox_log.Size = new System.Drawing.Size(697, 416);
            this.richTextBox_log.TabIndex = 6;
            this.richTextBox_log.Text = "";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox_logs);
            this.panel5.Controls.Add(this.groupBoxServer);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(200, 416);
            this.panel5.TabIndex = 48;
            // 
            // groupBox_logs
            // 
            this.groupBox_logs.Controls.Add(this.listBox_logs);
            this.groupBox_logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_logs.Location = new System.Drawing.Point(0, 208);
            this.groupBox_logs.Name = "groupBox_logs";
            this.groupBox_logs.Size = new System.Drawing.Size(200, 208);
            this.groupBox_logs.TabIndex = 48;
            this.groupBox_logs.TabStop = false;
            this.groupBox_logs.Text = "Logs";
            // 
            // listBox_logs
            // 
            this.listBox_logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_logs.FormattingEnabled = true;
            this.listBox_logs.Location = new System.Drawing.Point(3, 16);
            this.listBox_logs.Name = "listBox_logs";
            this.listBox_logs.Size = new System.Drawing.Size(194, 189);
            this.listBox_logs.TabIndex = 47;
            this.listBox_logs.SelectedIndexChanged += new System.EventHandler(this.listBox_logs_SelectedIndexChanged);
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.label_bw);
            this.groupBoxServer.Controls.Add(this.textBox_bw);
            this.groupBoxServer.Controls.Add(this.listBox_myipaddresses);
            this.groupBoxServer.Controls.Add(this.label_ppsw);
            this.groupBoxServer.Controls.Add(this.textBox_ppsw);
            this.groupBoxServer.Controls.Add(this.labelClients);
            this.groupBoxServer.Controls.Add(this.label_average);
            this.groupBoxServer.Controls.Add(this.textBoxClients);
            this.groupBoxServer.Controls.Add(this.textBox_average);
            this.groupBoxServer.Controls.Add(this.maskedTextBox_server_port);
            this.groupBoxServer.Controls.Add(this.label_adress);
            this.groupBoxServer.Controls.Add(this.label_port);
            this.groupBoxServer.Controls.Add(this.textBox_server_ip);
            this.groupBoxServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxServer.Location = new System.Drawing.Point(0, 0);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(200, 208);
            this.groupBoxServer.TabIndex = 22;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Server";
            // 
            // label_bw
            // 
            this.label_bw.AutoSize = true;
            this.label_bw.Location = new System.Drawing.Point(8, 138);
            this.label_bw.Name = "label_bw";
            this.label_bw.Size = new System.Drawing.Size(61, 13);
            this.label_bw.TabIndex = 47;
            this.label_bw.Text = "Bytes Write";
            // 
            // textBox_bw
            // 
            this.textBox_bw.Location = new System.Drawing.Point(80, 135);
            this.textBox_bw.Name = "textBox_bw";
            this.textBox_bw.ReadOnly = true;
            this.textBox_bw.Size = new System.Drawing.Size(100, 20);
            this.textBox_bw.TabIndex = 46;
            // 
            // listBox_myipaddresses
            // 
            this.listBox_myipaddresses.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox_myipaddresses.FormattingEnabled = true;
            this.listBox_myipaddresses.Location = new System.Drawing.Point(3, 162);
            this.listBox_myipaddresses.Name = "listBox_myipaddresses";
            this.listBox_myipaddresses.Size = new System.Drawing.Size(194, 43);
            this.listBox_myipaddresses.TabIndex = 45;
            this.listBox_myipaddresses.SelectedIndexChanged += new System.EventHandler(this.listBox_myipaddresses_SelectedIndexChanged);
            // 
            // label_ppsw
            // 
            this.label_ppsw.AutoSize = true;
            this.label_ppsw.Location = new System.Drawing.Point(7, 62);
            this.label_ppsw.Name = "label_ppsw";
            this.label_ppsw.Size = new System.Drawing.Size(39, 13);
            this.label_ppsw.TabIndex = 9;
            this.label_ppsw.Text = "PPSW";
            // 
            // textBox_ppsw
            // 
            this.textBox_ppsw.Location = new System.Drawing.Point(80, 59);
            this.textBox_ppsw.Name = "textBox_ppsw";
            this.textBox_ppsw.ReadOnly = true;
            this.textBox_ppsw.Size = new System.Drawing.Size(100, 20);
            this.textBox_ppsw.TabIndex = 8;
            // 
            // labelClients
            // 
            this.labelClients.AutoSize = true;
            this.labelClients.Location = new System.Drawing.Point(8, 112);
            this.labelClients.Name = "labelClients";
            this.labelClients.Size = new System.Drawing.Size(63, 13);
            this.labelClients.TabIndex = 7;
            this.labelClients.Text = "Clients(A/C)";
            // 
            // label_average
            // 
            this.label_average.AutoSize = true;
            this.label_average.Location = new System.Drawing.Point(6, 88);
            this.label_average.Name = "label_average";
            this.label_average.Size = new System.Drawing.Size(47, 13);
            this.label_average.TabIndex = 6;
            this.label_average.Text = "Average";
            // 
            // textBoxClients
            // 
            this.textBoxClients.Location = new System.Drawing.Point(80, 109);
            this.textBoxClients.Name = "textBoxClients";
            this.textBoxClients.ReadOnly = true;
            this.textBoxClients.Size = new System.Drawing.Size(100, 20);
            this.textBoxClients.TabIndex = 5;
            // 
            // textBox_average
            // 
            this.textBox_average.Location = new System.Drawing.Point(80, 85);
            this.textBox_average.Name = "textBox_average";
            this.textBox_average.ReadOnly = true;
            this.textBox_average.Size = new System.Drawing.Size(100, 20);
            this.textBox_average.TabIndex = 4;
            // 
            // maskedTextBox_server_port
            // 
            this.maskedTextBox_server_port.Location = new System.Drawing.Point(80, 36);
            this.maskedTextBox_server_port.Mask = "00000";
            this.maskedTextBox_server_port.Name = "maskedTextBox_server_port";
            this.maskedTextBox_server_port.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox_server_port.TabIndex = 3;
            this.maskedTextBox_server_port.Validated += new System.EventHandler(this.maskedTextBox_server_port_Validated);
            // 
            // label_adress
            // 
            this.label_adress.AutoSize = true;
            this.label_adress.Location = new System.Drawing.Point(6, 16);
            this.label_adress.Name = "label_adress";
            this.label_adress.Size = new System.Drawing.Size(39, 13);
            this.label_adress.TabIndex = 0;
            this.label_adress.Text = "Adress";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(6, 39);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(26, 13);
            this.label_port.TabIndex = 1;
            this.label_port.Text = "Port";
            // 
            // textBox_server_ip
            // 
            this.textBox_server_ip.Location = new System.Drawing.Point(80, 13);
            this.textBox_server_ip.Name = "textBox_server_ip";
            this.textBox_server_ip.Size = new System.Drawing.Size(100, 20);
            this.textBox_server_ip.TabIndex = 2;
            this.textBox_server_ip.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabControl_management
            // 
            this.tabControl_management.Controls.Add(this.tabPage_intro);
            this.tabControl_management.Controls.Add(this.tabPage_service);
            this.tabControl_management.Controls.Add(this.tabPage_client);
            this.tabControl_management.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_management.Location = new System.Drawing.Point(0, 0);
            this.tabControl_management.Name = "tabControl_management";
            this.tabControl_management.SelectedIndex = 0;
            this.tabControl_management.Size = new System.Drawing.Size(911, 448);
            this.tabControl_management.TabIndex = 5;
            // 
            // tabPage_client
            // 
            this.tabPage_client.Controls.Add(this.client_control1);
            this.tabPage_client.Location = new System.Drawing.Point(4, 22);
            this.tabPage_client.Name = "tabPage_client";
            this.tabPage_client.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_client.Size = new System.Drawing.Size(903, 422);
            this.tabPage_client.TabIndex = 8;
            this.tabPage_client.Text = "Client";
            this.tabPage_client.UseVisualStyleBackColor = true;
            this.tabPage_client.Enter += new System.EventHandler(this.tabPage_client_Enter);
            // 
            // client_control1
            // 
            this.client_control1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.client_control1.Location = new System.Drawing.Point(3, 3);
            this.client_control1.Name = "client_control1";
            this.client_control1.Size = new System.Drawing.Size(897, 416);
            this.client_control1.TabIndex = 0;
            // 
            // timer_update_gui
            // 
            this.timer_update_gui.Enabled = true;
            this.timer_update_gui.Interval = 333;
            this.timer_update_gui.Tick += new System.EventHandler(this.timer_update_gui_Tick);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 448);
            this.Controls.Add(this.tabControl_management);
            this.Name = "GUI";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUI_FormClosed);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabPage_service.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox_knownsvc.ResumeLayout(false);
            this.groupBox_activesvc.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_intro.ResumeLayout(false);
            this.panel_log.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox_logs.ResumeLayout(false);
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.tabControl_management.ResumeLayout(false);
            this.tabPage_client.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage_service;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox_knownsvc;
        private System.Windows.Forms.ListBox listBox_known_Workers;
        private System.Windows.Forms.GroupBox groupBox_activesvc;
        private System.Windows.Forms.ListBox listBox_activeservices;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button button_service_delete;
        private System.Windows.Forms.Button button_register_new_service;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button localservices_refresh_button;
        private System.Windows.Forms.TabPage tabPage_intro;
        private System.Windows.Forms.Panel panel_log;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Label label_adress;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox textBox_server_ip;
        private System.Windows.Forms.TabControl tabControl_management;
        private System.Windows.Forms.TabPage tabPage_client;
        private System.Windows.Forms.ListBox listBox_myipaddresses;
        private System.Windows.Forms.ListBox listBox_logs;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_server_port;
        private System.Windows.Forms.RichTextBox richTextBox_Cache_serialized;
        private System.Windows.Forms.Button button_view_worker_data;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_sessions2;
        private System.Windows.Forms.Label label_accountName;
        private client_control client_control1;
        private System.Windows.Forms.Label labelClients;
        private System.Windows.Forms.Label label_average;
        private System.Windows.Forms.TextBox textBoxClients;
        private System.Windows.Forms.TextBox textBox_average;
        private System.Windows.Forms.Timer timer_update_gui;
        private System.Windows.Forms.Label label_ppsw;
        private System.Windows.Forms.TextBox textBox_ppsw;
        private System.Windows.Forms.GroupBox groupBox_logs;
        private System.Windows.Forms.Label label_accountPrivileges;
        private System.Windows.Forms.Label label_bw;
        private System.Windows.Forms.TextBox textBox_bw;
        private System.Windows.Forms.TextBox textBox_accountid;
        private System.Windows.Forms.Label label_accountid;
        private System.Windows.Forms.ComboBox comboBox_accountPrivileges;
        private System.Windows.Forms.TextBox textBox_accountname;
    }
}

