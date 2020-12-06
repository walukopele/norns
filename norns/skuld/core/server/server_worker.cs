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
using System.Diagnostics;
using verdandi;

namespace skuld
{
    class server_worker : worker
    {
        serverdata data { get { return (serverdata)cache; } }
        server Parent;
        server_network network;

        public server_worker(server parent, server_network net)
        {
            network = net;
            Parent = parent;
            cache = new serverdata();
        }

        #region setup
        protected override void setup_commands()
        {
            this.register_command("broadcast", "U", "U", broadcast, privilege.admin);

            this.register_command("start", "N", "N", start, privilege.admin);
            this.register_command("stop", "N", "N", stop, privilege.admin);
            this.register_command("listwrk", "N", "u", list_workers, privilege.admin);
            this.register_command("svcadd", "U", "u", add_service, privilege.admin);
            this.register_command("svcdel", "U", "u", removeservice, privilege.admin);
            this.register_command("accesslevel", "LI", "m", accesslevel, privilege.admin);
            this.register_command("listdumps", "N", "u", listdumps, privilege.admin);
            

            this.register_command("ping", "L", "L", ping, privilege.everyone);
            this.register_command("serviceinfo", "I", "u", serviceinfo, privilege.everyone);
            this.register_command("auth", "LL", "S", login, privilege.everyone);
            this.register_command("new_user", "N", "LL", new_user, privilege.everyone);
            this.register_command("status", "N", "u", status, privilege.everyone);
        }

     

        protected override void setup_shedules()
        {
            this.register_shedule("update_status", update_status);
            this.register_shedule("login_timeout", login_timeout);
            this.register_shedule("increment", increment);
            this.register_shedule("stop_countdown", stop_countdown);
        }

    

        protected override void post_setup_work()
        {
            this.run_shedule("update_status", null, true, 0, new TimeSpan(0, 0, 1).Ticks);
        }
        #endregion

