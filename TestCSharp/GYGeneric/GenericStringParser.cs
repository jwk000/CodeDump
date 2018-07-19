using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GYGeneric
{
    class GenericStringParser
    {
        public static T GetObject<T>(string s)
        {
            Type type = typeof(T);
            if(type.IsPrimitive)
            {
                if (type == typeof(int))
                {
                    return (T)(object)int.Parse(s);
                }

                if (type == typeof(float))
                {
                    return (T)(object)float.Parse(s);
                }
            }
            if (type.IsClass)
            {
                string[] ss = s.Split(',');
                object obj = Activator.CreateInstance(type);
                FieldInfo[] infos = type.GetFields();
                if(ss.Length == infos.Length)
                {
                    for(int i=0;i<ss.Length;i++)
                    {
                        type = infos[i].FieldType;
                        object f = Activator.CreateInstance(type);
                        if (type == typeof(int))
                        {
                            f = (object)int.Parse(ss[i]);
                        }

                        if (type == typeof(float))
                        {
                            f = (object)float.Parse(ss[i]);
                        }

                        infos[i].SetValue(obj, f);
                    }
                    return (T)obj;
                }
            }
            return default(T);
        }

        public static List<T> GetObjectList<T>(string s) 
        {
            List<T> lst = new List<T>();
            var ss = s.Split(';');
            foreach (string t in ss)
            {
                T obj = GetObject<T>(t);
                if (obj != null)
                {
                    lst.Add(obj);
                }
            }
            return lst;
        }

    }
}
