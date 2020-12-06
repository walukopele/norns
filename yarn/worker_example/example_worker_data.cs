using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skuld;
using verdandi;

namespace skuld
{
    public class example_worker_data:asset//asset is simple save load impl for handing data
    {
        [cache] public int some_number = 0;

        //default contructor needed for spawning in this impl using reflection so asset has to have one
        public example_worker_data()
        { }
    }
}
