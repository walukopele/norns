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
        #region jobs
        private void increment(ref asset C, job self)
        {
            ((serverdata)C).test++;
        }
        private void restart(ref asset C, job self)
        {
            if (self.repeatcount == 0)
            {
                this.Parent.stop();
                this.Parent.start();
            }
            else
            {
                Broadcast(
                    new packet(
                        packet.server_target,
                        packet.err,
                        status_message("server will restart") + self.ETA().ToString()));
            }
        }

        private void log_status(ref asset c, job self)
        {
            log.Add(Parent.PPS.ToString() + ":" + Parent.ClientsCount.ToString());
        }

        private void update_status(ref asset c, job self)
        {
            this.data.pps = Parent.PPS;
            if (data.stats_max_pps_ever < data.pps) data.stats_max_pps_ever = data.pps;
            this.data.clientcount = Parent.ClientsCount;
        }
        private void clean(ref asset C, job self)
        {
            ((serverdata)C).clean_acc(self.current_time, ((serverdata)C).awaiting_proofs);
            ((serverdata)C).clean_acc(self.current_time, ((serverdata)C).logged_in);
        }
        #endregion
    }
}
