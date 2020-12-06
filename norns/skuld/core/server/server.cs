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
using System.Text;
using System.Reflection;
using System.Net;
using System.Security.Cryptography;
using System.IO;

using verdandi;

namespace skuld
{

    public class server
    {
        private object lock_object = new object();
        private List<Type> workers = new List<Type>();
        private server_network network;
        private server_service sservice;
        private bool started;
        private Log log = new Log("loading");

        public List<service> Collective { get; private set; } = new List<service>();        

        public bool Started { get { return started; } }
        public int ClientsCount { get { return network.ConnectedClients; } }
       
        public int PPS { get { return network.PacketPerSecond; } }
        public string WPS { get { return network.WritedPerSecond; } }
        public string IP { get { return sservice.data.config.serveraddress; } }
        public string Port { get { return sservice.data.config.port.ToString(); } }
        public List<account> Accounts{ get { return sservice.data.accounts; }}
        public List<session> Sessions { get { return network.Sessions; } }
        public server()
        {
            
            network = new server_network(Collective);
            //init data first
            sservice = new server_service(this, network);
            sservice.start(0);
            //next start network and pass servicelist as targets to request parsing

            network.start_network(sservice.data.config.ipaddress, sservice.data.config.port,sservice.data.maxclientcount);
            Collective.Add(sservice);
            started = false;
            scan();
            if (sservice.data.autostart) start();
        }





        public void SetIPPort(string ipaddress, string port = "1888")
        {
            IPAddress ip = IPAddress.Any;
            short p = 0;
            if (IPAddress.TryParse(ipaddress, out ip))
            {
                if (Int16.TryParse(port, out p))
                {
                    sservice.data.config.serveraddress = ipaddress;
                    sservice.data.config.port = p;
                }
            }
        }

        public void start()
        {
            if (!started)
            {
                if (!network.Ready) network.start_network(sservice.data.config.ipaddress, sservice.data.config.port,sservice.data.maxclientcount);
                log.Add("preparing all microservices...");
                try
                {
                    if (scan())
                    {
                        try
                        {
                            init_services(RegisteredServices);                          
                            started = true;
                            log.Add("all microservices started and ready on: " + this.network.Address + ":" + network.IPPort.ToString());
                        }
                        catch (Exception e) { log.Add("server.start", e); }
                    }
                    else
                        log.Add("server NOT started");
                }
                catch (Exception e) { log.Add("server.start", e); }

            }
        }
        public void stop()
        {

            log.Add("stopping server...");
            if (started)
            {
                started = false;


                for (byte i = 1; i < Collective.Count; i++)
                {
                    //log.Watch();
                    Collective[i].stop();
                    int idx =
                    sservice.data.serviceinfo.FindIndex(x => x.servicename == Collective[i].ServiceName);
                    if (idx != -1) sservice.data.serviceinfo[idx].running = false;
                    log.Add(Collective[i].ServiceName + " stopped");
                }

                Collective.Clear();
                Collective.Add(sservice);
            }
        }

        public void close()
        {            
            network.stop_network();
            stop();
            sservice.stop();
            hddiosheduler.Instance().Close();
        }

        

        public string[] KnownWorkers
        {
            get
            {
                List<string> temp = new List<string>();
                foreach (Type t in workers)
                {
                    temp.Add(t.FullName);
                }
                return temp.ToArray();
            }
        }

        public string[] RegisteredServices
        {
            get
            {
                List<string> temp = new List<string>();
                temp.Add("server");
                foreach (serviceinfo m in sservice.data.serviceinfo)
                {
                    temp.Add(m.servicename);
                }
                return temp.ToArray();
            }
        }
        public string[] ActiveServices
        {
            get
            {
                List<string> temp = new List<string>();                
                foreach (service s in Collective)
                {
                    temp.Add(s.ServiceName);
                }
                return temp.ToArray();
            }
        }


        public void register(serviceinfo info) 
        {
            if (sservice.data.serviceinfo.Exists(x =>
               // x.hostipaddress == info.hostipaddress &&
                x.internalport == info.internalport   &&
                x.servicename == info.servicename
                )) 
            {
                this.log.Add("cant register microservice with name:'"+info.servicename +"' check info");
                return;
            }

            info.internalport = (byte)sservice.data.serviceinfo.Count;
            init_one(info.servicename);
            sservice.data.serviceinfo.Add(info);
        }

        public void unregister(string servicename) 
        {
            int idx = Collective.FindIndex(x => x.ServiceName == servicename);
            if (idx != -1)
            {
                Collective[idx].stop();
                Collective.RemoveAt(idx);
                sservice.data.serviceinfo.RemoveAll(x => x.servicename == servicename);
            }
        }


        /// <summary>
        /// scan for plugins in plugin folder
        /// </summary>      
        private bool scan()
        {
            bool good = true;

            string path = sservice.data.config.workersfolder;
            //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            log.Add("scan starting in " + path);
            string[] paths = System.IO.Directory.GetFiles(path, "*.dll");
            int count = 0;
            workers.Clear();
            foreach (string p in paths)
            {
                string name = Path.GetFileName(p);
                try
                {
                    Assembly a = Assembly.LoadFrom(p);
                    Type[] types = a.GetTypes();


                    foreach (Type o in types)
                    {

                        if (o.BaseType != null && o.BaseType.Name == "worker")
                        {
                            count++;
                            workers.Add(o);                           
                            log.Add("found " + o.FullName + " in " + p);
                        }

                    }
                }
                catch (Exception) { log.Add("server.scan: "+name+" is broken");  }                
            }
            log.Add("found " + count.ToString() + " total while scan");

            return good;
        }

        private void init_services(string[] servicenames) 
        {
            List<service> services = new List<service>();
            foreach (string s in servicenames)
            {

                log.Add("init " + s +" microservice");
                
                try
                {
                   init_one(s);                    
                }
                catch (Exception ex) { log.Add(s+" init microservice error",ex); }
                
            }
           // sservice.Save();
            
            
        }

        private void init_one(string servicename)
        {
            if (sservice.data.serviceinfo.Exists(x => x.servicename == servicename))
            {
                byte idx = (byte)sservice.data.serviceinfo.FindIndex(x => x.servicename == servicename);
                

                    serviceinfo info = sservice.data.serviceinfo[idx];

                    if (info.remote) return;
                    //info.internalport = idx;
                    if (!info.remote)
                    {
                        //info.hostipaddress = network.Address;
                        info.hostipport = network.IPPort;
                    }


                    //string workerpath = sservice.data.winfo.Find(x => x.typename == workertype).path;

                    Type t = this.workers.Find(x => x.FullName == info.workertype);
                    if (t == null)
                    {
                        log.Add(servicename + " init failed no suitable worker found for type:" + info.workertype);

                        return;
                    }

                    worker w = (worker)Activator.CreateInstance(t);

                    service ret = new service();

                    ret.Init(Collective,network, info, w);
                    info.typename = ret.subordinate.GetType().Name;
                    ret.start((byte)Collective.Count);
                    info.internalport = (byte)Collective.Count;

                    this.Collective.Add(ret);
                    //
                    info.running = true;

                    sservice.data.serviceinfo[idx] = info;
                    log.Add(info.typename+" is started as "+ info.servicename+" at " + info.internalport.ToString()+" internalport");
                
            }
        }
    }
}
