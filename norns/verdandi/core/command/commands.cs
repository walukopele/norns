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

using System.Collections.Generic;
using System;

namespace verdandi
{
    
    public class commands
    {
        public delegate packet ServerWork(packet p, object session);
        public delegate void ClientWork(packet p, string format_toclient);
        public struct command
        {
            static void dummyc(packet p) { return; }

            ClientWork cwork;
            ServerWork swork;
            privilege prv;

            public ClientWork unhandled { get; set; }

            public byte target;//1
            public byte remote_command;//1

            public string servicename;//16
            public string commandname;//16
            public string formattoserver;//16
            public string formattoclient;//16
            public string version;//16
            public string servicetype;//16
            //public string description;

            public bool linked;
            public bool handled;

            public privilege Prw()
            {
                return prv;
            }

            public command(string command_name, string format_toserver, string format_toclient, privilege p = privilege.administrator, ServerWork w = null)
            {
                target = remote_command = packet.err;
                version = "";
                servicename = "";
                servicetype = "";
               // description = "";

                formattoserver = format_toserver;
                formattoclient = format_toclient;

                linked = true;//server is linked by default
                handled = true;
                swork = w;

                commandname = command_name;

                prv = p;
                cwork = null;
                unhandled = null;
            }
            public command(string servicename, string commandname, string format_toserver = "", string format_toclient = "", string servicetype = "", string version = "", ClientWork w = null)
            {
                linked = false;//client is unlinked by default
                handled = false;

                target = remote_command = packet.err;

                formattoserver = format_toserver;
                formattoclient = format_toclient;

                swork = null;
                cwork = w;

                this.servicename = servicename;
                this.commandname = commandname;
                this.version = version;
                this.servicetype = servicetype;
               // this.description = description;

                unhandled = null;
                prv = privilege.anonymous;
            }
            public void set_clientwork(ClientWork w)
            {
                this.cwork = w;
            }
            public void set_serverwork(ServerWork w)
            {
                this.swork = w;
            }
            public packet ServerDo(packet p, object session, privilege prv)
            {
                try
                {
                    if (swork == null) return new packet(p.target, packet.err, "unknown command");
                    else
                    {
                        if ((int)this.prv <= (int)prv)
                        {
                            return swork(p, session);
                        }
                        else return new packet(p.target, packet.err,
                            "[" + p.target.ToString() + "/"
                                + p.remote_command.ToString() + "]" +
                            " too low privilege to do it");
                    }
                }
                catch (Exception e) { return new packet(p, e.Message); }
            }
            public void ClientDo(packet p)
            {
                if (handled)
                    cwork(p, formattoclient);
                else unhandled?.Invoke(p, formattoclient);
            }
            public string extract()
            {
                return servicename + "," + commandname + ","/* + description + "," */+ formattoserver + "," + formattoclient + "," + servicetype + "," + version + "," + target.ToString() + "," + remote_command.ToString() + ";";
            }
        }

        List<command> actions = new List<command>();

