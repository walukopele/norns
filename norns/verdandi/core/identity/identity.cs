using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace verdandi
{
    /// <summary>
    /// auth system part. 
    /// possible usage:
    /// bind clientip and public key on server so next time you login as youself
    /// when cryptor establishes cryptothings
    /// </summary>
    public class identity:cryptor
    {
        public string privatekeyinfo
        {
            get
            {
                return Encoding.UTF8.GetString(rsa.ExportCspBlob(true));
            }
            set
            {
                try
                {
                    rsa.ImportCspBlob(Encoding.UTF8.GetBytes(value));
                }
                catch 
                {

                }
            }
        }
      
        public identity(int size) : base(size) { }

    }
}
