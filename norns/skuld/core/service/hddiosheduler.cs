using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace skuld
{
    public class hddiosheduler
    {
        Thread iothread;
        object iosync;
        List<job> iojobs;
        bool ioworking;
        int interval = 1;

        static hddiosheduler instance;

        public static hddiosheduler Instance()
        {
            if (instance == null) instance = new hddiosheduler();
            return instance;
        }
        private hddiosheduler()
        {
            ioworking = true;
            iojobs = new List<job>();
            iosync = new object();
            iothread = new Thread(hddprocess);
            iothread.Name = "Drive IO th";
            iothread.IsBackground = false;
            iothread.Start();
        }
        public void Close()
        {
            ioworking = false;

            lock (iosync)
                foreach (job j in iojobs)
                j.Do(0);            
        }
        public string List()
        {
            string ret = "";
            long now = DateTime.UtcNow.Ticks;
            lock (iosync)
                foreach (job j in iojobs)
                    ret += "\n" + j.name + " nextrun in " + j.TillNextRun(now);

            return ret;
        }

        public void AddJob(string name, job_delegate j, ref asset C)
        {
            lock (iosync)
                iojobs.Add(
                    new job(
                        name, j, ref C,null,
                        DateTime.UtcNow.Ticks,
                        new TimeSpan(0, interval, new Random().Next(30)).Ticks
                        , long.MaxValue));
        }
        public void AddJob(string name, job_delegate j, ref asset C,  session s)
        {
            lock (iosync)
                iojobs.Add(
                    new job(
                        name, j, ref C, s,
                        DateTime.UtcNow.Ticks,
                        new TimeSpan(0, interval, new Random().Next(30)).Ticks
                        , long.MaxValue));
        }
        public void Remove(string name)
        {
            lock (iosync)
                iojobs.Find(x => x.name == name)?.Kill();
        }
        //public void AddJob(job dump)
        //{
        //    lock (iosync)
        //        iojobs.Add(dump);
        //}
        //public void Remove(job dump)
        //{
        //    lock (iosync)
        //        iojobs.Remove(dump);
        //}
        private void hddprocess()
        {
            //bool working = (bool)o;
            while (ioworking)
            {
                //if (iojobs.Count > 0)
                //{
                lock (iosync)
                {
                    long now = DateTime.UtcNow.Ticks;
                    for (int i = iojobs.Count - 1; i >= 0; i--)
                    {

                        job todo = iojobs[i];
                        if (todo.Do(now))//job done so it will deleted
                        {

                            iojobs.RemoveAt(i);
                            i--;

                        }                        
                    }
                    //if (!ioworking) return;
                }
                Thread.Sleep(2000);
            }

            //}
            // else Thread.Sleep(200);
        }
    }
}
