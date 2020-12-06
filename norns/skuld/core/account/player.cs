using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skuld
{
    public class player
    {
        public struct info
        {
            [cache]public long area_uid;
            [cache]public long self_uid;
            [cache]public List<long> owned_uids;
        }
        [cache] public List<info> playerinfo = new List<info>();
        public player() { }
    }
}
