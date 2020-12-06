using System;
using Urdr;
using Urdr.Packet;

namespace ConsoleGui
{    
    class Program
    {
        
        static void Main(string[] args)
        {
            server urd=null;
            
            try
            {
                urd = new server();               
                urd.start();
                //setup_logs();
                
            }
            catch (Exception e) { Console.Write(e.Message); }
            finally
            {
                if (urd != null)
                    urd.stop();
            }
            Console.ReadKey();
        }

        //private static void setup_logs() 
        //{
        //    foreach (Log l in Log.Logs) 
        //    {
        //        l.Changed += l_Changed;
        //    }
        //}

        //static void l_Changed(string last)
        //{
        //    Console.WriteLine(last);
        //}

    }
}
