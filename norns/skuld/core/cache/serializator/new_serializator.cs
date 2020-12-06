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
using System.Collections;
using System.Reflection;
using fastJSON;
using verdandi;

namespace skuld
    {
        public class new_serializator
        {
            private object parcenumber(object number, Type t)
            {
                switch (t.ToString())
                {
                    case "System.Byte": return Byte.Parse(number.ToString());
                    case "System.SByte": return SByte.Parse(number.ToString());
                    case "System.Int16": return Int16.Parse(number.ToString());
                    case "System.Int32": return Int32.Parse(number.ToString());
                    case "System.Int64": return Int64.Parse(number.ToString());
                    case "System.UInt16": return UInt16.Parse(number.ToString());
                    case "System.UInt32": return UInt32.Parse(number.ToString());
                    case "System.UInt64": return UInt64.Parse(number.ToString());
                    case "System.Single": return Single.Parse(number.ToString());
                    case "System.Double": return Double.Parse(number.ToString());
                    default: return 0;
                }
            }
            private static bool checktype(Type T)
            {
                return
                    T == typeof(string) ||
                    T == typeof(bool) ||
                    T == typeof(byte) ||
                    T == typeof(short) ||
                    T == typeof(int) ||
                    T == typeof(long) ||
                    T == typeof(ushort) ||
                    T == typeof(uint) ||
                    T == typeof(ulong) ||
                    T == typeof(float);// ||
                                       //T == typeof(int[]) ||
                                       //T == typeof(float[]);
            }
            datatype sdt = datatype.undefined;
            string name = "";
            public new_serializator(datatype dt, string name = "") { sdt = dt; this.name = name; }
            public string serialize(object o) { return serialize_object_to_string(o, name, sdt); }
            public void deserialize(string str, ref object o, datatype dt)
            {
            object temp = JSON.Parse(str);
            if (temp is IDictionary)
                {


                    Dictionary<string, object> tempdic = (Dictionary<string, object>)temp;
                    deserialize_r(dt, ref o, tempdic);
                }
            }
        public Dictionary<string, object> deserialize(string str)
        {
            //fastJson.JsonParser jp = new fastJson.JsonParser(str);
            object temp = JSON.Parse(str);
            if (temp is IDictionary)
            {
                return (Dictionary<string, object>)temp;
            }
            else return new Dictionary<string, object>();
        }
            private void deserialize_r(datatype dt, ref object o, Dictionary<string, object> parced)
            {
                Type T = o.GetType();




                //if (T.GetInterface("IDictionary") == typeof(IDictionary))
                //{
                //    IDictionary id = (IDictionary)o;

                //    foreach (object idx in id.Keys)
                //    {
                //        Type t2 = T.GenericTyperemote_commands[0];
                //        object temp = Activator.CreateInstance(t2);

                //        Dictionary<string, object> tempdic = (Dictionary<string, object>)newval;
                //        deserialize_r(dt, ref val, tempdic);

                //        deserialize_object_from_lua(ref l, luapath + "." + idx, ref temp, t2);
                //        id[idx] = temp;
                //    }
                //}
                if (T.GetInterface("IList") == typeof(IList))
                {
                    IList il = (IList)o;
                    for (int i = 0; i < parced.Count; i++)
                    {
                        string si = i.ToString();
                        if (parced.ContainsKey(si))
                        {
                            object temp = parced[si];
                            if (temp is IDictionary)
                            {
                                Dictionary<string, object> tempdic = (Dictionary<string, object>)temp;
                                Type t2 = T.GenericTypeArguments[0];
                                object val = Activator.CreateInstance(t2);
                                deserialize_r(dt, ref val, tempdic);
                                il.Add(val);
                            }
                        }
                    }
                    return;
                }


                object ethalon = Activator.CreateInstance(T); //create empty thing
                PropertyInfo[] props = T.GetProperties();
                foreach (PropertyInfo f in props)//iterating through propertys only
                {
                    data t = Attribute.GetCustomAttribute(f, typeof(data)) as data;
                    // = m..GetCustomAttribute<data>();
                    if (t != null)
                        ///check flag
                        if ((t.dt & dt) == dt)
                        {
                            //we got name
                            string propertyname = f.Name;
                            //we got actual value
                            object val = T.GetProperty(propertyname).GetValue(o);
                            //we got type
                            Type valt = val.GetType();

                            if (parced.ContainsKey(propertyname))
                            {
                                object newval = parced[propertyname];
                                Type newvalt = newval.GetType();

                                if (checktype(valt))//simple type
                                {
                                    if (newvalt.GetInterface("IConvertible") == typeof(IConvertible))
                                        newval = Convert.ChangeType(newval, valt);
                                    //object newtempval = parcenumber(newval, valt);//if it is number change it type;
                                    T.GetRuntimeProperty(propertyname).SetValue(ethalon, newval);
                                }
                                else//class or else
                                {
                                    Dictionary<string, object> tempdic = (Dictionary<string, object>)newval;
                                    deserialize_r(dt, ref val, tempdic);

                                    T.GetRuntimeProperty(propertyname).SetValue(ethalon, val);
                                }

                            }
                            //else { Log.Logs[0].Add("no +" + propertyname + "+ value found"); }
                        }
                }
                o = ethalon;
            }
            /* private static void deserialize_object(Dictionary<string, object> d, string luapath, ref object o, Type T = null)// where T : new()
             {
                 //string error = "";
                 if (T == null) T = o.GetType();
                 if (o == null)
                     try
                     {
                         o = Activator.CreateInstance(T);

                     }
                     catch (Exception ex) { throw new Exception(ex.Source + "\n" + T.Name + "has no default constructor"); }



                 if (T.GetInterface("IDictionary") == typeof(IDictionary))
                 {
                     IDictionary id = (IDictionary)o;

                     foreach (object idx in id.Keys)
                     {
                         Type t2 = T.GenericTyperemote_commands[0];
                         object temp = Activator.CreateInstance(t2);
                         deserialize_object_from_lua(ref l, luapath + "." + idx, ref temp, t2);
                         id[idx] = temp;
                     }
                 }
                 if (T.GetInterface("IList") == typeof(IList))
                 {
                     IList il = (IList)o;
                     foreach (double idx in lt.Keys)
                     {
                         Type t2 = T.GenericTyperemote_commands[0];
                         object temp = Activator.CreateInstance(t2);
                         deserialize_object_from_lua(ref l, luapath + "[" + idx + "]", ref temp, t2);
                         il.Add(temp);
                     }
                 }
                 MemberInfo[] members = T.GetMembers();

                 foreach (MemberInfo m in members)
                 {
                     data att = m.GetCustomAttribute<data>();
                     if (att != null)
                     {
                         object val = null;
                         val = T.GetRuntimeproperty(m.Name).GetValue(o);
                         Type valt = val.GetType();
                         //simple type
                         if (checktype(valt))
                         {
                             //we got name
                             string membername = m.Name;
                             //we got actual value
                             object val = T.Getproperty(membername).GetValue(o);
                             //we got type
                             Type valt = val.GetType();

                             object newval = parced[membername];
                             Type newvalt = newval.GetType();

                             object[] temp = null;
                             // try
                             //{
                             temp = l.DoString("return " + luapath + "." + f.Name);
                             //}
                             // catch (Exception exc) { }
                             if (temp != null && temp[0] != null)

                             //((IConvertible)val)
                             T.GetRuntimeproperty(m.Name).SetValue(o, val);
                         }
                         //class
                         else
                         {
                             //TODO
                             deserialize_object_from_lua(ref l, luapath + "." + f.Name, ref val, valt);
                             T.GetRuntimeproperty(m.Name).SetValue(o, val);
                             //throw new NotImplementedException();
                         }
                     }
                 }
             }//*/

            private static string TypeDependResult(object val, string name = "")
            {

                string end = "";

                if (name != "")
                {
                    name = "\"" + name + "\":";
                    end = ",\n";
                }


                Type T = val.GetType();
                switch (T.ToString())
                {
                    case "System.String":
                        return name + '"' + val.ToString() + '"' + end;

                    case "System.Boolean":
                        if ((bool)val) return name + "true" + end;
                        else return name + "false" + end;

                    case "System.Byte":
                        return name + val.ToString() + end;

                    case "System.Int16":
                        return name + val.ToString() + end;

                    case "System.Int32":
                        return name + val.ToString() + end;

                    case "System.Int64":
                        return name + val.ToString() + end;

                    case "System.UInt16":
                        return name + val.ToString() + end;

                    case "System.UInt32":
                        return name + val.ToString() + end;

                    case "System.UInt64":
                        return name + val.ToString() + end;

                    case "System.Single":
                        float f = (float)val;
                        return name + f.ToString("F5", System.Globalization.CultureInfo.InvariantCulture) + end;
                    default: return "null";
                }

            }

            /// </summary>
            /// <param name="o"></param>
            /// <param name="name"></param>
            /// <param name="dt"></param>
            /// <param name="ethalon"></param>
            /// <param name="recurselevel"></param>
            /// <returns></returns>
            private static string serialize_object_to_string
                (
                object o,
                string name = "",
                datatype dt = datatype.template,
                int recurselevel = 0)
            {
                string ret = "";
                Type T = o.GetType();
                string end = "";
                string tab = "";
                for (int i = 0; i < recurselevel; i++)
                {
                    tab += "\t";
                }
                //prepare header
                if (name != "") ret += "\n" + tab + "\"" + name + "\":";
                else ret += tab;

                if (checktype(T)) ret += TypeDependResult(o);
                else { ret += "\n" + tab + "{\n"; end = "}"; }

                if (T.GetInterface("IDictionary") == typeof(IDictionary))
                {
                    recurselevel++;
                    if (((IDictionary)o).Keys.Count > 0)
                        foreach (object key in ((IDictionary)o).Keys)
                        {
                            if (key.GetType() == typeof(string))
                                ret += serialize_object_to_string(((IDictionary)o)[key], key.ToString(), dt, recurselevel);
                        }
                    recurselevel--;
                }
                if (T.GetInterface("IList") == typeof(IList))
                {
                    recurselevel++;
                    if (((IList)o).Count > 0)
                        for (int i = 0; i < ((IList)o).Count; i++)
                        {
                            ret += serialize_object_to_string(((IList)o)[i], i.ToString(), dt, recurselevel);
                        }
                    recurselevel--;
                }


                //object ethalon = Activator.CreateInstance(T);
                PropertyInfo[] props = T.GetProperties();
                foreach (PropertyInfo m in props)
                {
                        data t = Attribute.GetCustomAttribute(m, typeof(data)) as data;
                        // = m..GetCustomAttribute<data>();
                        if (t != null)
                            ///check flag
                            if ((t.dt & dt) == dt)
                            {
                                try
                                {
                                    object val = null;
                                    object ethalonval = null;
                                    val = T.GetProperty(m.Name).GetValue(o);
                                    Type valt = val.GetType();

                                    object ethalon = Activator.CreateInstance(T);

                                    ethalonval = T.GetProperty(m.Name).GetValue(ethalon);





                                    //if simple type
                                    if (checktype(valt))
                                    {
                                        if ((t.dt & datatype.usedefaults) == datatype.usedefaults)
                                        {
                                            ret += tab + TypeDependResult(val, m.Name);
                                        }
                                        else if (val.ToString() != ethalonval.ToString())
                                            ret += tab + TypeDependResult(val, m.Name);
                                    }

                                    //If it is stuct or class with subs
                                    else
                                    {
                                        recurselevel++;
                                        ret += tab + serialize_object_to_string(val, m.Name, dt, recurselevel);
                                        recurselevel--;
                                    }
                                }
                                catch (Exception exc)
                                {
                                    ret += TypeDependResult(exc.Message, m.Name);
                                }

                            }
                    
                }
                char retend = ret[ret.Length - 1];
                if (dt == datatype.template)
                {
                    if (retend != '{')
                        ret += end + ",";
                    else ret = "";
                }
                else ret += end + ",";

                //ret = ret.Replace(",}", "}");

                return ret;
            }
        }
    }



