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
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace verdandi
{
    //public struct 
    /// <summary>
    ///todo: crc compression etc
    /// client uid 
    /// </summary>
    public class packet
    {
        public static List<packet> Extract(byte[] inbuffer)
        {
            List<packet> ret = new List<packet>();

            int position = 0;

            if (inbuffer.Length >= headeroffset)
            {
                ushort size = 0;
                //todo breakpoint here
                do
                {
                    size = BitConverter.ToUInt16(inbuffer, position);

                    if (size >= headeroffset)
                    {
                        ret.Add(new packet(inbuffer, position, size));
                        position += size;
                    }
                }
                while (size >= headeroffset && position + headeroffset <= inbuffer.Length);
            }
            return ret;
        }

        public const byte error = 255;
        public const int maxpacketsize = ushort.MaxValue;
        public const int headeroffset = 4;///target,arg,size
        public const int offset_target = 0 + sizeof(short);
        public const int offset_remote_command = 0 + sizeof(short) + sizeof(byte);       

        public object internal_message { get; private set; }
        public bool is_internal { get; private set; }
        public byte target
        {
            get { return buffer[offset_target];  }
            set { buffer[offset_target] = value; }
        }
        public byte remote_command
        {
            get { return buffer[offset_remote_command]; }
            set { buffer[offset_remote_command] = value; }
        }   

        public ushort size { get { return write_counter; } }

        ushort write_counter=headeroffset;       
        ushort read_counter = headeroffset;

        byte[] buffer=new byte[headeroffset];        

        public packet(byte[] recieved,int startpos=0, ushort size=headeroffset)
        {
            internal_message = null;
            is_internal = false;
            buffer = new byte[size];
            write_counter = size;

            int length = recieved.Length;
            if (length >= size + startpos)
                Buffer.BlockCopy(recieved, startpos, buffer, 0, size);                              
        }
        public packet(byte target, byte remote_command, string someinfo8)
        {
            internal_message = null;
            is_internal = false;
            this.remote_command = remote_command;
            this.target = target;
            Write(someinfo8);
        }
        public packet(packet p, string someinfo8):this(p.target,p.remote_command,someinfo8){}
        public packet(byte target, byte remote_command, byte[] rawdata=null)
        {
            internal_message = null;
            is_internal = false;

            this.remote_command = remote_command;
            this.target = target;

            if (rawdata != null)
                addbytes(rawdata);
        }
        public packet(packet p, byte[] rawdata=null) : this(p.target, p.remote_command, rawdata) { }        
        public packet(ref object internal_message, byte target, byte remote_command)
        {
            buffer = null;           
            read_counter = 0;
            this.internal_message = internal_message;
            is_internal = true;
        }
     
        #region write     
        public void Write(string w)
        {
            this.addbytes(Encoding.UTF8.GetBytes(w));
        }
        public void Write(float w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(double w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(byte w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(sbyte w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(short w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(ushort w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(int w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(uint w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(long w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        public void Write(ulong w)
        {
            this.addbytes(BitConverter.GetBytes(w));
        }
        

        private void addbytes(byte[] bytes)
        {
            int bylength = bytes.Length;
            if (bylength + write_counter <= buffer.Length)
            {
                Array.Copy(bytes, 0, buffer, write_counter, bylength);
                write_counter = (ushort)(write_counter + bylength);
            }
            else
            {
                
                byte[] newbuf = new byte[buffer.Length * 2 + bylength];

                Buffer.BlockCopy(buffer, 0, newbuf, 0, buffer.Length);
                Array.Copy(bytes, 0, newbuf, write_counter, bylength);
               
                buffer = newbuf;
                write_counter = (ushort)(write_counter + bylength);
            }
        }      

        #endregion
        #region read
        public float ReadF()
        {
            byte c = (byte)(read_counter + sizeof(float));
            float ret=0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToSingle(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public double ReadD()
        {
            byte c = (byte)(read_counter + sizeof(double));
            double ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToDouble(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public int ReadSI()
        {
            byte c = (byte)(read_counter + sizeof(int));
            int ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToInt32(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public uint ReadUI()
        {
            byte c = (byte)(read_counter + sizeof(uint));
            uint ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToUInt32(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public long ReadSL()
        {
            byte c = (byte)(read_counter + sizeof(long));
            
            long ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToInt64(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public ulong ReadUL()
        {
            byte c = (byte)(read_counter + sizeof(ulong));

            ulong ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToUInt64(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public short ReadSS()
        {
            byte c = (byte)(read_counter + (byte)sizeof(short));
            short ret=0;
            if (c <= buffer.Length)
            {                
                ret = BitConverter.ToInt16(buffer, read_counter);
                read_counter = c;
            } 
            return ret;
        }
        public ushort ReadUS()
        {
            byte c = (byte)(read_counter + (byte)sizeof(ushort));
            ushort ret = 0;
            if (c <= buffer.Length)
            {
                ret = BitConverter.ToUInt16(buffer, read_counter);
                read_counter = c;
            }
            return ret;
        }
        public byte ReadUB()
        {
            byte c = (byte)(read_counter + (byte)sizeof(byte));
            byte ret = 0;
            if (c <= buffer.Length)
            {                
                ret = buffer[read_counter];
                read_counter = c;
            }
            return ret;
        }
        public sbyte ReadSB()
        {
            byte c = (byte)(read_counter + (byte)sizeof(sbyte));
            sbyte ret = 0;
            if (c <= buffer.Length)
            {
                ret = (sbyte)buffer[read_counter];
                read_counter = c;
            }
            return ret;
        }
        #endregion
        public byte[] Bytes
        {
            get
            {
                Array.Copy(BitConverter.GetBytes(write_counter), buffer, 2);
                return buffer;                
            }
        }      
   
        public string String
        {
            get
            {
                if (write_counter >= read_counter)
                    return Encoding.UTF8.GetString(buffer, read_counter, write_counter - read_counter);
                else return "";
            }
        }
       

        
        
       

     



    }

    
  

  
   



}