        public string[] Services
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (command c in actions)
                {
                    if (!ret.Exists(x => x == c.servicename))
                        ret.Add(c.servicename);
                }
                return ret.ToArray();
            }
        }
        public string[] that_services(string servicetype, string version)
        {
            List<command> srv = actions.FindAll(x => x.servicetype == servicetype && x.version == version);
            List<string> ret = new List<string>();
            foreach (command c in srv)
            {
                if (!ret.Exists(x => x == c.servicename))
                    ret.Add(c.servicename);
            }
            return ret.ToArray();
        }
        public string[] Commands(string servicename)
        {
            List<string> ret = new List<string>();
            List<command> srv = actions.FindAll(x => x.servicename == servicename);
            foreach (command c in srv)
            {
                ret.Add(c.commandname);
            }
            return ret.ToArray();
        }

        public string[] Types
        {
            get
            {
                List<string> types = new List<string>();
                foreach (command c in actions)
                {
                    if (!types.Exists(x => x == c.servicetype))
                        types.Add(c.servicetype);
                }
                return types.ToArray();
            }
        }
        public string[] Handled
        {
            get
            {
                List<command> good = actions.FindAll(x => x.handled);
                List<string> names = new List<string>();
                foreach (command c in good)
                {
                    names.Add(c.extract());
                }
                return names.ToArray();
            }
        }
        public string[] UnHandled
        {
            get
            {
                List<command> bad = actions.FindAll(x => !x.handled);
                List<string> names = new List<string>();
                foreach (command c in bad)
                {
                    names.Add(c.extract());
                }
                return names.ToArray();
            }
        }

        public commands(string extracted)
        {
            actions = new List<command>();

            string[] s = extracted.Split(';');
            foreach (string ss in s)
            {
                if (ss != "")
                {
                    string[] sss = ss.Split(',');
                    if (sss.Length >= 8)
                    {
                        if (sss[0] != "" && sss[1] != "")
                        {
                            command com = new command(sss[0], sss[1], sss[2], sss[3], sss[4], sss[5], null);
                            //servicename + "," + 
                            //commandname + "," + 
                            //description + "," + 
                            //formattoserver + "," + 
                            //formattoclient + "," + 
                            //servicetype + "," + 
                            //version + "," + 
                            //target.ToString() + "," + 
                            //remote_command.ToString() + ";";

                            byte sb;
                            byte cb;
                            if (byte.TryParse(sss[6], out sb))
                            {
                                if (byte.TryParse(sss[7], out cb))
                                {
                                    com.target = sb;
                                    com.remote_command = cb;
                                    actions.Add(com);
                                }
                            }
                        }
                    }
                }
            }
        }

        public packet ServerDo(packet p, object session, privilege prv)
        {
            if (p.remote_command < actions.Count) return actions[p.remote_command].ServerDo(p, session, prv);
            else return new packet(p.target, packet.err, "nothing happened");
        }

        public int Set_Unhandled(ClientWork unh, byte target_opcode)
        {
            int result = 0;
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                command c = actions[i];
                if (!c.handled && c.target == target_opcode)
                {
                    c.unhandled = unh;
                    result++;
                }
                actions[i] = c;
            }
            return result;
        }

        public int Link(string extractedfromserver)
        {
            int result = 0;
            //create temp commands from extractedstring
            commands fromserver = new commands(extractedfromserver);
            //use local commands as it was already registered;
            //find in local commands such with a 
            foreach (command c in fromserver.actions)
            {
                int idx =
                this.actions.FindIndex(x => x.commandname == c.commandname && x.servicename == c.servicename);
                if (idx != -1)//update
                {
                    command temp =
                    actions[idx];

                    temp.target = c.target;
                    temp.remote_command = c.remote_command;
                    temp.formattoclient = c.formattoclient;
                    temp.formattoserver = c.formattoserver;
                    temp.version = c.version;
                    temp.servicetype = c.servicetype;
                    //temp.description = c.description;

                    temp.linked = true;

                    actions[idx] = temp;
                    result++;

                }
                else //add new unhandled
                {
                    command temp = new command(c.servicename, c.commandname, "N", "N", null);

                    temp.target = c.target;
                    temp.remote_command = c.remote_command;
                    temp.formattoserver = c.formattoserver;
                    temp.formattoclient = c.formattoclient;
                    temp.version = c.version;
                    temp.servicetype = c.servicetype;
                    //temp.description = c.description;

                    temp.linked = true;
                    temp.handled = false;

                    actions.Add(temp);
                }
            }
            return result;
        }

        public void ClientDo(packet p)
        {
            int idx = actions.FindIndex(x => x.remote_command == p.remote_command && x.target == p.target);
            if (idx != -1) actions[idx].ClientDo(p);
        }

        public command Get_Remote(string servicename, string commandname)
        {
            command ret = new command(servicename, commandname);
            int index =
                    actions.FindIndex(x => x.servicename == servicename && x.commandname == commandname);
            if (index != -1)
            {
                ret = actions[index];
            }
            return ret;
        }

        public string Get_Target(byte target_opcode)
        {
            string ret = "";
            int index =
                    actions.FindIndex(x => x.target == target_opcode);
            if (index != -1)
            {
                ret = actions[index].servicename;
            }
            return ret;
        }

        /// <summary>
        /// регистрация новой команды. 
        /// работает на сервере как обработчик проверок
        /// работает на клиенте как отрисовщик
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="W"></param>
        /// <param name="prv"></param>
        public void RegisterServerCommand(string commandname, string format_toserver, string format_toclient, ServerWork W, privilege prv = privilege.administrator)
        {
            if (actions.Count < packet.err)//last action for error handling
            {
                int idx =
                actions.FindIndex(x => x.commandname == commandname);
                if (idx != -1)
                {
                    command com = actions[idx];
                    com.set_serverwork(W);
                    //com.description = description;
                    actions[idx] = com;//rewrite exiting
                }
                else
                {
                    command com = new command(commandname, format_toserver, format_toclient, prv, W);
                    //com.description = description;
                    com.remote_command = (byte)actions.Count;
                    actions.Add(com);//new}
                }
            }
        }

        public void RegisterClientReaction(string commandname, string servicename, ClientWork w)
        {
            int idx = this.actions.FindIndex(x => x.servicename == servicename && x.commandname == commandname);
            if (idx != -1)//if exits
            {
                command cmd = this.actions[idx];
                cmd.set_clientwork(w);
                cmd.handled = true;
                this.actions[idx] = cmd;
            }
            else
            {
                command cmd = new command(servicename, commandname, "N", "N", "", "", w);
                cmd.set_clientwork(w);
                cmd.handled = true;
                actions.Add(cmd);
            }

        }

        public void RegisterClientReaction(string commandname, string servicename, byte target, byte remote_command, ClientWork w)
        {

            //update if exists
            int idx =
                this.actions.FindIndex(x => x.servicename == servicename && x.commandname == commandname);
            if (idx != -1)
            {
                //update or add
                command com = actions[idx];
                com.remote_command = remote_command;
                com.target = target;
                com.set_clientwork(w);
                com.handled = true;
                com.linked = true;
                this.actions[idx] = com;
            }
            else
            {
                command com = new command(servicename, commandname, "N", "N", "", "", w);
                com.remote_command = remote_command;
                com.target = target;
                com.handled = true;
                com.linked = true;
                actions.Add(com);
            }
        }

        /// <summary>
        /// нужно для передачи списка команд клиенту
        /// передаются только те команды на которые есть права.
        /// </summary>

        /// <returns></returns>
        public string extract(privilege prv)
        {
            string ret = "";
            foreach (command c in actions)
            {
                if (prv >= c.Prw())
                    ret += c.extract();
            }
            return ret;
        }
        public string extract()
        {
            string ret = "";
            foreach (command c in actions)
            {
                ret += c.extract();
            }
            return ret;
        }
        public void settargetname(string name)
        {
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                command c = actions[i];
                c.servicename = name;
                actions[i] = c;
            }
        }
        public void settargetbyte(byte sb)
        {

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                command c = actions[i];
                c.target = sb;
                actions[i] = c;
            }
        }
        public void settargettype(string typename)
        {
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                command c = actions[i];
                c.servicetype = typename;
                actions[i] = c;
            }
        }
        public void settargetversion(string versioninfo)
        {
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                command c = actions[i];
                c.version = versioninfo;
                actions[i] = c;
            }
        }
    }
}
