//Copyright(c) 2018 walukopele@gmail.com

//Данная лицензия разрешает лицам, получившим копию данного программного обеспечения и
//    сопутствующей документации(в дальнейшем именуемыми «Программное Обеспечение»), 
//безвозмездно использовать Программное Обеспечение без ограничений, включая неограниченное
//    право на использование, копирование, изменение, слияние, публикацию, распространение,
//    сублицензирование и/или продажу копий Программного Обеспечения, а также лицам, которым
//    предоставляется данное Программное Обеспечение, при соблюдении следующих условий:

//Указанное выше уведомление об авторском праве и данные условия должны быть включены во все
//    копии или значимые части данного Программного Обеспечения.

//ДАННОЕ ПРОГРАММНОЕ ОБЕСПЕЧЕНИЕ ПРЕДОСТАВЛЯЕТСЯ «КАК ЕСТЬ», БЕЗ КАКИХ-ЛИБО ГАРАНТИЙ, 
//    ЯВНО ВЫРАЖЕННЫХ ИЛИ ПОДРАЗУМЕВАЕМЫХ, ВКЛЮЧАЯ ГАРАНТИИ ТОВАРНОЙ ПРИГОДНОСТИ, 
//    СООТВЕТСТВИЯ ПО ЕГО КОНКРЕТНОМУ НАЗНАЧЕНИЮ И ОТСУТСТВИЯ НАРУШЕНИЙ, 
//    НО НЕ ОГРАНИЧИВАЯСЬ ИМИ.НИ В КАКОМ СЛУЧАЕ АВТОРЫ ИЛИ ПРАВООБЛАДАТЕЛИ НЕ НЕСУТ
//        ОТВЕТСТВЕННОСТИ ПО КАКИМ-ЛИБО ИСКАМ, ЗА УЩЕРБ ИЛИ ПО ИНЫМ ТРЕБОВАНИЯМ, В ТОМ ЧИСЛЕ, 
//        ПРИ ДЕЙСТВИИ КОНТРАКТА, ДЕЛИКТЕ ИЛИ ИНОЙ СИТУАЦИИ, ВОЗНИКШИМ ИЗ-ЗА ИСПОЛЬЗОВАНИЯ 
//        ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ ИЛИ ИНЫХ ДЕЙСТВИЙ С ПРОГРАММНЫМ ОБЕСПЕЧЕНИЕМ. 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using verdandi;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace wyrd
{
    public class client
    {
        public const ushort STANDARD_PORT = 18888;
        #region events
        public delegate void CE(client c, string what);
        public event CE on_info;
        public event CE afterlogin;
        /// <summary>
        /// better to register custom handlers here in runtime
        /// </summary>
        public event CE afterconnect;
        #endregion

        client_network network;

        commands allhandlers = new commands("");
       
        public bool logged { get; private set; }

        private long est;
        private int alevel = 0;
        private string ip = "";


        private int servicecount = byte.MaxValue - 1;
        private int servicecounter = 0;

        public client()
        {
                      
        }

        #region stats
        public string[] handled
        {
            get { return allhandlers.Handled; }
        }
        public string[] unhandled
        {
            get { return allhandlers.UnHandled; }
        }

        public string[] services { get { return allhandlers.Services; } }
        //public string[] that_services(string servicetype, string serviceversion)
        // { return allhandlers.that_services(servicetype, serviceversion); }

        //public string[] types { get { return allhandlers.Types; } }
        public string[] get_commands(string servicename)
        {
            return allhandlers.Commands(servicename);
        }
        //public string description(string servicename,string commandname)
        //{
        //    return allhandlers.Get_Remote(servicename, commandname).description;
        //}

        public string pps
        {
            get
            {
                string ret = "";

                if (network != null)
                    if (network.Connected)
                        if (network.current != null)
                            return network.pps.ToString();
                        else return "null";
                    else return "disc";
                return ret;
            }
        }
        public string ping
        {
            get
            {
                TimeSpan ts = new TimeSpan(est);
                return ts.Milliseconds.ToString();
            }
        }
        public string acclevel
        {
            get
            {
                if (alevel < 0) return "unknown";
                else return Enum.GetNames(typeof(privilege))[alevel];
            }
        }
        #endregion
        #region controls
        public void Connect(string ipaddress, ushort port = client.STANDARD_PORT)
        {
            ip = ipaddress;

            on_info += Client_on_info;

            network = new client_network();
            network.AddHandler(ex_On_Received);
            network.on_info += Network_on_info;
            network.ondisconnected = oncrush;

            logged = false;
            populate_commands();
          
            network.Connect(ipaddress, port);

        }
        void oncrush(remoteinfo r)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, DateTime.UtcNow.Ticks.ToString() + ".crush"), r.lasterror);
        }
        public void Close()
        {
            network?.Disconnect();
        }
        public void Send(string servicename, string commandname, params object[] obs)
        {
            if (network != null)
                if (network.Connected)
                {
                    commands.command cmd = allhandlers.Get_Remote(servicename, commandname);
                    if (cmd.remote_command == 255 && cmd.target == 255)
                    {
                        on_info(this, "'" + servicename + "/" + commandname + "' not linked");
                    }
                    else
                    {
                        if (cmd.linked)
                        {
                            packet p = new packet(cmd.target, cmd.remote_command);
                            foreach (object o in obs)
                            {
                                if (o.GetType() == typeof(float)) p.Write((float)o);
                                if (o.GetType() == typeof(double)) p.Write((double)o);
                                if (o.GetType() == typeof(byte)) p.Write((byte)o);
                                if (o.GetType() == typeof(sbyte)) p.Write((sbyte)o);
                                if (o.GetType() == typeof(int)) p.Write((int)o);
                                if (o.GetType() == typeof(uint)) p.Write((uint)o);
                                if (o.GetType() == typeof(long)) p.Write((long)o);
                                if (o.GetType() == typeof(ulong)) p.Write((ulong)o);
                            }
                            Send(p);
                        }
                        else
                        {
                            on_info(this, "'" + servicename + "','" + commandname + "' trying exec unlinked command");
                        }
                    }
                }
                else
                {
                    on_info(this, "can`t send...");
                }
        }
        public void Send(packet raw)
        {
            if (network != null)
                if (network.Connected)
                {
                    network.Send(raw);
                }
                else
                {
                    on_info(this, "can`t send...");
                }
        }
        public string TryParse(string servicename, string commandname, string commaseparatedargs, out packet p, out bool bad)
        {
            commands.command cmd = allhandlers.Get_Remote(servicename, commandname);

            string ret = "";
            string toserverformat = cmd.formattoserver;
            int desirednumargs = toserverformat.Length;
            string[] args = commaseparatedargs.Split(',');
            int actualnumargs = args.Length;

            string str = "Server need " + desirednumargs.ToString() + " have " + actualnumargs.ToString() + ". Args are: ";

            packet manual_unh = new packet(cmd.target, cmd.remote_command);

            bool badarg = false;

            for (int i = 0; i < desirednumargs; i++)
            {
                if (i < actualnumargs && i < desirednumargs)
                {
                    string end = "";
                    if (i < actualnumargs - 1 && i < desirednumargs - 1)
                    {
                        end = ", ";
                    }
                    else end = ".";

                    switch (toserverformat[i])
                    {
                        case 'B':
                            sbyte testsb;
                            if (sbyte.TryParse(args[i], out testsb))
                            {
                                manual_unh.Write(testsb);
                                str += "'" + testsb.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error signed byte here" + end;
                                badarg = true;
                            }
                            break;
                        case 'b':
                            byte testub;
                            if (byte.TryParse(args[i], out testub))
                            {
                                manual_unh.Write(testub);
                                str += "'" + testub.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error unsigned byte here" + end;
                                badarg = true;
                            }
                            break;
                        case 'S':
                            short testss;
                            if (short.TryParse(args[i], out testss))
                            {
                                manual_unh.Write(testss);
                                str += "'" + testss.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error signed short here" + end;
                                badarg = true;
                            }
                            break;
                        case 's':
                            ushort testus;
                            if (ushort.TryParse(args[i], out testus))
                            {
                                manual_unh.Write(testus);
                                str += "'" + testus.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error unsigned short here" + end;
                                badarg = true;
                            }
                            break;
                        case 'I':
                            int testsi;
                            if (int.TryParse(args[i], out testsi))
                            {
                                manual_unh.Write(testsi);
                                str += "'" + testsi.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error signed integer here" + end;
                                badarg = true;
                            }
                            break;
                        case 'i':
                            uint testui;
                            if (uint.TryParse(args[i], out testui))
                            {
                                manual_unh.Write(testui);
                                str += "'" + testui.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error unsigned integer here" + end;
                                badarg = true;
                            }
                            break;
                        case 'L':
                            long testsl;
                            if (long.TryParse(args[i], out testsl))
                            {
                                manual_unh.Write(testsl);
                                str += "'" + testsl.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error signed long here" + end;
                                badarg = true;
                            }
                            break;
                        case 'l':
                            ulong testul;
                            if (ulong.TryParse(args[i], out testul))
                            {
                                manual_unh.Write(testul);
                                str += "'" + testul.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error unsigned long here" + end;
                                badarg = true;
                            }
                            break;
                        case 'D':
                            double testd;
                            if (double.TryParse(args[i], out testd))
                            {
                                manual_unh.Write(testd);
                                str += "'" + testd.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error double here" + end;
                                badarg = true;
                            }
                            break;
                        case 'F':
                            float testf;
                            if (float.TryParse(args[i], out testf))
                            {
                                manual_unh.Write(testf);
                                str += "'" + testf.ToString() + "'" + end;
                                badarg = false;
                            }
                            else
                            {
                                str += " error float here" + end;
                                badarg = true;
                            }
                            break;
                        //case 'U':
                        //    manual_unh.Write(args[i]);
                        //    str += "utf16 string" + end;
                        //    badarg = false;
                        //    break;
                        case 'u':
                            manual_unh.Write(args[i]);
                            str += "utf8 string" + end;
                            badarg = false;
                            break;
                        case 'V':
                            str += "void" + end;
                            badarg = false;
                            break;
                        case 'N':
                            str += "nothing" + end;
                            badarg = false;
                            break;
                        default:
                            str += args[i] + end;
                            badarg = true;
                            break;
                    }
                }
            }
            ret = str;
            p = manual_unh;
            bad = badarg || cmd.remote_command == 255 && cmd.target == 255;
            return ret;
        }
        public void SetHandler(string servicename, string commandname, commands.ClientWork w)
        {
            allhandlers.RegisterClientReaction(commandname, servicename, w);
            on_info(this, servicename + "/" + commandname + " registered...");
        }
        #endregion
        #region accounting
        //public void ReAuth()
        //{

        //}
        public void Auth(long passa, long passb)
        {
            //auth by password+rsapub

            //server creates anonymous account with name anonimous

            //later client impersonates with name and email if he wants

            //email used to register new rsa and recover account

            //if identity not exist use random

            //MD5Cng md5 = new MD5Cng();

            //byte[] hash =
            //md5.ComputeHash(Encoding.Unicode.GetBytes(pass));

            //string authinfo = server_id.encrypt_auth_info(id.publickeyinfo);

            //string authinfo = id.self_publickeyinfo;

            //byte[] cryptohash = server_id.encrypt_auth_info(hash);

            // pass_a = BitConverter.ToInt64(hash, 0);
            // pass_b = BitConverter.ToInt64(hash, 7);



            ////
            //short authphase_credentials = 1;

            //if it NOT exists on server, server creates AES key and save RSApub and AES as client credentials with name=Name;

            ///upd: if server know pubid it test pass if not it tests rigts to create new
            ///
            //
            //


            this.Send("server", "auth", passa, passb);
        }

        public enum impersonateinfo : byte
        {
            name = 1,
            email = 2,
            companyname = 3
        }
        public void Impersonate(impersonateinfo mode, string info)
        {
            if (info.Length < 64)
                Send("server", "impersonate", (byte)mode, info);
        }
        //public void RequestRestoreAccount(string email)
        //{

        //}
        //public void ConfirmRestoreAccount(string email)
        //{ }


        #endregion
        #region utilites

        private void ex_On_Received(remoteinfo c)
        {
            packet[] g = c.Get;         

            foreach (var p in g)
            {
                if (p.remote_command == packet.error)
                {                    
                    on_info(this, "[" + allhandlers.Get_Target(p.target) + " error]:" + p.String);                    
                }
                else
                {
                    //breakpoint
                    allhandlers.ClientDo(p);
                }
            }
        }
        private void Network_on_info(string what)
        {
            on_info(this, what);
        }
        private void Client_on_info(client c, string what)
        {

        }
        private void populate_commands()
        {
            allhandlers = new commands("");
            allhandlers.unknownWork = unknown;
            allhandlers.RegisterClientReaction("bootstrap", "server",0 , 0, bootstrap_handler);

            SetHandler("server", "broadcast", broadcast_handler);
            SetHandler("server", "start", start_server);
            SetHandler("server", "stop", stop_server);
            SetHandler("server", "ping", ping_handler);
            SetHandler("server", "auth", auth_handler);
            SetHandler("server", "serviceinfo", serviceinfo_handler);
            SetHandler("server", "impersonate", impersonate_handler);
            SetHandler("server", "status", server_stats_handler);          
            SetHandler("server", "id", check_id);
            SetHandler("server", "new_user", new_user);
        }

        private void unknown(packet p, string format_toclient)
        {
            on_info(this, BitConverter.ToString(p.Bytes));
        }

        private void new_user(packet p, string format_toclient)
        {
            //pass_a = p.ReadSL();
            //pass_b = p.ReadSL();            
        }
        
        #endregion
        #region predefined handlers
        private void check_id(packet p, string format)
        {
            string serverpub = p.String;
        }
        private void broadcast_handler(packet p, string format)
        {
            on_info(this, p.String);
        }
        private void impersonate_handler(packet p, string format)
        {
            byte result = p.ReadUB();
            on_info(this, result.ToString());
        }
        private void ping_handler(packet p, string format)
        {
            long currenttime = DateTime.UtcNow.Ticks;
            long traveltime = p.ReadSL();
            alevel = p.ReadSB();
            est = currenttime - traveltime;            
            Send("server", "ping",currenttime);            
        }
        private void debug_handler(packet p, string format)
        {
            string ret = "(" + format + "):";

            foreach (var item in format)
            {
                switch (item)
                {
                    case 'm':
                        ret += "[status message code:" + p.ReadSI().ToString() + "]";
                        break;
                    case 'B':
                        ret += "[" + p.ReadSB().ToString() + "]";
                        break;
                    case 'b':
                        ret += "[" + p.ReadUB().ToString() + "]";
                        break;
                    case 'S':
                        ret += "[" + p.ReadSS().ToString() + "]";
                        break;
                    case 's':
                        ret += "[" + p.ReadUS().ToString() + "]";
                        break;
                    case 'I':
                        ret += "[" + p.ReadSI().ToString() + "]";
                        break;
                    case 'i':
                        ret += "[" + p.ReadUI().ToString() + "]";
                        break;
                    case 'L':
                        ret += "[" + p.ReadSL().ToString() + "]";
                        break;
                    case 'l':
                        ret += "[" + p.ReadUL().ToString() + "]";
                        break;
                    case 'D':
                        ret += "[" + p.ReadD().ToString() + "]";
                        break;
                    case 'F':
                        ret += "[" + p.ReadF().ToString() + "]";
                        break;
                    case 'u':
                        ret += "[" + p.String + "]";
                        break;
                    case 'V':
                        ret += "[Ok!]";
                        break;
                    case 'N':
                        ret += "[und]";
                        break;
                    default:
                        ret += "[raw:]" + BitConverter.ToString(p.Bytes);
                        break;
                }
            }
            on_info(this, ret);           
        }
        private void start_server(packet p, string format)
        {
            populate_commands();
            Send("server", "bootstrap");
        }
        private void stop_server(packet p, string format)
        {
            
        }
        private void bootstrap_handler(packet p, string format)
        {
            if (p.String != "")
            {
                servicecounter++;

                int num = allhandlers.Link(p.String);
                string tname = allhandlers.Get_Target(p.target);
                on_info(this, "[" + tname + "]: " + num.ToString() + " handlers linked");

                num = allhandlers.Set_Unhandled(debug_handler, p.target);
                on_info(this, "[" + tname + "]: " + num.ToString() + " handlers left unlinked");

                if (p.target == 0)
                {
                    servicecount = byte.MaxValue;
                    servicecounter = 0;

                    Send("server", "serviceinfo");                   
                }

                if (servicecount == servicecounter)//all services bootstrapped;
                {
                    on_info(this, "[all services bootstrapped and ready]");
                    Send("server", "ping");                  
                    afterconnect?.Invoke(this, "ok");
                }
            }
        }
        private void serviceinfo_handler(packet p, string format)
        {
            //string clean = p.String.Replace('\n', ' ');
            string[] raw = p.String.Split(';');

            string[] topmanansver = Array.FindAll(raw, x => x != "" && x != "\n");

            servicecount = topmanansver.Length;// +server

            if (servicecount == 0)
            {
                on_info(this, "[only one service exists nothing to bootstrap]");
                Send("server", "ping");
                afterconnect?.Invoke(this, "");
            }

            on_info(this, "[setup remote]");

            foreach (string test in topmanansver)
            {
                string a = test.Trim();

                if (a != "")
                {
                    string[] aa = a.Split(',');
                    string name = aa[0];
                    byte internalport = byte.Parse(aa[1]);

                    on_info(this, "[remote]: " + name);
                    allhandlers.RegisterClientReaction("bootstrap", name, internalport, 0, bootstrap_handler);
                    Send(name, "bootstrap");
                }
            }
        }
        private void auth_handler(packet p, string format)
        {
            short authstage = p.ReadSS();

            //short authphase_fail = 0;
            short ok = 1;
            //short notexists = 2;
            //short authphase_done = 3;


            if (authstage == ok)
            {
                on_info(this, "Logged...");
                logged = true;
                afterlogin?.Invoke(this, "");
            }
            else
            {
                on_info(this, "Not logged...");
            }
        }
        private void server_stats_handler(packet p, string format)
        {
            int pps = p.ReadSI();
            int ccount = p.ReadSI();
            int maxc = p.ReadSI();
            string writed = p.String;

            on_info(this,
                "client count is " + ccount.ToString() + " " +
                "out of max " + maxc.ToString() + " allowed. " +
                "PPS is " + pps.ToString() + ". " +
                "Average is " + ((float)pps / (float)ccount).ToString() + " " +
                "Write bandwith is " + writed);          
        }
        #endregion
    }
}
