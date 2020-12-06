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
using verdandi;
using System.Collections.Generic;
using System.IO;

namespace skuld
{
    

    public class asset
    {
              
        [cache] public List<job> slowjobs = new List<job>();

        public object sync = new object();
        public bool dumping = false;
        public string Name = "";
        public Log log;

        private int backup_counter = 0;
        public  int bcount()
        {
            backup_counter++;
            if (backup_counter > 3) backup_counter = 0;
            return backup_counter;
        }

        public asset()
        {

        }       
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>string formatted as json</returns>
        public override string ToString() 
        {
            string ret = "";
            
            try
            {
                ret = new serialisator(datatype.cache).serialize(this);
            }
            catch (Exception e) { log.Add(Name+".cache.tostring",e); }
            
            return ret;
             //   lua_serializator.serialize_object_to_string(this, "", datatype.cache);
        }
        public void Dump(StreamWriter stream)
        {
            new serialisator(datatype.cache).serialize(this, stream);
        }
        

        
    }

}
