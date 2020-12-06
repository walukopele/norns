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

//using verdandi;
using verdandi;
using System.Collections.Generic;
using System.Threading;

namespace skuld
{
    /// <summary>

    /// </summary>
    public class service
    {
        class task
        {
            public session ses;
            public packet p;
            public task(ref session session, ref packet packet)
            {
                ses = session;
                p = packet;
            }
        }

        public string ServiceName { get; protected set; }
        public sheduler sheduler { get; protected set; }
        public worker subordinate { get; protected set; }
        public Log log;

        public string WorkerType { get { return subordinate.GetType().ToString(); } }
        public string WorkerTypeShort { get { return subordinate.GetType().Name; } }

        //packet sort
        private Thread packet_process;
        private List<task> toworker = new List<task>();
        private bool working = false;
        private object locker = new object();
       
        server_network net;
        List<service> collective;

        public service()
        {

        }

        public void Init(List<service> collective, server_network net, serviceinfo info, worker w)
        {
            this.log = new Log(info.servicename);
            this.ServiceName = info.servicename;
            this.net = net;
            this.collective = collective;

            this.log.Add("loading cache...");
            this.subordinate = w;
            this.subordinate.cache.log = this.log;
            //here loading occurs
            this.sheduler = new sheduler(info.servicename, ref subordinate.cache);
            this.log.Add("cache loaded!");

            subordinate.Setup(this, this.log);
        }
        public void Broadcast(packet p)
        {
            foreach (session s in net.Sessions)
            {
                s.remote.Add(p);
            }
        }
        public packet ForwardInternal(packet p, session ses, string serviceName, string commandName)
        {
            if (p.is_internal)
            {
                int idx = collective.FindIndex(x => x.ServiceName == serviceName);
                if (idx != -1)
                {
                    p.target = (byte)idx;

                    p.remote_command = (byte)collective[idx].subordinate.getCmdByte(serviceName, commandName);

                    return collective[idx].subordinate.process(p, ses);
                }
            }
            return null;
        }

        public void add(packet p, session ses)
        {
            lock (locker)
                toworker.Add(new task(ref ses, ref p));
        }
        private void process()
        {
            int sleepcount = 0;
            List<task> temp = new List<task>();
            while (working)
            {
                temp.Clear();
                lock (locker)
                {
                    if (toworker.Count > 0)
                    {
                        temp.AddRange(toworker);
                        toworker.Clear();
                    }
                }
                foreach (task t in temp)
                {
                    packet p = subordinate.process(t.p, t.ses);
                    if (p != null)
                        t.ses.remote.Add(p);
                }

                sleepcount++;
                if (sleepcount > 60)
                {
                    Thread.Sleep(2);
                    sleepcount = 0;
                }
            }
        }

        public void stop()
        {
            log.Add("stopping " + this.ServiceName + "...");
            working = false;
            packet_process.Abort();

            sheduler.Clear();
            sheduler.Close();

            log.Add(this.ServiceName + " stopped!");
        }
        public void start(byte internalport)
        {
            log.Add("starting " + this.ServiceName + "...");
            subordinate.setinternalport(internalport);
            sheduler.start();

            working = true;
            packet_process = new Thread(process);
            packet_process.Name = ServiceName + " P proc";
            packet_process.Start();

            log.Add(this.ServiceName + " started!");
        }
    }
}
