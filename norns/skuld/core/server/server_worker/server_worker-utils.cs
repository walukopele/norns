using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verdandi;

namespace skuld
{
    partial class server_worker:worker
    {
        #region randomutils
        private bool check_name_is_valid(string name)
        {
            return name != "" && name != "admin";
        }
        #endregion
    }
}
