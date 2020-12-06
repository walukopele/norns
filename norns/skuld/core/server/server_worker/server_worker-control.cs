using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verdandi;

namespace skuld
{
    partial class server_worker : worker
    {
        private packet broadcast(packet p, object session)
        {
            packet br = new packet(
                    packet.server_target,
                    packet.err
                    );
            br.Write(p.String);
            Broadcast(br);
            return new packet(p, status_message("ok"));
        }
        private packet do_serviceinfo(packet p, object session)
        {
            string ret = "";
            foreach (serviceinfo m in data.serviceinfo)
            {
                if (m.running)
                    ret += m.servicename + "," + m.hostipaddress + "," + m.hostipport + "," + m.internalport.ToString() + ";";
            }
            return new packet(p.target, p.remote_command, ret);
        }
        private packet do_reset_server(packet p, object session)
        {
            session s = (session)session;
            account account = s.session_account;

            byte count = p.ReadUB();            

            this.run_shedule("restart", true, DateTime.UtcNow.Ticks, new TimeSpan(0, 0, 5).Ticks, count);
            return new packet(p, status_message("server is restarting soon..."));
        }
        private packet start(packet p, object session)
        {
            Parent.start();
            Broadcast(p);
            return new packet(p, status_message("server started..."));
        }
        private packet stop(packet p, object session)
        {
            Parent.stop();
            return new packet(p, status_message("server stopped..."));
        }
        private packet reg_svc(packet p, object session)
        {

            string[] what = p.String.Split(' ');

            if (what.Length == 2)
            {
                if (Parent.RegisteredServices.Length > byte.MaxValue - 2)
                    return new packet(p, status_message("server is full of services"));
                string workername = what[0];
                string servicename = what[1];
                if (Array.Exists<string>(Parent.KnownWorkers, x => x == workername))
                {
                    if (Parent.register(new serviceinfo(servicename, workername)))
                    {                        
                        return new packet(p, status_message("service registered"));
                    }
                    else return new packet(p, status_message("service already exists"));
                }
                else return new packet(p, status_message("no such worker"));

            }

            return new packet(p, status_message("nothing happened"));
        }
        private packet unreg_svc(packet p, object session)
        {
            string svcname = p.String;
            if (Array.Exists<string>(Parent.RegisteredServices, x => x == svcname))
            {
                Parent.unregister(svcname);
                return new packet(p, svcname + " deleted");
            }
            else return new packet(p, status_message("no such service exists"));
        }      
        private packet list_workers(packet p, object session)
        {
            string[] workers = Parent.KnownWorkers;
            string ret = "";
            foreach (string w in workers)
            {
                ret += w + " ";
            }
            return new packet(p, ret);
                
        }
    }
}
