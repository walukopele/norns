using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Net;
using wyrd;
using verdandi;


namespace skuld
{
    public partial class client_control : UserControl
    {
        //List<client> cli;
        client c;
        //packet manual_unh;

        public bool working {get;private set;}
        public string iP;
        public long passa;
        public long passb;


        public client_control()
        {
            working = false;
            InitializeComponent();
            //this.Init(new client());
            //c = new client();
            
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }


        public void Init(client c) 
        {
            this.c = c;
            c.on_info += C_on_info;
            c.Debug = true;
            c.afterlogin += C_afterlogin;
            c.afterconnect += C_afterconnect;
            working = this.c != null;
        }

        private void C_afterconnect(client c,string what)
        {
            c.SetHandler("server", "checktest", checktest);
        }

        private void checktest(packet p, string format)
        {
            long ret = p.ReadSL();
            C_on_info(c, ret.ToString());
        }

        private void C_on_info(client c, string what)
        {
            string last = what+'\n';
            if (working)
            {
                richTextBox_client_debug.Text += last;
                richTextBox_client_debug.SelectionStart = richTextBox_client_debug.Text.Length;
                richTextBox_client_debug.ScrollToCaret();
                //listBox_unhandled.Items.Clear();
                //listBox_unhandled.Items.AddRange(c.unhandled);
                //listBox_handled.Items.Clear();
                //listBox_handled.Items.AddRange(c.handled);
            }
        }

        private void C_afterlogin(client c, string s)
        {
        }

        public void Close() 
        {
            if (working)
            {
                working = false;               
                c.Close();
            }
        }
        void log_Changed(string last)
        {
            if (working)
            {
                richTextBox_client_debug.Text += last;
                richTextBox_client_debug.SelectionStart = richTextBox_client_debug.Text.Length;
                richTextBox_client_debug.ScrollToCaret();
                comboBox_service.Items.Clear();

                string[] unh = c.unhandled;
                
                //listBox_unhandled.Items.Clear();
                //listBox_unhandled.Items.AddRange(c.unhandled);
                //listBox_handled.Items.Clear();
                //listBox_handled.Items.AddRange(c.handled);
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            c.Connect(comboBox_servers.Text);
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (working)
            {
                c.Close();
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            if (working)
            {
                
                    string[] row = comboBox_pass.Text.Split(' ');
                    if (row.Length > 0)
                        
                                                   

                            if (row.Length > 1)
                                passa = Int64.Parse(row[1]);

                            if (row.Length > 2)
                                passb = Int64.Parse(row[2]);
                        
                
                c.Auth(passa,passb);
            }
        }

        private void button_newUser_Click(object sender, EventArgs e)
        {
            if (working)
            {
                c.SetHandler("server", "new_user", newuser);
                c.Send("server", "new_user");
            }
        }

        private void newuser(packet p, string format_toclient)
        {
            passa = p.ReadSL();
            passb = p.ReadSL();
            string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "accounts");
            File.AppendAllText(path, comboBox_servers.Text + " " + passa.ToString() + " " + passb.ToString() + "\n");

        }

        private void timer_guiupdate_Tick(object sender, EventArgs e)
        {
            if (working)
            {
                label_ppsr.Text = c.pps;
                label_Pingvalue.Text = c.ping;
                label_nstatus.Text = c.acclevel;
            }
        }

        private void listBox_unhandled_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            
        }

        private void print_packet_raw(packet p)
        {
            byte[] ba = p.Bytes;
            StringBuilder hex = new StringBuilder(ba.Length * 3);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
                hex.Append(' ');
            }
            string sm = p.String.Replace('\\', '|');
            richTextBox_client_debug.AppendText("\n[a]:" + " raw:`" + hex.ToString() + "`" + " sm:`" + sm + "`");
            richTextBox_client_debug.AppendText("\n");
            richTextBox_client_debug.SelectionStart = richTextBox_client_debug.Text.Length;
            // scroll it automatically
            richTextBox_client_debug.ScrollToCaret();
        }

