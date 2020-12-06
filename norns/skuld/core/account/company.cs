using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verdandi;

namespace skuld
{
    public class company
    {
        //identity id = new identity(512, 256);
        [cache]string licence;
        [cache]string name;
        public class role
        {
            string name;
            int account_localid;
        }
    }
}
