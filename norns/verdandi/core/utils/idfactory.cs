using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace verdandi
{
    public class idfactory
    {
        Random r = new Random(DateTime.UtcNow.Second);
       
        ushort[] ids;

        //public idfactory()
        //{
        //    ids = new ushort[ushort.MaxValue];
        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        ids[i] = 0;
        //    }
        //}
        public idfactory(int maxids=ushort.MaxValue-2)
        {
            if (maxids < ushort.MaxValue)
            {
                ids = new ushort[maxids];
            }
            else ids = new ushort[ushort.MaxValue];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = 0;
            }
        }

        public ushort Asquire()
        {
            ushort num = prepare();
            int idx =
            Array.FindIndex(ids, x => x == 0);
            if (idx != -1)
            {
                ids[idx] = num;
            }
            return num;
        }
        public void Release(ushort id)
        {
            int idx =
            Array.FindIndex(ids, x => x == id);
            if (idx != -1)
            {
                ids[idx] = 0;
            }
        }
        private ushort prepare()
        {
            byte[] buf = new byte[2];

            r.NextBytes(buf);
            ushort num = BitConverter.ToUInt16(buf, 0);

            while (Array.Exists(ids, x => x == num) || num == 0)
            {
                r.NextBytes(buf);
                num = BitConverter.ToUInt16(buf, 0);
            }

            return num;
        }
    }
}
