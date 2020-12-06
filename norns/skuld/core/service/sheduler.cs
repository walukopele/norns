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
using System.Threading;
using verdandi;
using System.IO;



namespace skuld
{
    
    /// <summary>
    /// TODO: major redo after first working client example
    /// </summary>
    public class sheduler
    { 
        Thread fastthread;
        Thread slowthread;
        object fastsync = new object();
        object slowsync = new object();
        List<job> fastjobs;
        List<job> slowjobs;

        static
            Dictionary<string, job_delegate> sheds = new Dictionary<string, job_delegate>();
             

        asset data;
        long ticks_interval_to_assume_job_slow;
        bool working = false; 
        string Name="";
        Log log;

        public sheduler(string Name, ref asset Workercache)
        {
            this.Name = Name;
            //
            data = Workercache;
            log = data.log;
            data.Name = Name;
            load(ref data, null);
            data.Name = Name;
            Workercache = data;
            //

            working = true;
            ticks_interval_to_assume_job_slow = TimeSpan.TicksPerSecond;

            fastjobs = new List<job>();
            slowjobs = data.slowjobs;

            //selfdump = new job(Name + " dump", dump, ref data, DateTime.UtcNow, new TimeSpan(0, 0, 30), long.MaxValue);

            hddiosheduler.Instance().AddJob(Name + " dump", dump, ref data);


            fastthread = new Thread(fastprocess);
            fastthread.Name = Name + " FAST shd";

            slowthread = new Thread(slowprocess);
            slowthread.Name = Name + " SLOW shd";
        }

        public void start()
        {            
            fastthread.Start(working);
            slowthread.Start(working);
        }


            ///TODO undo and incremental save|load
            ///
        private void dump(ref asset c, job self,ref session s)
        {
            if (!c.dumping)
            {
                c.dumping = true;
                string path = Path.Combine("cache", c.Name + ".actual");
                //log.Add(Name+" serialize start");
                //log.Watch();
                //skuld.cache temp =  

                if (File.Exists(path))
                {
                    //string datetime = DateTime.UtcNow.Year + "." + DateTime.UtcNow.Month + "." + DateTime.UtcNow.Day;
                    string path2 = Path.Combine("cache", c.Name + ".backup");
                    if (!File.Exists(path2))
                        File.Move(path, path2);
                    else
                    {
                        File.Delete(path2);
                        File.Move(path, path2);
                    }
                }

                try
                {
                    if (!File.Exists(path))
                        File.Delete(path);
                    using (StreamWriter writer = File.CreateText(path))
                        c.Dump(writer);
                }
                catch (Exception exc) { log.Add(c.Name + " dumpata error: ", exc); }

                //Log.defaultlog.Add(c.Name + " serialize end");
                //File.WriteAllText(
                //    Path.Combine("cache",Name + ".lua") ,
                //      s
                //Newtonsoft.Json.JsonConvert.SerializeObject(cache)
                //     );

                //cache.Save();
                c.dumping = false;
            }
        }
        private void load(ref asset c, job self)
        {
            string path = Path.Combine("cache", c.Name + ".actual");
            FileInfo info = new FileInfo(path);

            if (info.Exists)
            {
                if (info.Length > 0)
                {
                    object o = c;
                    string s = File.ReadAllText(path);

                    //todo: change to full file reading as stream for big files
                    serialisator j = new serialisator(datatype.cache);

                    j.deserialize(s, ref o, datatype.cache);


                    c = (asset)o;
                }
                else
                {
                    path = Path.Combine("cache", c.Name + "0.backup");
                    info = new FileInfo(path);
                    if (info.Exists)
                    {
                        if (info.Length > 0)
                        {
                            object o = c;
                            string s = File.ReadAllText(path);

                            //todo: change to full file reading as stream for big files
                            serialisator j = new serialisator(datatype.cache);
                            try
                            {
                                j.deserialize(s, ref o, datatype.cache);
                            }
                            catch (Exception e) { log.Add(c.Name, e); }
                            c = (asset)o;
                        }
                        else
                            log.Add(c.Name + " backup broken");
                    }
                    else log.Add(c.Name + " no file to load");
                }
            }
            else
            {
                path = Path.Combine("cache", c.Name + "0.backup");
                info = new FileInfo(path);
                if (info.Exists)
                {
                    if (info.Length > 0)
                    {
                        object o = c;
                        string s = File.ReadAllText(path);

                        //todo: change to full file reading as stream for big files
                        serialisator j = new serialisator(datatype.cache);
                        try
                        {
                            j.deserialize(s, ref o, datatype.cache);
                        }
                        catch (Exception e) { log.Add(c.Name, e); }
                        c = (asset)o;
                    }
                    else
                        log.Add(c.Name + " backup broken");
                }
                else log.Add(c.Name + " no file to load");
            }
        }


