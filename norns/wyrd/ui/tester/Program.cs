using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;

namespace wyrd.Headless
{
    class Program
    {
        static int ccount = 1;
        static List<client> cl = new List<client>();
        static string ip_s;
        static string time;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string _ip = args[0];

                time = DateTime.UtcNow.Ticks.ToString();

                IPAddress ip;
                if (IPAddress.TryParse(_ip, out ip))
                {
                    ip_s = _ip;

                    if (args.Length > 1)
                        int.TryParse(args[1], out ccount);

                    //ip_s = "192.168.0.19";
                    //ccount = 500;

                    while (cl.Count < ccount)
                    {
                        client c = new client();
                        c.on_info += C_on_info;
                        c.Connect(ip_s);
                        c.afterconnect += C_afterconnect;
                        cl.Add(c);
                    }
                }
                else
                    Console.WriteLine("wrong ip: " + _ip);

            }
            else
                Console.WriteLine("you need specify ip");
            Console.ReadKey();
        }

        private static void C_afterconnect(client c, string what)
        {            
            //c.Send("server", "ping");            
        }

        private static void C_on_info(client c, string what)
        {            
            Console.WriteLine(what);
        }
    }
}
