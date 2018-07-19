using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GYGeneric
{
    public class GenericBuilder
    {
        static void push_primitive(object obj, Type valuetype, BuffBuilder bb)
        {
            if (valuetype == typeof(uint))
            {
                bb.PushUint((uint)(obj));
            }
            else if (valuetype == typeof(int))
            {
                bb.PushInt((int)(obj));
            }
            else if (valuetype == typeof(ushort))
            {
                bb.PushUshort((ushort)(obj));
            }
            else if (valuetype == typeof(short))
            {
                bb.PushShort((short)(obj));
            }
            else if (valuetype == typeof(ulong))
            {
                bb.PushUlong((ulong)(obj));
            }
            else if (valuetype == typeof(long))
            {
                bb.PushLong((long)(obj));
            }
            else if (valuetype == typeof(byte))
            {
                bb.PushByte((byte)(obj));
            }
            else if (valuetype == typeof(float))
            {
                bb.PushFloat((float)(obj));
            }
            else if(valuetype == typeof(bool))
            {
                bb.PushBool((bool)(obj));
            }
        }
        static void push_primitive_array(object obj, int length, FieldInfo field, BuffBuilder bb)
        {
            System.Type elemtype = field.FieldType.GetElementType();
            if (elemtype == typeof(uint))
            {
                uint[] objs = (uint[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushUint(objs[j]);
                }
            }
            else if (elemtype == typeof(int))
            {
                int[] objs = (int[])field.GetValue(obj);
                for (uint j = 0; j < length; ++j)
                {
                    bb.PushInt(objs[j]);
                }
            }
            else if (elemtype == typeof(ushort))
            {
                ushort[] objs = (ushort[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushUshort(objs[j]);
                }
            }
            else if (elemtype == typeof(short))
            {
                short[] objs = (short[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushShort(objs[j]);
                }
            }
            else if (elemtype == typeof(ulong))
            {
                ulong[] objs = (ulong[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushUlong(objs[j]);
                }
            }
            else if (elemtype == typeof(long))
            {
                long[] objs = (long[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushLong(objs[j]);
                }
            }
            else if (elemtype == typeof(byte))
            {
                byte[] objs = (byte[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushByte(objs[j]);
                }
            }
            else if (elemtype == typeof(float))
            {
                float[] objs = (float[])field.GetValue(obj);
                for (int j = 0; j < length; ++j)
                {
                    bb.PushFloat(objs[j]);
                }
            }
            else if (elemtype == typeof(bool))
            {
                bool[] objs = (bool[])field.GetValue(obj);
                for (int j = 0; j < length; j++)
                {
                    bb.PushBool(objs[j]);
                }
            }
        }
        static void push_class(object obj, BuffBuilder bb)
        {
            FieldInfo[] fields = obj.GetType().GetFields();
            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i].FieldType.IsPrimitive)
                {
                    push_primitive(fields[i].GetValue(obj), fields[i].FieldType, bb);
                }
                else if(fields[i].FieldType.IsEnum)
                {
                    push_primitive((int)fields[i].GetValue(obj), typeof(int), bb);
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    string s = fields[i].GetValue(obj) as string;
                    bb.PushString(s);
                }
                else if (fields[i].FieldType.IsArray)
                {
                    System.Type elemtype = fields[i].FieldType.GetElementType();
                    if (elemtype.IsClass)
                    {
                        object[] objs = (object[])fields[i].GetValue(obj);
                        bb.PushInt(objs.Length);
                        for (int j = 0; j < objs.Length; ++j)
                        {
                            push_class(objs[j], bb);
                        }
                    }
                    else if (elemtype.IsPrimitive)
                    {
                        object[] objs = (object[])fields[i].GetValue(obj);
                        push_primitive_array(obj, objs.Length, fields[i], bb);
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
                        bb.PushInt(length);
                        //keys
                        MethodInfo keysMethod = fields[i].FieldType.GetMethod("get_Keys");
                        ICollection kc = keysMethod.Invoke(fields[i].GetValue(obj), null) as ICollection;
                        foreach (var k in kc)
                        {
                            push_primitive(k, keyType, bb);
                        }
                        //values
                        MethodInfo valueMethod = fields[i].FieldType.GetMethod("get_Values");
                        ICollection vc = valueMethod.Invoke(fields[i].GetValue(obj), null) as ICollection;
                        foreach (var v in vc)
                        {
                            push_class(v, bb);
                        }
                    }
                    else if (fields[i].FieldType.Name == "List`1")
                    {
                        Type valueType = fields[i].FieldType.GenericTypeArguments[0];
                        MethodInfo countMethod = fields[i].FieldType.GetMethod("get_Count");
                        int length = (int)countMethod.Invoke(fields[i].GetValue(obj), null);
                        bb.PushInt(length);
                        //values
                        ICollection c = fields[i].GetValue(obj) as ICollection;
                        if (valueType.IsClass)
                        {
                            foreach (var e in c)
                            {
                                if (valueType == typeof(string))
                                {
                                    string s = e as string;
                                    bb.PushString(s);
                                }
                                else
                                {
                                    push_class(e, bb);
                                }
                            }
                        }
                        else if (valueType.IsPrimitive)
                        {
                            foreach (var e in c)
                            {
                                push_primitive(e, valueType, bb);
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
                    push_class(sub_obj, bb);
                }
                else
                {
                    throw new Exception(string.Format("不支持的序列化类型{0}", fields[i].FieldType.Name));
                }
            }
        }
        public static byte[] GenericsEncode(object obj)
        {
            BuffBuilder bb = new BuffBuilder(1024 * 1024);
            push_class(obj, bb);
            return bb.GetBuff();
        }

    }
}
