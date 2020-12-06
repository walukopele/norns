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

            ret += "'U' is null terminated UTF16 string\n";         

            ret += "'m' is code for status_message\n";

            ret += "Any numeric values can have any order but string argument should be last in packet.\n";
            return new packet(p, ret);
        }
        private packet checktest(packet p, object session)
        {
            return new packet(p, data.test);
        }
        private packet help_error(packet p, object session)
        {  
            return new packet(p,name_error(p.ReadSI()));
        }
        private packet listdumps(packet p, object session)
        {
            return new packet(p, hddiosheduler.Instance().List());
        }
        private packet status(packet p, object session)
        {
            return new packet(p, data.pps, data.clientcount,data.maxclients);
        }
        private packet ping(packet p, object session)
        {
            run_shedule("increment");
            
            return new packet(p,p.ReadSL(),((session)session).session_account.accesslevel);
        }
        private packet listshedules(packet p, object session)
        {
            string ret="";
            
            Dictionary<string, int> unames = new Dictionary<string, int>();

            foreach (job j in data.jobs)
                {
                if (unames.ContainsKey(j.name))
                    unames[j.name]++;
                else unames[j.name] = 1;
            }
            foreach (KeyValuePair<string, int> un in unames)
            {
                ret += un.Key + "=" + un.Value+"; ";
            }

            return new packet(p, ret);
        }
        private packet listlogged(packet p, object session)
        {
            string logged = "";
            Dictionary<string, int> unames = new Dictionary<string, int>();
            foreach (account a in data.logged_in)
            {
                if (unames.ContainsKey(a.name))
                    unames[a.name]++;
                else unames[a.name] = 1;
            }

            foreach (KeyValuePair<string, int> un in unames)
            {
                logged += un.Key + "=" + un.Value + "; ";
            }

            return new packet(p, logged);
        }
    }
}