using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDump
{
    enum eIDLAttr
    {
        NOATTR,
        ROOT,//root class
        KEY, //dict的key
        STRING,//从string解析复杂字段
        IN,//枚举限定
        RANGE,//范围检查
        OPTIONAL,//可选的字段，默认为必须有
        TAG,//标签名 xml标签和类名不一致用
        REWARD,//奖励类型，特殊解析
    }

    class IDLAttr
    {
        public eIDLAttr attr_type = eIDLAttr.NOATTR;
        public string attr_name;
        public string attr_param;

        public static IDLAttr ParseAttr(string line)
        {
            string attrstr = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);
            //处理带参数的属性
            string[] ss = attrstr.Split('(');
            string attrname = ss[0];
            string attrparam = null;

            if (ss.Length > 1)
            {
                attrparam = ss[1].Split(')')[0];
            }

            if (!string.IsNullOrEmpty(attrstr))
            {
                if (attrname == "root")
                {
                    return new IDLAttr { attr_type = eIDLAttr.ROOT, attr_name = attrname, attr_param = attrparam };
                }
                if (attrname == "key")
                {
                    return new IDLAttr { attr_type = eIDLAttr.KEY, attr_name = attrname, attr_param = attrparam };
                }
                if (attrname == "string")
                {
                    return new IDLAttr { attr_type = eIDLAttr.STRING, attr_name = attrname, attr_param = attrparam };
                }
                if (attrname == "optional")
                {
                    return new IDLAttr { attr_type = eIDLAttr.OPTIONAL, attr_name = attrname, attr_param = attrparam };
                }
                if (attrname == "tag")
                {
                    return new IDLAttr { attr_type = eIDLAttr.TAG, attr_name = attrname, attr_param = attrparam };
                }
                if (attrname == "range")
                {
                    return new IDLAttr { attr_type = eIDLAttr.RANGE, attr_name = attrname, attr_param = attrparam };
                }
                if(attrname == "reward")
                {
                    return new IDLAttr { attr_type = eIDLAttr.REWARD, attr_name = attrname, attr_param = attrparam };
                }
            }
            return null;

        }

    }



}
