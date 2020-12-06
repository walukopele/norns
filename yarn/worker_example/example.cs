using System;
using System.Collections.Generic;
using verdandi;//network

namespace skuld//server namespace
{
    public class example:worker
    {        
        //!!
        example_worker_data data { get { return (example_worker_data)cache; } }

        public example()
        {
            //!!
            cache = new example_worker_data();
        }

        protected override void setup_commands()
        {
            this.register_command("action", "V","V", server_action, privilege.user);         
        }

        protected override void setup_shedules()
        {
            this.register_shedule("test_shedule", test_shedule);
        }

        protected override void post_setup_work()
        {
            
        }
        //server actions
        private packet server_action(packet p, object session)
        {
            session s = (session)session;                       
            //            
            account a = s.session_account;
            //
            this.run_shedule("test_shedule",s,  new TimeSpan(0, 0, 7), 5);
            //
            return null;
        }
        //shedules
        private void test_shedule(ref asset c, job self, ref session s)
        {
            example_worker_data worker_Data = (example_worker_data)c;            
            worker_Data.some_number++;
        }


    }
}