        //todo remake with fresh head
        //i think the only thing it need to save is slow thread. something like cache backups or jobs with significant delay|interval
        //public void Save() { }
        //   public void Load() { }

        private void fastprocess(object o)
        {
            while (working)
            {
                long now = DateTime.UtcNow.Ticks;

                for (int i = fastjobs.Count - 1; i >= 0; i--)
                {
                    job work = null;
                    lock (fastsync)
                        if (i < fastjobs.Count)
                            work = fastjobs[i];

                    if (work != null)
                         if (work.Do(now))//job done so it will deleted
                        {
                            lock (fastsync)
                                fastjobs.RemoveAt(i);
                            continue;
                        }

                }
                Thread.Sleep(8);
            }
        }
        private void slowprocess(object o)
        {
            while (working)
            {
                long now = DateTime.UtcNow.Ticks;
                for (int i = slowjobs.Count - 1; i >= 0; i--)
                {
                    lock (slowsync)
                        if (slowjobs[i].Do(now))//job done so it will deleted
                        {                        
                            slowjobs.RemoveAt(i);
                            continue;
                        }                        
                    
                }
                Thread.Sleep(200);               
            }
        }

        private void choose_right_jobthread(job test, long limiter, bool unique)
        {
            if (test.IsFast(limiter))
            {
                if (unique)
                {
                    if (!fastjobs.Contains(test))
                        lock (fastsync)
                            fastjobs.Add(test);
                }
                else
                    lock (fastsync)
                        fastjobs.Add(test);
            }
            else
            {
                if (unique)
                {
                    if (!slowjobs.Contains(test))
                        lock (slowsync)
                            slowjobs.Add(test);
                }
                else
                    lock (slowsync)
                        slowjobs.Add(test);                
            }
        }

        public void Reg(string jobname, job_delegate work)
        {
            if (!sheds.ContainsKey(jobname))
                sheds.Add(jobname, work);
        }

        public void Run(string registered_jobname,  session s, TimeSpan interval, long repeatcount = 0)//job task)
        {
            this.Run(registered_jobname, s,false, 0, interval.Ticks, repeatcount);
        }
        public void Run(string registered_jobname,  session s,  TimeSpan startdelay, TimeSpan interval, long repeatcount = 0)//job task)
        {
            DateTime time = new DateTime(DateTime.UtcNow.Ticks + startdelay.Ticks);
            this.Run(registered_jobname,  s, false, time.Ticks, interval.Ticks, repeatcount);
        }        
        public void Run(string registered_jobname,  session s,  DateTime nextrun, TimeSpan interval, long repeatcount = long.MaxValue)//job task)
        {
            Run(registered_jobname,  s, false, nextrun.Ticks, interval.Ticks, repeatcount);
        }
        /// <summary>
        /// does not start anything if any exists
        /// </summary>
        /// <param name="unique"></param>
        /// <param name="registered_jobname"></param>
        /// <param name="nextrun"></param>
        /// <param name="interval"></param>
        /// <param name="repeatcount"></param>
        public void Run(string registered_jobname, session s, bool unique = false, long nextrun = 0, long interval = 0, long repeatcount = long.MaxValue)//job task)
        {
            if (sheds.ContainsKey(registered_jobname))
            {
                job_delegate jd = sheds[registered_jobname];

                job temp = new job(registered_jobname, jd, ref data, s, nextrun, interval, repeatcount);
                choose_right_jobthread(temp, ticks_interval_to_assume_job_slow, unique);
            }
        }

        /// <summary>
        /// todo:
        /// </summary>
        /// <param name="registered_jobname"></param>
        /// <param name="force"></param>
        public void Cancel(string registered_jobname, bool force)
        {
            if (force)
            {
                job_delegate test =
                sheds[registered_jobname];
                if (test != null)
                {
                    lock (slowsync)
                    {
                        int idx = slowjobs.FindIndex(x => x.name == registered_jobname);
                        if (idx != -1)
                        {
                            slowjobs[idx].Status = job.status.done;
                        }
                    }
                    lock (fastsync)
                    {
                        int idx = fastjobs.FindIndex(x => x.name == registered_jobname);
                        if (idx != -1)
                        {
                            fastjobs[idx].Status = job.status.done;
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            lock (fastsync)
                fastjobs.Clear();
        }

        public void Close()
        {            
            working = false;
            Clear();
            //dump(ref data, null);
            hddiosheduler.Instance().Remove(Name + " dump");           
        }


    }
}