        #region packets
        private packet broadcast(packet p, object session)
        {
            packet br = new packet(0,1);
            br.Write(p.String);
            Broadcast(br);
            return null;
        }
        private packet removeservice(packet p, object session)
        {
            string what = p.String;

            if (!Array.Exists(Parent.ActiveServices, x => x == what))
            {
                Parent.unregister(what);
                return new packet(p, "done");
            }
            else return new packet(p, "wrong name");
        }
        private packet add_service(packet p, object session)
        {
            string[] what = p.String.Split(' ');
            if (what.Length == 2)
                if (Array.Exists(Parent.KnownWorkers, x => x == what[0]))
                    if (!Array.Exists(Parent.ActiveServices, x => x == what[1]))
                    {
                        Parent.register(new serviceinfo(what[1], what[0], data.config.serveraddress, false));
                        return new packet(p, "done");
                    }
                    else return new packet(p, "wrong name");
                else return new packet(p, "wrong worker");
            else return new packet(p, "wrong input need 'name workertype'");
        }
        private packet list_workers(packet p, object session)
        {
            string[] wrk = Parent.KnownWorkers;
            string ret = "";
            foreach (string s in wrk)
            {
                ret += s + "\n";
            }
            return new packet(p, ret);
        }
        private packet format_description(packet p, object session)
        {
            string ret = "This is general help about command format.\n";
            ret += "Commands to server and from server has following format:";
            ret += "'XXXXXX..' - where every letter X is for type of argument. Order of letters is the same as order of arguments.\n";

            ret += "'N' is undefined(null)\n";
            ret += "'V' is nothing(void)\n";

            ret += "'B' is signed 8-bit integer\n";
            ret += "'b' is unsigned 8-bit integer\n";

            ret += "'S' is signed 16-bit integer\n";
            ret += "'s' is unsigned 16-bit integer\n";

            ret += "'I' is signed 32-bit integer\n";
            ret += "'i' is unsigned 32-bit integer\n";

            ret += "'L' is signed 64-bit integer\n";
            ret += "'l' is unsigned 64-bit integer\n";

            ret += "'D' is double precision floating point\n";
            ret += "'F' is single precision floating point\n";

            //ret += "'U' is null terminated UTF16 string\n";
            ret += "'u' is null terminated UTF8 string\n";

            ret += "Any numeric values can have any order but string argument should be last in packet.\n";
            return new packet(p, ret);
        }
        private packet checktest(packet p, object session)
        {
            packet ret = new packet(p);
            ret.Write(data.test);
            return ret;
        }
        private packet listdumps(packet p, object session)
        {
            return new packet(p, hddiosheduler.Instance().List());
        }
        private packet accesslevel(packet p, object session)
        {
            long uid = p.ReadSL();
            sbyte accesslevel = p.ReadSB();

            int idx = data.accounts.FindIndex(x => x.uid == uid);
            if (idx != -1)
            {
                data.accounts[idx].accesslevel = accesslevel;
            }
            return null;
        }
        private packet ping(packet p, object session)
        {
            run_shedule("increment", (session)session);
            packet ret = new packet(p);
            ret.Write(p.ReadSL());
            ret.Write(((session)session).session_account.accesslevel);                                 

            return ret;
        }
        private packet login(packet p, object session)
        {
            session session_current = (session)session;
            packet ret = new packet(p);
            short ok = 1;
            short not_exists = 2;

            long uid = p.ReadSL();
            long passB = p.ReadSL();

            log.Add(session_current.connection_uid.ToString() + " user trying to auth to account");

            account a = null;
            if (!data.accounts.Exists(x => x.uid == uid && x.passB == passB))
            {

                log.Add(session_current.connection_uid.ToString() + " no such account to auth " + uid.ToString());
                ret.Write(not_exists);
                return ret;
            }
            else a = data.accounts.Find(x => x.uid == uid && x.passB == passB);

            if (session_current != null)
            {
                session_current.session_account = a;
                log.Add(a.name + " auth success. associated with " + session_current.connection_uid.ToString());


            }
            ret.Write(ok);
            return ret;
        } 
        private packet new_user(packet p, object session)
        {
            byte[] buf = new byte[8];
            new Random().NextBytes(buf);
            long uid = DateTime.UtcNow.Ticks;
            long passB = BitConverter.ToInt64(buf, 0);
            session s = (session)session;

            log.Add(s.connection_uid.ToString()+" try to create new account");

            if (!data.accounts.Exists(x => x.uid == uid && x.passB == passB))
            {
                account a = null;
                if (data.accounts.Count == 0)
                {
                    a = new account("God", data.accounts.Count);
                    a.accesslevel = (int)privilege.admin;
                }
                else
                {
                    a = new account("Anonymous", data.accounts.Count);
                    a.accesslevel = (int)privilege.anonymous;
                }

                a.uid = uid;
                a.passB = passB;

                data.accounts.Add(a);

                log.Add(s.connection_uid.ToString() + " new account created");

                packet ret = new packet(p);
                ret.Write(uid);
                ret.Write(passB);

                return ret;
            }
            else
            {
                log.Add(s.connection_uid.ToString() + " already exists");
                return null;
            }
        }
        private packet serviceinfo(packet p, object session)
        {
            string ret = "";
            foreach (serviceinfo m in data.serviceinfo)
            {
                if(m.running)
                ret += m.servicename + "," + m.internalport.ToString() + ";\n";
            }
            return new packet(p, ret);
        }        
        private packet status(packet p, object session)
        {
            packet ret = new packet(p);
            ret.Write(Parent.PPS);
            ret.Write(Parent.ClientsCount);
            ret.Write(data.maxclientcount);
            ret.Write(Parent.WPS);
            return ret; 
        }
        private packet start(packet p, object session)
        {
            Parent.start();            
            return new packet(p, "server started...");
        }
        private packet stop(packet p, object session)
        {
            this.run_shedule("stop_countdown", (session)session, DateTime.UtcNow, new TimeSpan(0, 0, 5), 10);
            return null;
        }
        #endregion

        #region jobs            
        private void increment(ref asset C, job self, ref session s)
        {
            ((serverdata)C).test++;
        }

        private void login_timeout(ref asset C, job self, ref session s)
        {
            serverdata dat = (serverdata)C;
        }        
        
        private void update_status(ref asset c, job self, ref session s)
        {
            this.data.pps = Parent.PPS;           
            this.data.clientcount = Parent.ClientsCount;            
        }
        private void stop_countdown(ref asset C, job self, ref session s)
        {
            if (self.repeatcount == 0)
            {               
                this.Parent.stop();
                Broadcast(new packet(0,1,"server stopped."));
            }
            else
            {
                Broadcast(
                    new packet(0,1,"server will restart in " + self.ETA().ToString() + " seconds..."));
            }
        }
        #endregion

        #region randomutils
       
        #endregion
    }
}
