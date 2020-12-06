using System;
using System.Collections.Generic;

using System.Windows.Forms;
//using System.Data.SQLite;
using System.Net;
//using skuld.Common;
using verdandi;

//using wyrd;

using System.Net.NetworkInformation;
using System.Net.Sockets;
using skuld;
using System.IO;

namespace Gui
{
    public partial class GUI : Form
    {
        
        server urd;
        public GUI()
        {
            InitializeComponent();
            urd = new server();         
        }     


        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox_server_ip.Text = urd.IP;
            maskedTextBox_server_port.Text = urd.Port;

            List<IPAddress> ipList = new List<IPAddress>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine("found ip " + ua.Address.ToString());
                        ipList.Add(ua.Address);
                        listBox_myipaddresses.Items.Add(ua.Address.ToString());
                    }
                }
            }
            string path = Path.Combine(Environment.CurrentDirectory, "log");
            foreach (string file in Directory.EnumerateFiles(path))
            {
                listBox_logs.Items.Add(file);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            urd.SetIPPort(textBox_server_ip.Text,urd.Port);            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            listBox_known_Workers.Items.Clear();
            listBox_known_Workers.Items.AddRange(urd.KnownWorkers);
        }

        private void listBox_registeredservices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox_activeservices.SelectedIndex;
            if (i == -1) return;

            service s = urd.Collective[i];
            textBox_name.Text = s.ServiceName;
            listBox_known_Workers.Text = s.WorkerType;          
        }

        private void button_register_new_service_Click(object sender, EventArgs e)
        {
            serviceinfo man =
                    new serviceinfo(
                        textBox_name.Text,
                        listBox_known_Workers.Text,
                        "",
                        false
                        );

            urd.register(man);
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {            
                listBox_known_Workers.Items.Clear();
                listBox_known_Workers.Items.AddRange(urd.KnownWorkers);
                listBox_activeservices.Items.Clear();
                listBox_activeservices.Items.AddRange(urd.ActiveServices);
                listBox_sessions2.Items.Clear();
            listBox_sessions2.Items.Clear();
            foreach (session s in urd.Sessions)
            {
                listBox_sessions2.Items.Add(s.connection_uid);
            }
            comboBox_accountPrivileges.Items.Clear();
            comboBox_accountPrivileges.Items.AddRange(Enum.GetNames(typeof(verdandi.privilege)));
        }

        private void button_service_delete_Click(object sender, EventArgs e)
        {
            urd.unregister(listBox_activeservices.Text);
            listBox_activeservices.Items.Clear();
            listBox_activeservices.Items.AddRange(urd.ActiveServices);
        }

        private void listBox_myipaddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_server_ip.Text = (string)listBox_myipaddresses.SelectedItem;
        }
        
        private void button_view_worker_data_Click(object sender, EventArgs e)
        {
            if (listBox_activeservices.SelectedIndex != -1) 
            {
                richTextBox_Cache_serialized.Text=
                urd.Collective[listBox_activeservices.SelectedIndex].subordinate.cache.ToString();
            }
        }        

        private void GUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            urd.stop();
            urd.close();
            client_control1.Close();
        } 

        private void maskedTextBox_server_port_Validated(object sender, EventArgs e)
        {
            urd.SetIPPort(urd.IP, maskedTextBox_server_port.Text);
        }

        private void tabPage_client_Enter(object sender, EventArgs e)
        {
            if(!client_control1.working)
            client_control1.Init(new wyrd.client());
        }       

        private void timer_update_gui_Tick(object sender, EventArgs e)
        {
            if (urd != null)
            {
                textBoxClients.Text = urd.ClientsCount.ToString();
                textBox_average.Text = (urd.PPS / (float)(urd.ClientsCount)).ToString();
                textBox_ppsw.Text = urd.PPS.ToString();
                textBox_bw.Text = urd.WPS;
            }
        }

        private void listBox_logs_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox_log.Text = File.ReadAllText(listBox_logs.Text);
        }

        private void listBox_sessions2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox_sessions2.SelectedIndex;
            if (idx == -1) return;

            session s = urd.Sessions.Find(x => x.connection_uid == (ushort)listBox_sessions2.SelectedItem);
            if (s == null) return;

            textBox_accountid.Text = s.session_account.uid.ToString();
            textBox_accountname.Text = s.session_account.name;
            comboBox_accountPrivileges.SelectedIndex = s.session_account.accesslevel;


        }

        private void textBox_accountname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_accountname_DoubleClick(object sender, EventArgs e)
        {
            textBox_accountname.Enabled = true;
        }

        private void textBox_accountname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n')
            {
                int idx = listBox_sessions2.SelectedIndex;
                if (idx == -1) return;

                session s = urd.Sessions.Find(x => x.connection_uid == (ushort)listBox_sessions2.SelectedItem);
                if (s == null) return;

                s.session_account.name = textBox_accountname.Text;

                textBox_accountname.Enabled = false;
            }
        }

        private void comboBox_accountPrivileges_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox_sessions2.SelectedIndex;
            if (idx == -1) return;

            session s = urd.Sessions.Find(x => x.connection_uid == (ushort)listBox_sessions2.SelectedItem);
            if (s == null) return;

            s.session_account.accesslevel = (sbyte)comboBox_accountPrivileges.SelectedIndex;
        }
    }
}
