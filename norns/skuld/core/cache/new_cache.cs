using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skuld.cache
{
    class new_cache
    {

        Dictionary<string, object> storage = new Dictionary<string, atomicvalue>();
    }

    class storage_delta//backlog to undo
    {

    }

    class atomicvalue<T>
    {
        long time;
        T val;
        public explicit operator=
    }
}
