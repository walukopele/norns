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
using System.Net;
using verdandi;

namespace wyrd
{
    //
    /// <summary>
    /// 
    ///туду: кидает спящий коннект по мере продвижения игрока в мире от шарда к шарду. 
    ///туду: постепенно опрашивает настоящий сервер и когда приближается к границам шарда 
    ///туду:  переключается с одного коннекта на другой.
    /// 
    ///todo протокол подключения клента коннект-клиент посылает открытый ключ- сервер принимает ключ клиента посылает свой открытый ключ + 
    ///todo клиент требует листинг комманд - сервер шлет листинг - клиент требует список менеджеров - сервер шлет список.+

    /// </summary>
    class client_network
    {
        public IPAddress IP { get; private set; }
        public remoteinfo current { get; private set; }
        public bool Connected { get; private set; }
        public event exchanger.LogInfo on_info;
        public exchanger.ExCallback ondisconnected { get;  set; }
        //public exchanger.ExCallback ongotdelivery { get; set; }

        private void dummy(string what) { }
        private int CURRENT_SERVER_IDX;
        private exchanger ex;  

        public void Send(packet p)
        {
            current.Add(p);
        }

        public int pps { get { return ex.PPS; } }



        public client_network()
        {
            Connected = false;
            CURRENT_SERVER_IDX = 0;
            ex = new exchanger(1);
            ex.Connected = ex_On_ConnectedTo;
            ex.On_Info += Ex_On_Info;
        }

        private void Ex_On_Info(string what)
        {
            on_info(what);
        }

        public void Connect(string serverIp, ushort port)
        {           
            on_info("connecting to server...");
            //todo rewrite for multiconnect here
            IPAddress ip;
            if (!IPAddress.TryParse(serverIp, out ip))
            {
                on_info("wrong ip");
                return;
            }

            IP = ip;

            ex.Connect("server", ip, port);
        }
        public void AddHandler(exchanger.ExCallback e)
        {
            ex.Received = e;
        }
        public void Disconnect()
        {
            ex.Close();
            Connected = false;
        }
        public bool Ready
        {
            get { return current != null; }
        }

        void ex_On_ConnectedTo(remoteinfo c)
        {
            if (!Connected)
            {
                Connected = true;
                c.ondead = ondisconnected;
                CURRENT_SERVER_IDX = c.idx;
                current = c;

                //every time we connects to new remote
                c.Add(
                    new packet(
                        0, 0, //started setup here 0 is internalport of server service and 0 is setup command
                        BitConverter.GetBytes(
                            new Random().NextDouble()
                            )
                            )
                            );//bootstrap 
                on_info("connected to known remote: " + c.idx);
            }
        }
    }
}
