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
using System.Security.Cryptography;

namespace verdandi
{
    public class cryptor
    {
        //public byte[] remote_public_rsa_key = null;
        //public byte[] self_public_rsa_key = null;
        //public byte[] aes_key = null;
        //public byte[] aes_vector = null;

        //public bool enabled { get; private set; } = false;
        //public exchanger.ExCallback completed { get; set; }

        protected AesCryptoServiceProvider aes;
        ICryptoTransform decryptor;
        ICryptoTransform encryptor;
        protected RSACryptoServiceProvider rsa;

        public byte[] self_public_key { get { return rsa.ExportCspBlob(false); } }

        public byte[] crypt_aes_key(byte[] what, byte[] remote_public_key)
        {
            rsa.ImportCspBlob(remote_public_key);
            return rsa.Encrypt(what, false);
        }
        public byte[] decrypt_aes_key(byte[] what)//private key stored locally
        {
            byte[] vs = new byte[64];
            Array.Copy(what, vs, 64);
            return rsa.Decrypt(vs, false);
        }
        //aes
        public void init_aes()
        {
            aes = new AesCryptoServiceProvider();
            aes.KeySize = 128;
            aes.GenerateIV();
            aes.GenerateKey();
            aes.Padding = PaddingMode.Zeros;
        }
        public byte[] get_aes_vector()
        {
            return aes.IV;
        }
        public byte[] get_aes_key()
        {
            return aes.Key;
        }
        public byte[] get_aes_cryto_phrase()
        {
            byte[] key = this.get_aes_key();
            byte[] vector = this.get_aes_vector();
            int keysize = key.Length;
            int vectorsize = vector.Length;
            int intsize = 4;//sizeof int
            byte[] ret = new byte[intsize + keysize + intsize + vectorsize];

            //copy keysize at 0;
            Array.Copy(BitConverter.GetBytes(keysize), ret, intsize);
            //copy key at 0+4
            Array.Copy(key, 0, ret, intsize, keysize);
            //copy vector size at 0+4+32=36;
            Array.Copy(BitConverter.GetBytes(vectorsize), 0, ret, intsize + keysize, intsize);
            //copy vector
            Array.Copy(vector, 0, ret, intsize + keysize + intsize, vectorsize);

            return ret;
        }

        public void install_aes_crypto_phrase(byte[] rawinput)
        {
            int intsize = 4;//first int
            int key_size = BitConverter.ToInt32(rawinput, 0);
            byte[] key = new byte[key_size];
            Array.Copy(rawinput, intsize, key, 0, key_size);
            int vectorsize = BitConverter.ToInt32(rawinput, intsize + key_size);
            byte[] vector = new byte[vectorsize];
            Array.Copy(rawinput, intsize + key_size + intsize, vector, 0, vectorsize);
            setup_aes(key, vector);
        }
        private void setup_aes(byte[] key, byte[] vector)
        {
            aes.Key = key;
            aes.IV = vector;
            aes.Padding = PaddingMode.Zeros;
        }

        public byte[] encrypt(byte[] data)
        {
            encryptor = aes.CreateEncryptor();
            int length = data.Length;

            int blocksize = encryptor.InputBlockSize;
            int chunksize = encryptor.OutputBlockSize;

            int chunkcount = length / chunksize;

            int tail = length % chunksize;

            if (tail > 0) chunkcount += 1;

            byte[] ret = new byte[chunkcount*chunksize];

            byte[] chunk = new byte[encryptor.OutputBlockSize];
            for (int counter = 0; counter < length; counter = counter + blocksize)
            {
                if (counter + blocksize < length)
                    encryptor.TransformBlock(data, counter, blocksize, chunk, 0);
                else chunk = encryptor.TransformFinalBlock(data, counter, length - counter);                
                Buffer.BlockCopy(chunk, 0, ret, counter, chunksize);
            }
            return ret;
        }
        public byte[] decrypt(byte[] data)
        {
            decryptor = aes.CreateDecryptor();
            int length = data.Length;

            int blocksize = decryptor.InputBlockSize;
            int chunksize = decryptor.OutputBlockSize;
            //
            int chunkcount = length / chunksize;
            //int tail = length % chunksize;

            //if (tail > 0) chunkcount += 1;

            byte[] ret = new byte[chunkcount * chunksize];


            byte[] chunk = new byte[decryptor.OutputBlockSize];
            for (int counter = 0; counter < length; counter = counter + blocksize)
            {
                if (counter + blocksize < length)
                    decryptor.TransformBlock(data, counter, blocksize, chunk, 0);
                else chunk = decryptor.TransformFinalBlock(data, counter, length - counter);
                Buffer.BlockCopy(chunk, 0, ret, counter, chunksize);                
            }

            return ret;
        }

        public cryptor(int size)
        {
            rsa = new RSACryptoServiceProvider(size);
            aes = new AesCryptoServiceProvider();
            aes.KeySize = 128;
        }
    }
}
