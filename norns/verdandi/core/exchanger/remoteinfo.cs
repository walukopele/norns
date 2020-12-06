using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace verdandi
{
    public class remoteinfo
    {
        public int idx { get; set; }
        public ushort connection_uid { get; set; }
        public object session { get; set; } = null;

        private Socket remotesocket;

        private List<packet> receive = new List<packet>();
        private List<packet> todeliver = new List<packet>();
        
        private byte[] agot = new byte[packet.maxpacketsize];

        private readonly object locker_r = new object();
        private readonly object locker_w = new object();

        private cryptor crypto;
        private long init_time = DateTime.UtcNow.Ticks;
        private long waitticks = TimeSpan.TicksPerMillisecond * 16;
        private long lastwrite = DateTime.UtcNow.Ticks;
        private bool writeing = false;
        private SocketAsyncEventArgs wa = null;

        public string remotename { get; private set; }
        public string endpoint { get { if (remotesocket.Connected) return remotesocket.RemoteEndPoint.ToString(); else return ""; } }
        public long lastactive { get; private set; } = 0;
        public int pps { get; private set; } = 0;
        public int bytesWritePerSec { get; private set; } = 0;
        private int currentpps = 0;
        private int currentBytesWrite = 0;

        public bool server_side { get; private set; } = false;
        public bool dead { get; private set; } = false;
        public string lasterror { get; private set; } = "";

        public remoteinfo(ref Socket remotesocket, string name, bool server_side = false)
        {
            this.server_side = server_side;
            this.lastactive = DateTime.UtcNow.Ticks;

            this.remotesocket = remotesocket;
            this.remotesocket.SendBufferSize = packet.maxpacketsize;
            this.remotesocket.ReceiveBufferSize = packet.maxpacketsize;

            this.idx = -1;

            this.connection_uid = 0;
            this.remotename = name;
        }

        //private exchanger.ExCallback onsetup_success;
        public exchanger.ExCallback onsetup { get; private set; }
        public exchanger.ExCallback ondead { get; set; }
        public exchanger.ExCallback onreceive { get; set; }
        public void setup(exchanger.ExCallback onsetup)
        {
            this.onsetup = onsetup;

            if (server_side)
            //client starts cryprosetup
            {
                crypto = new cryptor(2048);
                byte[] got = new byte[2048];
                remotesocket.Blocking = true;
                //got client pub
                remotesocket.Receive(got);
                //crypto.remote_public_rsa_key = got;
                //encrypt aes with client pub
                crypto.init_aes();
                byte[] aes = crypto.crypt_aes_key(crypto.get_aes_cryto_phrase(), got);
                remotesocket.Send(aes);
                remotesocket.Blocking = false;
                onsetup?.Invoke(this);
            }
            else
            {
                remotesocket.Blocking = true;
                //send client pub
                remotesocket.Send(crypto.self_public_key);

                byte[] got = new byte[2048];
                //got crypted aes 
                remotesocket.Receive(got);

                byte[] aes = crypto.decrypt_aes_key(got);

                crypto.install_aes_crypto_phrase(aes);

                remotesocket.Blocking = false;
                //SERVER setupcompete                
                onsetup?.Invoke(this);
            }
        }

        public void connect(IPAddress ip, int port, exchanger.ExCallback onsetup_success, cryptor identity = null)
        { 
            if (identity == null)
                crypto = new cryptor(512);
            else crypto = identity;

            remotesocket.Blocking = true;
            remotesocket.Connect(new IPEndPoint(ip, port));
            remotesocket.Blocking = false;
            setup(onsetup_success);
        }     

        public void startwrite()
        {
            if (wa == null)
            {
                wa = new SocketAsyncEventArgs();
                wa.Completed += wA_Completed;
            }
            long now = DateTime.UtcNow.Ticks;

            if (now - lastwrite > waitticks&&!writeing)
            {
                lastwrite = now;
                
                List<packet> temp = new List<packet>();
                
                lock (locker_w)
                {
                    temp.AddRange(todeliver);
                    todeliver.Clear();
                }

                if (temp.Count > 0)
                {
                    int bytestotal = 0;
                    foreach (packet p in temp)
                        bytestotal += p.size;

                    byte[] tosend = new byte[bytestotal];

                    int offset = 0;
                    foreach (packet p in temp)
                    {
                        ushort size = p.size;
                        Buffer.BlockCopy(p.Bytes, 0, tosend, offset, size);
                        offset += size;
                    }

                    tosend = crypto.encrypt(tosend);                    

                    try
                    {
                        writeing = true;
                        wa.SetBuffer(tosend, 0, tosend.Length);
                        remotesocket?.SendAsync(wa);
                    }
                    catch(Exception exc)
                    {
                        pps = 0;
                        bytesWritePerSec = 0;
                        this.dead = true;
                        lasterror = exc.Message;
                        ondead?.Invoke(this);
                    }
                }
            }
        }
        private void wA_Completed(object sender, SocketAsyncEventArgs e)
        {

            currentBytesWrite += e.BytesTransferred;

            writeing = false;

            long now = DateTime.UtcNow.Ticks;
            currentpps++;
            
            if (now - init_time > TimeSpan.TicksPerSecond)
            {
                init_time = now;

                pps = currentpps;
                currentpps = 0;

                bytesWritePerSec = currentBytesWrite;
                currentBytesWrite = 0;
            }

        }  

        public void startread()
        {
            SocketAsyncEventArgs ra = new SocketAsyncEventArgs();
            ra.Completed += rA_Completed;
            ra.SetBuffer(agot, 0, packet.maxpacketsize);
            ra.UserToken = this;
            remotesocket.ReceiveAsync(ra);
        }
        private void rA_Completed(object sender, SocketAsyncEventArgs e)
        {                      
            remoteinfo token = (remoteinfo)e.UserToken;
            int count = e.BytesTransferred;
            try
            {
                if (count > 0 && e.SocketError == SocketError.Success)
                {
                    byte[] temp = new byte[count];

                    Buffer.BlockCopy(agot, 0, temp, 0, count);

                    temp = crypto.decrypt(temp);

                    List<packet> packets = packet.Extract(temp);
                                        
                    lock (locker_r)
                    {
                        receive.Clear();
                        receive.AddRange(packets);
                    }
                    lastactive = DateTime.UtcNow.Ticks;

                    onreceive(this);
                }
                token.remotesocket?.ReceiveAsync(e);
            }
            catch(Exception exc)
            {
                pps = 0;
                bytesWritePerSec = 0;
                this.dead = true;
                lasterror = exc.Message;
                ondead?.Invoke(this);
            }            
        }        

        public void close()
        {
            
            if (remotesocket != null)
            {
                remotesocket.Close();
            }
        }

        public packet[] Get
        {
            get
            {
                List<packet> ret = new List<packet>();  
                lock(locker_r)
                    if (receive.Count > 0)
                        ret.AddRange(receive);
                return ret.ToArray();
            }
        }
        public void Add(packet p)
        {
            lock (locker_w)
            {
                todeliver.Add(p);
            }               
        }
    }
}
