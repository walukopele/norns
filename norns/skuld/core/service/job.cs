using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skuld
{
    public delegate void job_delegate(ref asset C, job self,ref session s);

    public class job
    {
        public enum status : byte
        {
            undefined,
            ok,
            canceled,
            fail,
            need_repeat,
            working,
            done
        }
        public status Status;

        static long tps = TimeSpan.TicksPerSecond;

        //do not save
        asset C;
        job_delegate thingtodo;
        session s;
        //save
        [cache] public string name = "";
        [cache] public long nextrun = 0;
        [cache] public long interval = 0;
        [cache] public long repeatcount = 0;
        //[cache] public bool fast = true;
        public void Kill()
        {
            nextrun = interval = repeatcount = 0;
        }
        public long ETA()
        {
            long seconds = interval / tps;
            return seconds * repeatcount;
        }

        public long TillNextRun(long now)
        {
            return (nextrun - now) / tps;
        }

        public bool IsFast(long limiter)
        {
            return interval < limiter+5;
        }
        public bool Do(long now)
        {
            //Status = status.working;
            long delta = nextrun-now;
            if (delta > 0)
            {
                Status = status.ok;
                return false;//time to process has not come yet;
            }
            else //start time achieved;
            {
                if (repeatcount == 0) //time to stop repeat here
                {
                    if (thingtodo != null && C != null)
                        lock (C.sync)
                            thingtodo(ref C, this,ref s);

                    Status = status.done;

                    return true;
                    //possible bugs maybe. br; //when job done job manager will delete job
                }
                else //need to repeat job
                {
                    if (thingtodo != null && C != null)
                        lock (C.sync)
                            thingtodo(ref C, this,ref s);

                    nextrun = now + interval;
                    repeatcount = repeatcount - 1;
                }
            }
            return false;
        }

        public void reinstall(ref asset c, job_delegate work,session s)
        {
            this.C = c;
            this.thingtodo = work;
            this.s = s;
        }
        public job(string name, job_delegate payload, ref asset c, long nextrun, long interval, long repeatcount = 0)
        {
            Status = status.ok;
            C = c;
            thingtodo = payload;
            //this.s = s;

            this.name = name;
            this.interval = interval;
            this.nextrun = nextrun;
            this.repeatcount = repeatcount;
        }
        public job(string name, job_delegate payload, ref asset c,session s, long nextrun, long interval, long repeatcount = 0)
        {
            Status = status.ok;
            C = c;
            thingtodo = payload;
            this.s = s;

            this.name = name;
            this.interval = interval;
            this.nextrun = nextrun;
            this.repeatcount = repeatcount;
        }
        public job() { }
    }
}
