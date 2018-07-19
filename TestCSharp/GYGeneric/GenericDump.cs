using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace GYGeneric
{
    class GenericDump
    {
        static int dump_deepth = 0;
        static void dump_primitive(object obj, FieldInfo field, StringBuilder sb)
        {
            string padding = new string('\t', dump_deepth);
            sb.AppendFormat("{0}{1} = {2}\n", padding, field.Name, field.GetValue(obj));
        }
        static string get_primitive(object obj, Type valuetype)
        {
            if (valuetype == typeof(uint))
            {
                return ((uint)(obj)).ToString();
            }
            else if (valuetype == typeof(int))
            {
                return ((int)(obj)).ToString();
            }
            else if (valuetype == typeof(ushort))
            {
                return ((ushort)(obj)).ToString();
            }
            else if (valuetype == typeof(short))
            {
                return ((short)(obj)).ToString();
            }
            else if (valuetype == typeof(ulong))
            {
                return ((ulong)(obj)).ToString();
            }
            else if (valuetype == typeof(long))
            {
                return ((long)(obj)).ToString();
            }
            else if (valuetype == typeof(byte))
            {
                return ((byte)(obj)).ToString();
            }
            else if (valuetype == typeof(float))
            {
                return ((float)(obj)).ToString();
            }
            return null;
        }
        static void dump_primitive_array(object obj, int length, FieldInfo field, StringBuilder sb)
        {
            string padding = new string('\t', dump_deepth);
            System.Type elemtype = field.FieldType.GetElementType();
            if (elemtype == typeof(uint))
            {
                uint[] objs = (uint[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(int))
            {
                int[] objs = (int[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(ushort))
            {
                ushort[] objs = (ushort[])field.GetValue(obj);
                for (uint j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(short))
            {
                short[] objs = (short[])field.GetValue(obj);
                for (uint j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(ulong))
            {
                ulong[] objs = (ulong[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(long))
            {
                long[] objs = (long[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(byte))
            {
                byte[] objs = (byte[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }
            else if (elemtype == typeof(float))
            {
                float[] objs = (float[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    sb.AppendFormat("{0}{1}[{2}] = {3}\n", padding, field.Name, j, objs[j]);
                }
            }

        }
        static void dump_class(object obj, StringBuilder sb)
        {
            if (obj == null)
            {
                sb.Append("null\n");
                return;
            }
            string padding = new string('\t', dump_deepth);
            sb.AppendFormat("{0}{1}\n{0}{{\n", padding, obj.GetType().Name);
            dump_deepth++;
            string padding2 = new string('\t', dump_deepth);

            FieldInfo[] fields = obj.GetType().GetFields();
            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i].FieldType.IsPrimitive)
                {
                    dump_primitive(obj, fields[i], sb);
                }
                else if(fields[i].FieldType.IsEnum)
                {
                    sb.AppendFormat("{0}{1} = {2}\n", padding2, fields[i].Name, fields[i].GetValue(obj));
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    string s = fields[i].GetValue(obj) as string;
                    sb.AppendFormat("{0}{1} = {2}\n", padding2, fields[i].Name, s);
                }
                else if (fields[i].FieldType.IsArray)
                {
                    Type elemtype = fields[i].FieldType.GetElementType();
                    object[] objs = (object[])fields[i].GetValue(obj);
                    if (elemtype.IsClass)
                    {
                        foreach (var o in objs)
                        {
                            dump_class(o, sb);
                        }
                    }
                    else if (elemtype.IsPrimitive)
                    {
                        dump_primitive_array(obj, objs.Length, fields[i], sb);
                    }
                }
                else if (fields[i].FieldType.IsConstructedGenericType)
                {
                    //处理dictionary
                    if (fields[i].FieldType.Name == "Dictionary`2")
                    {
                        Type keyType = fields[i].FieldType.GenericTypeArguments[0];
                        if (!keyType.IsPrimitive)
                        {
                            throw new Exception("必须使用基础类型作为Dict的key");
                        }
                        Type valueType = fields[i].FieldType.GenericTypeArguments[1];
                        MethodInfo countMethod = fields[i].FieldType.GetMethod("get_Count");
                        int length = (int)countMethod.Invoke(fields[i].GetValue(obj), null);
                        //keys
                        MethodInfo keysMethod = fields[i].FieldType.GetMethod("get_Keys");
                        ICollection kc = keysMethod.Invoke(fields[i].GetValue(obj), null) as ICollection;
                        List<object> keylist = new List<object>();
                        foreach (var k in kc)
                        {
                            keylist.Add(k);
                        }
                        //values
                        MethodInfo valueMethod = fields[i].FieldType.GetMethod("get_Values");
                        ICollection vc = valueMethod.Invoke(fields[i].GetValue(obj), null) as ICollection;
                        List<object> valuelist = new List<object>();
                        foreach (var v in vc)
                        {
                            valuelist.Add(v);
                        }

                        for (int j = 0; j < length; j++)
                        {
                            sb.AppendFormat("{0}{1}[{2}]=\n", padding, fields[i].Name, (int)keylist[j]);
                            if (valueType.IsClass)
                            {
                                dump_class(valuelist[j], sb);
                            }
                            else
                            {
                                throw new Exception("不支持普通类型作为dict value");
                            }
                        }
                    }
                    else if (fields[i].FieldType.Name == "List`1")
                    {
                        Type valueType = fields[i].FieldType.GenericTypeArguments[0];
                        MethodInfo countMethod = fields[i].FieldType.GetMethod("get_Count");
                        int length = (int)countMethod.Invoke(fields[i].GetValue(obj), null);
                        //values
                        ICollection c = fields[i].GetValue(obj) as ICollection;
                        List<object> valuelist = new List<object>();
                        foreach (var e in c)
                        {
                            valuelist.Add(e);
                        }

                        if (valueType.IsClass)
                        {
                            for (int j = 0; j < length; j++)
                            {
                                if (valueType == typeof(string))
                                {
                                    sb.AppendFormat("\t{0}{1}[{2}]={3}\n", padding, fields[i].Name, j, valuelist[j] as string);
                                }
                                else
                                {
                                    sb.AppendFormat("{0}{1}[{2}]=\n", padding, fields[i].Name, j);
                                    dump_class(valuelist[j], sb);
                                }
                            }
                        }
                        if (valueType.IsPrimitive)
                        {
                            for (int j = 0; j < length; j++)
                            {
                                sb.AppendFormat("{0}{1}[{2}]={3}\n", padding, fields[i].Name, j, get_primitive(valuelist[j], valueType));
                            }

                        }
                    }
                    else
                    {
                        throw new Exception("不支持的泛型序列化");
                    }
                }
                else if (fields[i].FieldType.IsClass)
                {
                    object sub_obj = fields[i].GetValue(obj);
                    dump_class(sub_obj, sb);
                }
                else
                {
                    throw new Exception("不支持的序列化类型");
                }
            }
            sb.AppendFormat("{0}}}\n", padding);
            dump_deepth--;
        }
        public static string Dump(object obj)
        {
            StringBuilder sb = new StringBuilder(1024);
            dump_class(obj, sb);
            return sb.ToString();
        }

    }
}