        private void listBox_unhandled_DoubleClick(object sender, EventArgs e)
        {
            
        }

        

        private void listBox_handled_SelectedIndexChanged(object sender, EventArgs e)
        {
                     
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox_arguments_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox_send_command_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void listBox_handled_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void textBox_arguments_KeyPress(object sender, KeyPressEventArgs e)
        { 
        }

        private void textBox_arguments_KeyUp(object sender, KeyEventArgs e)
        {
            string srvname = comboBox_service.Text;
            string comname = comboBox_command.Text;
            //int idxh = listBox_handled.SelectedIndex;
            //if (idxh != -1)
            //{
            //    srvname = listBox_handled.SelectedItem.ToString().Split(',')[0];
            //    comname = listBox_handled.SelectedItem.ToString().Split(',')[1];
            //}
            //else
            //{
            //    textBox_command_help.Text = "Please select handled first. Use command args separated by comma. Press 'enter' to send command";
            //    int idx = listBox_unhandled.SelectedIndex;
            //    if (idx != -1)
            //    {
            //        srvname = listBox_unhandled.SelectedItem.ToString().Split(',')[0];
            //        comname = listBox_unhandled.SelectedItem.ToString().Split(',')[1];
            //    }
            //    else textBox_command_help.Text = "Please select unhandled first. Use command args separated by comma. Press 'enter' to send command";
            //}

            packet p;
            bool bad=true;
            textBox_command_help.Text = c.TryParse(srvname,comname, textBox_arguments.Text, out p,out bad);

            if (e.KeyCode == Keys.Return&&!bad)
            {
                c.Send(p);
            }
        }

        private void comboBox_service_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_service_DropDown(object sender, EventArgs e)
        {            
            comboBox_service.Items.Clear();
            comboBox_service.Items.AddRange(c.services);
        }

        private void comboBox_command_DropDown(object sender, EventArgs e)
        {
            string[] names = c.get_commands(comboBox_service.Text);
            Array.Sort(names);
            comboBox_command.Items.Clear();
            comboBox_command.Items.AddRange(names);
        }

        private void comboBox_servers_DropDown(object sender, EventArgs e)
        {
            if (working)
            {
                string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "accounts");

                if (File.Exists(path))
                {

                    string[] data = File.ReadAllText(path).Split('\n');
                    comboBox_servers.Items.Clear();
                    foreach (string rows in data)
                    {
                        string[] row = rows.Split(' ');

                        string address = "127.0.0.1";
                       

                        if (row.Length > 0)
                            if (!comboBox_servers.Items.Contains(row[0]))
                            {
                                address = row[0];
                                comboBox_servers.Items.Add(address);

                                if (row.Length > 1)
                                    passa = Int64.Parse(row[1]);

                                if (row.Length > 2)
                                    passb = Int64.Parse(row[2]);
                            }
                    }

                }
                else
                {
                    File.AppendAllText(path, comboBox_servers.Text + " " + passa.ToString() + " " + passb.ToString() + "\n");
                }
            }

            //string[] files=Directory.GetFiles(Directory.GetCurrentDirectory(),"*.server",SearchOption.TopDirectoryOnly);

            //List<string> names = new List<string>();
            //foreach (string f in files)
            //{
            //    names.Add(Path.GetFileNameWithoutExtension(f));
            //}
            //names.Sort();
            //comboBox_servers.Items.Clear();
            //comboBox_servers.Items.AddRange(names.ToArray());
        }

        private void comboBox_servers_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void groupBox_login_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox_command_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_pass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_pass_DropDown(object sender, EventArgs e)
        {
            if (working)
            {
                string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "accounts");

                if (File.Exists(path))
                {

                    string[] data = File.ReadAllText(path).Split('\n');

                    comboBox_pass.Items.Clear();
                    comboBox_pass.Items.AddRange(data);

                }
            }
        }
    }
}
