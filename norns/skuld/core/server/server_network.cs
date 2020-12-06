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
using System.Threading;


using verdandi;

namespace skuld
{
    /// <summary>
    /// sort packets to services
    /// </summary>
    public class server_network
    {
        private int clientscount;
        private List<service> targets;
        private exchanger ex;
        private idfactory idf;
        private Log log;

        private readonly object seslocker = new object();       
        private readonly object brolocker = new object();

        protected IPAddress address = IPAddress.Any;
        protected short port = 1888;

        public string Address { get { return address.ToString(); } }        
        public short IPPort { get { return port; } }
        public int ConnectedClients { get { return ex.CountConnected; } }
        public int PacketPerSecond { get { return ex.PPS; } }
        public string WritedPerSecond { get { return ex.BytesWrite; } }
        public string[] Clients { get { return ex.Remotes; } }
        public bool Ready { get { return address != null && ex != null; } }
        public List<session> Sessions { get; private set; } = new List<session>();
    
        public server_network(List<service> targets) 
        {            
            this.targets = targets;
            this.log = new Log("network");
            ex = new exchanger(1);            
            idf = new idfactory(1);
        }

        public void start_network(IPAddress ip, short port, int maxclients)
        {
            log.Add("starting network...");
            clientscount = maxclients;
            ex = new exchanger(maxclients); 
            idf = new idfactory(maxclients);

            ex.Connected = setup_session;
            ex.Received = received;
            ex.On_Info += Ex_On_Info;
            
            this.address = ip;
            this.port = port;
            try
            {
                ex.Listen(ip, port);
                log.Add("network started!");
            }
            catch (Exception e) { log.Add("[network]:" + e.Source + " " + e.Message, Log.loglevel.error); }          
        }
        public void stop_network()
        {
            try
            {
                ex.Close();
                log.Add("server network has stopped");
            }
            catch (Exception e) { log.Add("[network]:", e); }
        }

        private void received(remoteinfo c)
        {
            session ses = (session)c.session;
            if (ses != null)
            {
                packet[] temp = c.Get;
                
                foreach (packet o in temp)
                {
                    if (o != null && o.target < targets.Count)
                    {
                        targets[o.target].add(o, ses);
                    }
                }
            }
        }
        private void Ex_On_Info(string what)
        {
            log.Add(what);
        }
        private void setup_session(remoteinfo ci)
        {
            try
            {               
                ci.connection_uid = idf.Asquire();
                
                session ss = new session(ci.connection_uid);
                ci.session = ss;
                ci.ondead = OnDead;
                ss.remote = ci;
                Sessions.Add(ss);
                log.Add(
                    ci.endpoint
                    + " connected and got " + ci.connection_uid.ToString()
                    + " id");
            }
            catch(Exception e)
            {
                log.Add("[network]", e);
                ci.close();
            }
        }
        private void OnDead(remoteinfo r)
        {
            session s = (session)r.session;
            log.Add(s.connection_uid+" closed with message: '"+r.lasterror+"'");
            Sessions.Remove(s);
        }
    }
}
