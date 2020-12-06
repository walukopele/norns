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

namespace skuld
{
    public enum datatype
    {
        undefined = 0,
        cache = 1 << 0,
        template = 1 << 1,
        usedefaults = 1 << 2,
        owned_property = 1<<3
    }
    /// <summary>
    /// serializable attribute to use with custom asset serializator. mark field to use. 
    /// </summary>
    public class data : System.Attribute
    {
        public datatype dt { get; private set; }
        public data() {dt |= datatype.usedefaults; }
        public data(datatype dt) { this.dt |= dt; }
    }
    /// <summary>
    /// serializable attribute to use with custom asset serializator. mark PROPERTY to use. 
    /// </summary>
    public sealed class template : data
    {
        public template() : base(datatype.template) { }
        public template(bool usedefaults) : base(datatype.template | datatype.usedefaults) { }
    }
    /// <summary>
    /// serializable attribute to use with custom asset serializator. mark PROPERTY to use. 
    /// </summary>
    public sealed class cache : data
    {
        public cache() : base(datatype.cache) { }
        public cache(bool usedefaults) : base(datatype.cache | datatype.usedefaults) { }
    }
    /// <summary>
    /// serializable attribute to use with custom asset serializator. mark PROPERTY to use. 
    /// </summary>
    public sealed class omni : data
    {
        public omni() : base(datatype.template | datatype.cache){ }
    }

}
