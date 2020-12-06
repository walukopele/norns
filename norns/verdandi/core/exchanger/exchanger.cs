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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace verdandi
{
    /// <summary>
    /// core networking
    /// </summary>
    public class exchanger
    {
        private Socket self;
        private remoteinfo[] actives;
        private Queue<remoteinfo> newbies = new Queue<remoteinfo>();
        private bool this_is_listener = false;
        private readonly int maxremotes;

        #region threads
        private Thread t_clean;
        private Thread t_accept;
        private Thread t_write;

        private readonly object locker_active = new object();

        private bool ioworking = false;
        #endregion

        public exchanger(int maxremotes = 500)
        {
            this.maxremotes = maxremotes;
            On_Info += dummyl;
            actives = new remoteinfo[maxremotes];           
        }

        #region events
        public delegate void ExCallback(remoteinfo c);

        public ExCallback Received;
        public ExCallback Connected;

        public delegate void LogInfo(string what);
        public LogInfo On_Info;
        private void dummyl(string d) { return; }
        #endregion

        #region control
        public void Listen(IPAddress ip, int port)
        {
            Close();
            this_is_listener = true;

            self = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            self.Bind(new IPEndPoint(ip, port));
            self.Listen(250);

            startio(true);
        }
        public void Connect(string name, IPAddress ip, int port, cryptor identity = null)
        {

            if (!this_is_listener)
            {
                startio(false);

                Socket
                self1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                remoteinfo ci = new remoteinfo(ref self1, name);

                try
                {
                    ci.connect(ip, port, add_connection, identity);
                }
                catch(Exception e) { On_Info("Connection failed: "+e.Message); }
            }
        }
        public void Close()
        {
            ioworking = false;

            lock (locker_active)
                foreach (var item in actives)
                {
                    if (item != null)
                    {
                        item.close();
                    }
                }

            if (self != null)
            {
                self.Close();
            }

            this_is_listener = false;
        }
        #endregion

        #region utilites
        public string[] Remotes
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (var item in actives)
                {
                    if (item != null)
                        ret.Add(item.remotename);
                }
                return ret.ToArray();
            }
        }
        //public remoteinfo this[string remoteservername]
        //{
        //    get
        //    {
        //        return Array.Find(actives, x => x != null && x.remotename == remoteservername);
        //    }
        //}
        //public remoteinfo this[int idx]
        //{
        //    get
        //    {
        //        return actives[idx];
        //    }
        //}
        public int CountConnected
        {
            get
            {
                int ret = 0;

                foreach (var item in actives)
                {
                    if (item != null)
                    {
                        ret++;
                    }
                }
                return ret;
            }
        }
        public int PPS
        {
            get
            {
                int ret = 0;

                foreach (var item in actives)
                {
                    if (item != null)
                    {
                        ret = ret + item.pps;
                    }
                }
                return ret;
            }
        }
        public string BytesWrite
        {
            get
            {
                int ret = 0;

                foreach (var item in actives)
                {
                    if (item != null)
                    {
                        ret = ret + item.bytesWritePerSec;
                    }
                }
                if (ret < 1024) return ret.ToString() + " B";
                if (ret > 1024 && ret < 1048576) return (ret / 1024).ToString() + " KB";
                if (ret > 1048576) return (ret / 1048576).ToString() + " MB";

                return "";
            }
        }
        #endregion

        #region IO
        private void startio(bool is_server)
        {
            if (!ioworking)
            {
                if (is_server)
                {
                    string name = "server";

                    ioworking = true;

                    t_clean = new Thread(clean_ing);
                    t_clean.Name = name + " ex CL th";
                    t_clean.IsBackground = true;

                    t_accept = new Thread(accept_ing);
                    t_accept.Name = name + " ex AC th";
                    t_accept.IsBackground = true;

                    t_write = new Thread(writeing);

                    t_write.Name = name + " ex W th";
                    t_write.Start();

                    t_clean.Start();
                    t_accept.Start();
                }
                else
                {
                    ioworking = true;

                    t_write = new Thread(writeing);

                    t_write.Name = "client ex W th";
                    t_write.Start();
                }
            }
        }

        private void writeing()
        {
            while (ioworking)
            {
                long now = DateTime.UtcNow.Ticks;

                foreach (remoteinfo test in actives)
                    if (test != null)
                    {
                        test.startwrite();
                    }
                Thread.Sleep(2);
            }
        }
        private void clean_ing()
        {
            long stale_delay = TimeSpan.TicksPerSecond*10;
            int count = 0;
            while (ioworking)
            {
                long now = DateTime.UtcNow.Ticks;

                if (count > maxremotes - 1) count = 0;

                remoteinfo test = actives[count];

                if (test != null)
                    if (now - test.lastactive > stale_delay || test.dead)
                    {
                        test.ondead?.Invoke(test);
#if DEBUG
                        On_Info(test.remotename + " disconnected...");
#endif
                        actives[test.idx] = null;
                        test.close();
                        test = null;
                    }

                count++;

                Thread.Sleep(40);//250 clients 40 msecs = 10 sec per revolution
            }
        }
        private void accept_ing()
        {
            self.Blocking = true;
            while (ioworking)
            {
                try
                {
                    Socket client = self.Accept();
                    //client.Blocking = false;
                    client.ReceiveBufferSize = packet.maxpacketsize;
                    //client.ReceiveTimeout = 0;
                    remoteinfo newbie = new remoteinfo(ref client, "client", true);
#if DEBUG
                    On_Info("starting setup");
#endif
                    newbie.setup(add_connection);
                }
                catch (Exception) { }
                Thread.Sleep(40);
            }
        }

        private void add_connection(remoteinfo newbie)
        {
            lock (locker_active)
            {
                int find_idx = Array.FindIndex<remoteinfo>(actives, x => x == null);

                if (find_idx != -1)
                {
                    newbie.idx = find_idx;
                    newbie.onreceive = Received;
                    newbie.startread();
                    //after placing in actives remote behaves in normal mod - crypted and so on.
                    actives[find_idx] = newbie;
                    Connected?.Invoke(newbie);
                }
            }
        }
        #endregion
    }
}
