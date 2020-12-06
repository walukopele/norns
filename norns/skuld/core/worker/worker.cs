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
// 
// 
using verdandi;
using System.Collections.Generic;
using System.Collections.Concurrent;
//using NLua;


namespace skuld
{
    //коннект падает на хендшейке!!!

    ///
    //добавить автокалькуляцию кеша для сборок наследуемых от этого класса.
    /// <summary>
    /// how to use
    /// create Work delegate
    /// register new command with it
    /// 
    /// </summary>
    public abstract class worker//<T>
    {
        public asset cache;// { get; protected set; }
        public string Name { get; private set; }
       
        private commands Commands { get; set; }
        private sheduler Sheduler;
        private service master;
      

        protected Log log; 

        public worker()
        {
            
        }
        /// <summary>
        /// broadcast needed for broadcast to all clients
        /// services needed for internal messaging
        /// </summary>
        /// <param name="master"></param>
        /// <param name="shed"></param>
        /// <param name="log"></param>
        /// <param name="services"></param>
        /// <param name="broadcast_queue"></param>
        public void Setup(service master, Log log)
        {
            log.Add("starting setup of " + master.ServiceName + " worker...");
            
            this.Name = master.ServiceName;
            this.master = master;
            this.Sheduler = master.sheduler;           
            this.log = log;

            this.Commands = new commands("");

            this.register_command("bootstrap", "L","R", this.bootstrap, privilege.everyone);//0

            this.setup_commands();

            this.Commands.settargetname(this.Name);

            this.setup_shedules();

            this.post_setup_work();

            log.Add("setup of " + master.ServiceName + " worker finished!");
        }

        public void setinternalport(byte port)
        {
            Commands.settargetbyte(port);
        }

        protected abstract void setup_commands();

        protected abstract void setup_shedules();

        protected abstract void post_setup_work();

        #region register
        protected void register_command(string name,string fmt_toserver,string fmt_toclient, commands.ServerWork w, privilege prv = privilege.user)
        {
            try
            {
                Commands.RegisterServerCommand(name, fmt_toserver,fmt_toclient, w, prv);
            }
            catch (Exception e)
            {
                log.Add("[worker.setup."+name+"][e]:" + e.Message, Log.loglevel.error);
            }
        }
        protected void register_shedule(string desired_name, job_delegate job)
        {
            Sheduler.Reg(desired_name, job);
        }
        protected void run_shedule(string registered_jobname,  session s, TimeSpan interval, long repeatcount = 0)
        {
            Sheduler.Run(registered_jobname, s,interval, repeatcount);
        }
        protected void run_shedule(string registered_jobname,  session s, DateTime nextrun, TimeSpan interval, long repeatcount = 0)
        {
            Sheduler.Run(registered_jobname,  s,  nextrun, interval, repeatcount);
        }
        protected void run_shedule(string registered_jobname,  session s, TimeSpan startdelay, TimeSpan interval, long repeatcount = 0)
        {
            Sheduler.Run(registered_jobname,  s, startdelay, interval, repeatcount);
        }
        protected void run_shedule(string registered_jobname,  session s)
        {
            Sheduler.Run(registered_jobname, s,false, 0, 0, 0);
        }
        protected void run_shedule(string registered_jobname,  session s, bool unique = false, long nextrun = 0, long interval = 0, long repeatcount = long.MaxValue)//job task)
        {
            Sheduler.Run(registered_jobname, s, unique, nextrun, interval, repeatcount);
        }
            #endregion
    
            #region Utilites
        //    protected void Broadcast(packet p)
        //{
        //    master.Broadcast(p);            
        //}

        /// <summary>
        /// some shitty implementation of interservice messaging
        /// todo: remake with fresh head to eliminate listfind overhead
        /// </summary>
        /// <param name="p"></param>
        /// <param name="serviceName"></param>
        /// <param name="commandName"></param>
        /// <returns></returns>
        protected packet ForwardInternal(packet p,session ses, string serviceName, string commandName)
        {
            return master.ForwardInternal(p,ses, serviceName, commandName);
        }
        protected void Broadcast(packet p)
        {
            master.Broadcast(p);
        }
        public byte getCmdByte(string srvname, string cmdname)
        {
            return Commands.Get_Remote(srvname, cmdname).remote_command;
        }
        #endregion
        #region packets
        public packet process(packet received,object session)
        {
            if (received.Bytes.Length == 0) return null;
            session s = (session)session;
            if (!received.is_internal)//external
            {
                account current_account = s.session_account;
                return Commands.ServerDo(received, session, (privilege)current_account.accesslevel);
            }
            //internal
            else return Commands.ServerDo(received, session, privilege.server);
        }
        packet bootstrap(packet p, object session)
        {            
            string ex = Commands.extract();
            return new packet(p, ex);
        }                
        #endregion
        #region jobs
        
        #endregion

    }
}
