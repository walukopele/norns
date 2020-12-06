using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace verdandi
{
    public class identity : cryptor
    {
        public identity(int rsakeysize=512,int aeskeysize=128) : base(rsakeysize,aeskeysize) { }

        public string self_privatekeyinfo
        {
            get
            {
                return b62.ToB(self_private_key);
            }
            set
            {
                self_private_key= b62.FromB(value);
            }
        }
        public string self_publickeyinfo
        {
            get
            {
                return b62.ToB(self_public_key);
            }            
        }
        
        public string encrypt_auth_token(int random)
        {
            return
              b62.ToB(
                    rsa_encrypt(                    
                        BitConverter.GetBytes(
                            random)));
        }
        public int decrypt_auth_token(string got)
        {
            return
                BitConverter.ToInt32(
                    rsa_decrypt(
                        b62.FromB(got)),0);
        }

        public byte[] encrypt_auth_info(byte[] info)
        {
            return rsa_encrypt(info);
        }

        public byte[] decrypt_auth_info(byte[] got)
        {
            return rsa_decrypt(got);
        }
    }
}
