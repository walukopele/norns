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
        serverdata data { get { return (serverdata)cache; } }
        server Parent;
        server_network network;

        public server_worker(server parent, server_network net)
        {
            network = net;
            Parent = parent;
            cache = new serverdata();
            Version = "o15";
        }

        #region setup
        protected override void setup_commands()
        {
            this.register_command("broadcast", "U", "m", broadcast, privilege.gm);//client bounded
            this.register_command("server_start", "N", "m", start, privilege.administrator);//client bounded
            this.register_command("server_stop", "N", "m", stop, privilege.administrator);//client bounded

            this.register_command("format", "N", "U", format_description, privilege.everyone);
            this.register_command("ping", "L", "L", ping, privilege.everyone);
            this.register_command("serviceinfo", "I", "u", do_serviceinfo, privilege.everyone);
            this.register_command("auth", "SLL", "sm", do_auth, privilege.everyone);

            this.register_command("impersonate", "bU", "m", impersonate, privilege.gm);
            this.register_command("server_restart", "b", "m", do_reset_server, privilege.administrator);
            this.register_command("togglegm", "U", "m", togglegm, privilege.administrator);

            this.register_command("server_status", "N", "III", status, privilege.everyone);
            this.register_command("helperror", "I", "U", help_error, privilege.everyone);

            this.register_command("list_dumps", "N", "U", listdumps, privilege.gm);
            this.register_command("list_sheds", "N", "U", listshedules, privilege.gm);
            this.register_command("list_logged", "N", "U", listlogged, privilege.gm);
            this.register_command("list_workers", "N", "U", list_workers, privilege.administrator);

            this.register_command("checktest", "N", "L", checktest, privilege.anonymous);

            this.register_command("reg_svc", "U", "m", reg_svc, privilege.administrator);
            this.register_command("unreg_svc", "U", "m", unreg_svc, privilege.administrator);


            //
            this.register_command("reg_node", "U", "N", regnode, privilege.administrator);
            this.register_command("list_nodes", "U", "N", listnodes, privilege.administrator);
            this.register_command("unreg_node", "U", "N", unregnode, privilege.administrator);
            this.register_command("deploysvcto", "U", "N", deploysvcto, privilege.administrator);
            //

            //this.register_command("id", "N", "U", server_id, privilege.everyone);
        }

        //private packet server_id(packet p, object session)
        //{
        //    return new packet(p, data.UniqueID.self_publickeyinfo);
        //}


        private packet deploysvcto(packet p, object session)
        {
            throw new NotImplementedException();
        }

        private packet unregnode(packet p, object session)
        {
            throw new NotImplementedException();
        }

        private packet listnodes(packet p, object session)
        {
            throw new NotImplementedException();
        }

        private packet regnode(packet p, object session)
        {
            throw new NotImplementedException();
        }

        protected override void setup_shedules()
        {
            this.register_shedule("update_status", update_status);
            this.register_shedule("clean", clean);
            this.register_shedule("restart", restart);
            this.register_shedule("increment", increment);
        }
        protected override void run_shedules()
        {
            this.run_shedule("update_status", true, 0, new TimeSpan(0, 0, 1).Ticks);
            this.run_shedule("clean", true, 0, new TimeSpan(0, 0, 5).Ticks);
        }


        protected override void post_setup_work()
        {
            //cache relate
            data.setup();
        }
        #endregion
    }
}
