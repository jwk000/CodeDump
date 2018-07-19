using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GYGeneric
{
    public class GenericParser
    {

        static object pop_primitive(Type FieldType, BuffParser bp)
        {
            if (FieldType == typeof(uint))
            {
                return bp.PopUint();
            }
            else if (FieldType == typeof(int))
            {
                return bp.PopInt();
            }
            else if (FieldType == typeof(ushort))
            {
                return bp.PopUshort();
            }
            else if (FieldType == typeof(short))
            {
                return bp.PopShort();
            }
            else if (FieldType == typeof(ulong))
            {
                return bp.PopUint64();
            }
            else if (FieldType == typeof(long))
            {
                return bp.PopLong();
            }
            else if (FieldType == typeof(byte))
            {
                return bp.PopByte();
            }
            else if (FieldType == typeof(float))
            {
                return bp.PopFloat();
            }
            else if (FieldType == typeof(bool))
            {
                return bp.PopBool();
            }
            return null;
        }
        static Array pop_primitive_array(int length, Type elemtype, BuffParser bp)
        {
            if (elemtype == typeof(uint))
            {
                uint[] objs = (uint[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopUint32();
                }
                return objs;
            }
            else if (elemtype == typeof(int))
            {
                int[] objs = (int[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopInt32();
                }
                return objs;
            }
            else if (elemtype == typeof(ushort))
            {
                ushort[] objs = (ushort[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopUshort();
                }
                return objs;
            }
            else if (elemtype == typeof(short))
            {
                short[] objs = (short[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopShort();
                }
                return objs;
            }
            else if (elemtype == typeof(ulong))
            {
                ulong[] objs = (ulong[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopUlong();
                }
                return objs;
            }
            else if (elemtype == typeof(long))
            {
                long[] objs = (long[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopLong();
                }
                return objs;
            }
            else if (elemtype == typeof(float))
            {
                float[] objs = (float[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopFloat();
                }
                return objs;
            }
            else if (elemtype == typeof(byte))
            {
                byte[] objs = (byte[])Array.CreateInstance(elemtype, length);
                objs = bp.PopBytes((int)length);
                return objs;
            }
            else if (elemtype == typeof(bool))
            {
                bool[] objs = (bool[])Array.CreateInstance(elemtype, length);
                for (uint j = 0; j < length; ++j)
                {
                    objs[j] = bp.PopBool();
                }
                return objs;
            }
            return null;
        }

        static void pop_class(object obj, BuffParser bp)
        {
            FieldInfo[] fields = obj.GetType().GetFields();
            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i].FieldType.IsPrimitive)
                {
                    object o = pop_primitive(fields[i].FieldType, bp);
                    fields[i].SetValue(obj, o);
                }
                else if(fields[i].FieldType.IsEnum)
                {
                    object o = pop_primitive(typeof(int), bp);
                    fields[i].SetValue(obj, o);
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    int length = bp.PopInt();
                    fields[i].SetValue(obj, bp.PopString((int)length));
                }
                else if (fields[i].FieldType.IsArray)
                {
                    int length = bp.PopInt();
                    if (length == 0)
                    {
                        continue;
                    }

                    Type elemtype = fields[i].FieldType.GetElementType();
                    if (elemtype.IsClass)
                    {
                        object[] objs = (object[])Array.CreateInstance(elemtype, length);
                        for (int j = 0; j < length; ++j)
                        {
                            objs[j] = Activator.CreateInstance(elemtype);
                            pop_class(objs[j], bp);
                        }
                        fields[i].SetValue(obj, objs);
                    }
                    else if (elemtype.IsPrimitive)
                    {
                        object o = pop_primitive_array(length, elemtype, bp);
                        fields[i].SetValue(obj, o);
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
                            throw new Exception("不能使用class作为Dict的key");
                        }
                        Type valueType = fields[i].FieldType.GenericTypeArguments[1];
                        int length = bp.PopInt();
                        Array keys = pop_primitive_array(length, keyType, bp);
                        Array values = Array.CreateInstance(valueType, length);
                        if (valueType.IsClass)
                        {
                            for (int j = 0; j < length; ++j)
                            {
                                object vo = Activator.CreateInstance(valueType);
                                pop_class(vo, bp);
                                values.SetValue(vo, j);
                            }
                        }
                        else if (valueType.IsPrimitive)
                        {
                            values = pop_primitive_array(length, valueType, bp);
                        }
                        object dict = Activator.CreateInstance(fields[i].FieldType);
                        MethodInfo addMethod = fields[i].FieldType.GetMethod("Add");
                        for (int n = 0; n < length; n++)
                        {
                            object[] param = new object[] { keys.GetValue(n), values.GetValue(n) };
                            addMethod.Invoke(dict, param);
                        }
                        fields[i].SetValue(obj, dict);
                    }
                    else if (fields[i].FieldType.Name == "List`1")
                    {
                        Type valueType = fields[i].FieldType.GenericTypeArguments[0];
                        int length = bp.PopInt();
                        MethodInfo addMethod = fields[i].FieldType.GetMethod("Add");
                        object list = Activator.CreateInstance(fields[i].FieldType);
                        Array values = Array.CreateInstance(valueType, length);
                        if (valueType == typeof(string))
                        {
                            for (int j = 0; j < length; ++j)
                            {
                                int len = bp.PopInt();
                                values.SetValue(bp.PopString(len), j);
                            }
                        }
                        else if (valueType.IsClass)
                        {
                            for (int j = 0; j < length; ++j)
                            {
                                object vo = Activator.CreateInstance(valueType);
                                pop_class(vo, bp);
                                values.SetValue(vo, j);
                            }
                        }
                        else if (valueType.IsPrimitive)
                        {
                            values = pop_primitive_array(length, valueType, bp);
                        }

                        for (int n = 0; n < length; n++)
                        {
                            addMethod.Invoke(list, new[] { values.GetValue(n) });
                        }
                        fields[i].SetValue(obj, list);
                    }
                    else
                    {
                        throw new Exception("不支持的泛型序列化");
                    }

                }
                else if (fields[i].FieldType.IsClass)
                {
                    object sub_obj = Activator.CreateInstance(fields[i].FieldType);
                    pop_class(sub_obj, bp);
                    fields[i].SetValue(obj, sub_obj);
                }
                else
                {
                    throw new Exception(string.Format("不支持的反序列化类型{0}", fields[i].FieldType.Name));
                }
            }
        }
        public static T GenericsDecode<T>(byte[] buff) where T : class, new()
        {
            T obj = new T();
            BuffParser bp = new BuffParser(buff);
            pop_class(obj, bp);
            return obj;
        }

    }
}
