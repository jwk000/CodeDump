using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CodeDump
{

    class TemplateData
    {
        //全局变量
        public Dictionary<string, object> globalVariantDict = new Dictionary<string, object>();
        //局部变量
        public Dictionary<string, object> localVariantDict = new Dictionary<string, object>();

        public TemplateData()
        {
            globalVariantDict.Add("G", new GlobalMeta());
        }
        public void SetGlobalVariant(string name, object obj)
        {
            if (globalVariantDict.ContainsKey(name))
            {
                globalVariantDict[name] = obj;
                return;
            }
            globalVariantDict.Add(name, obj);
        }

        public void SetLocalVariant(string name, object obj)
        {
            if (localVariantDict.ContainsKey(name))
            {
                localVariantDict[name] = obj;
                return;
            }
            localVariantDict.Add(name, obj);
        }


        public object GetVariantObject(string objname, string fieldname)
        {
            object obj = null;
            if (localVariantDict.TryGetValue(objname, out obj))
            {
                PropertyInfo info = obj.GetType().GetRuntimeProperty(fieldname);
                return info.GetValue(obj);
            }
            if (globalVariantDict.TryGetValue(objname, out obj))
            {
                PropertyInfo info = obj.GetType().GetRuntimeProperty(fieldname);
                return info.GetValue(obj);
            }

            return null;

        }

        //提取对象
        public object ExtraMetaData(string extra)
        {
            Regex reg = new Regex(@"\${(\w+).(\w+)}");
            var m = reg.Match(extra);
            if(!m.Success)
            {
                return false;
            }
            string objname = m.Groups[1].Value;
            string fieldname = m.Groups[2].Value;

            return GetVariantObject(objname, fieldname);
        }
        //展开宏
        public string ExtendMetaData(string line)
        {
            Regex reg = new Regex(@"\${(\w+).(\w+)}");
            return reg.Replace(line, m =>
            {
                string objname = m.Groups[1].Value;
                string fieldname = m.Groups[2].Value;
                string s = GetVariantObject(objname, fieldname) as string;
                return s;// ?? m.Groups[0].Value;
            });

        }

    }

}
