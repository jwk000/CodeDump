using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeDump
{
    enum eIDLType
    {
        INVALID,
        BOOL,
        INT,
        FLOAT,
        STRING,
        ENUM,
        CLASS,
        LIST,
        DICT,
    }

    class IDLType
    {
        public string type_name;
        public eIDLType type = eIDLType.CLASS;//基本类型或容器类型
        public IDLType[] inner_type;//容器内部类型
    }

    enum eIDLAttr
    {
        NOATTR,
        ROOT,//root class
        KEY, //dict的key
        STRING,//从string解析复杂字段
        DEFAULT,//默认值
        IN,//枚举限定
        RANGE,//范围检查
    }

    class IDLAttr
    {
        public eIDLAttr attr_type = eIDLAttr.NOATTR;
        public string attr_name;
    }

    class IDLAttr_RANGE : IDLAttr
    {
        public int min, max;
        public bool check(int num)
        {
            return num >= min && num <= max;
        }
    }

    class IDLEnumField
    {
        public IDLEnum Enum { get; set; }
        public string item_name;
        public string item_value;
        public string comment;
    }

    class IDLEnum
    {
        public IDLMeta Meta { get; set; }

        public string enum_name;
        public string comment;
        public Dictionary<string, IDLEnumField> enum_fields = new Dictionary<string, IDLEnumField>();
    }

    class IDLClassField
    {
        public IDLClass Class { get; set; }
        public IDLType field_type = new IDLType();
        public string type_name;
        public string field_name;
        public string comment;
        public string default_value;
        public IDLAttr field_attrs = new IDLAttr();
    }
    class IDLClass
    {
        public IDLMeta Meta { get; set; }

        public IDLAttr class_attr;
        public string class_name;
        public string comment;
        public List<IDLClassField> fieldList = new List<IDLClassField>();
    }

    class IDLUsing
    {
        public string comment;
        public string using_name;
        public IDLMeta Meta { get; set; }
    }
    enum ParseState
    {
        End,
        BeginClass,
        BeginEnum
    }

    class IDLMeta
    {
        public string meta_name;
        public string meta_file_path;
        public string root_class_name;
        public List<IDLUsing> using_meta = new List<IDLUsing>();
        public Dictionary<string, IDLClass> meta_class = new Dictionary<string, IDLClass>();
        public Dictionary<string, IDLEnum> meta_enum = new Dictionary<string, IDLEnum>();

        ParseState m_parseState = ParseState.End;

        string parseComment(ref string line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '/' && line[i + 1] == '/')
                {
                    string s = line.Substring(i);//带//
                    line = line.Substring(0, i);
                    return s;
                }
            }
            return null;
        }

        IDLAttr parseAttr(string line)
        {
            string attrstr = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);

            if (!string.IsNullOrEmpty(attrstr))
            {
                if (attrstr == "root")
                {
                    return new IDLAttr { attr_type = eIDLAttr.ROOT, attr_name = attrstr };
                }
                if (attrstr == "key")
                {
                    return new IDLAttr { attr_type = eIDLAttr.KEY, attr_name = attrstr };
                }
                if (attrstr == "string")
                {
                    return new IDLAttr { attr_type = eIDLAttr.STRING, attr_name = attrstr };
                }
                //处理带参数的属性
                string attrname;
                string[] attrparam;
                string[] ss = attrstr.Split('(');
                attrname = ss[0];
                ss = ss[1].Split(')');
                ss = ss[0].Split(',');
                attrparam = ss;

                if (attrname == "range")
                {
                    int min = int.Parse(attrparam[0]);
                    int max = int.Parse(attrparam[1]);
                    return new IDLAttr_RANGE
                    {
                        attr_type = eIDLAttr.RANGE,
                        attr_name = attrname,
                        min = min,
                        max = max
                    };
                }
            }
            return null;

        }

        string parseDefaultValue(ref string line)
        {
            var ss = line.Split('=');
            if (ss.Length == 1)
            {
                return null;
            }
            line = ss[0] + ";";
            ss = ss[1].Split(';');
            return ss[0].Trim();
        }

        public bool Parse(string idl)
        {
            meta_class.Clear();
            IDLClass m_class = null;
            IDLEnum m_enum = null;
            string[] lines = File.ReadAllLines(idl);
            meta_name = Path.GetFileNameWithoutExtension(idl);
            meta_file_path = Path.Combine(Path.GetDirectoryName(idl), meta_name).Replace('\\', '/');
            string comment;
            IDLAttr attr = null;
            for (int i = 0; i < lines.Count(); i++)
            {
                //注释
                comment = parseComment(ref lines[i]);

                //空行
                if (Regex.IsMatch(lines[i], @"^\s*$"))
                {
                    continue;
                }


                //特性
                if (Regex.IsMatch(lines[i], @"^\s*\[.*\]\s*$"))
                {
                    attr = parseAttr(lines[i]);
                    continue;
                }

                //引用
                if (Regex.IsMatch(lines[i], @"^\s*using\s+(\w+);\s*$"))
                {
                    Match match = Regex.Match(lines[i], @"^\s*using\s+(\w+);\s*$");
                    IDLUsing u = new IDLUsing();
                    u.comment = comment;
                    u.using_name = match.Groups[1].Value;
                    u.Meta = this;
                    using_meta.Add(u);
                    continue;
                }

                //结束
                if (Regex.IsMatch(lines[i], @"^\s*}\s*$"))
                {
                    if (m_parseState == ParseState.End)
                    {
                        Console.WriteLine("idl文件错误：第{0}行", i);
                        return false;
                    }
                    if (m_parseState == ParseState.BeginClass)
                    {
                        meta_class.Add(m_class.class_name, m_class);
                        m_class = null;
                    }
                    else if (m_parseState == ParseState.BeginEnum)
                    {
                        meta_enum.Add(m_enum.enum_name, m_enum);
                        m_enum = null;
                    }
                    m_parseState = ParseState.End;

                    continue;
                }
                //枚举开始
                if (Regex.IsMatch(lines[i], @"^\s*enum\s*\w+\s*{\s*$"))
                {
                    if (m_parseState != ParseState.End)
                    {
                        Console.WriteLine("idl文件错误：第{0}行", i);
                        return false;
                    }
                    m_parseState = ParseState.BeginEnum;
                    Match match = Regex.Match(lines[i], @"^\s*enum\s*(\w+)\s*{\s*$");
                    m_enum = new IDLEnum();
                    m_enum.comment = comment;
                    m_enum.enum_name = match.Groups[1].Value;
                    m_enum.Meta = this;
                    attr = null;//用完清空
                    continue;
                }

                //结构体开始
                if (Regex.IsMatch(lines[i], @"^\s*class\s*\w+\s*{\s*$"))
                {
                    if (m_parseState != ParseState.End)
                    {
                        Console.WriteLine("idl文件错误：第{0}行", i);
                        return false;
                    }
                    m_parseState = ParseState.BeginClass;
                    Match match = Regex.Match(lines[i], @"^\s*class\s*(\w+)\s*{\s*$");
                    m_class = new IDLClass();
                    m_class.comment = comment;
                    m_class.class_name = match.Groups[1].Value;
                    m_class.Meta = this;
                    m_class.class_attr = attr;
                    if(attr!=null && attr.attr_type == eIDLAttr.ROOT)
                    {
                        this.root_class_name = m_class.class_name;
                    }
                    attr = null;//用完清空
                    continue;
                }


                //结构体
                if (m_parseState == ParseState.BeginClass)
                {
                    string def = parseDefaultValue(ref lines[i]);
                    Match m = Regex.Match(lines[i], @"\s*(\S+)\s+(\w+)\s*;\s*$");
                    if (m.Success == false)
                    {
                        Console.WriteLine("idl文件错误：第{0}行", i);
                        return false;
                    }

                    IDLClassField field = new IDLClassField();
                    field.comment = comment;
                    field.type_name = m.Groups[1].Value;
                    field.field_name = m.Groups[2].Value;
                    field.default_value = def;
                    field.field_attrs = attr;
                    field.Class = m_class;
                    if (!ParseFieldType(field.type_name, field.field_type))
                    {
                        Console.WriteLine("idl文件错误：第{0}行，字段类型无效", i);
                        return false;
                    }
                    m_class.fieldList.Add(field);
                    attr = null;//用完清空
                    continue;
                }

                //枚举
                if (m_parseState == ParseState.BeginEnum)
                {
                    Match m = Regex.Match(lines[i], @"\s*(\w+)\s*=\s*(\w+)\s*,\s*$");
                    if (m.Success == false)
                    {
                        Console.WriteLine("idl文件错误：第{0}行", i);
                        return false;
                    }

                    IDLEnumField item = new IDLEnumField();
                    item.comment = comment;
                    item.item_name = m.Groups[1].Value;
                    item.item_value = m.Groups[2].Value;
                    item.Enum = m_enum;
                    m_enum.enum_fields.Add(item.item_name, item);
                    attr = null;//用完清空
                    continue;
                }



                Console.WriteLine("idl文件错误：第{0}行", i);
                return false;
            }
            return true;


        }

        public IDLEnum FindUsingEnum(string enum_name)
        {
            if(meta_enum.TryGetValue(enum_name, out IDLEnum enumtype))
            {
                return enumtype;
            }

            foreach (var u in using_meta)
            {
                var c = CodeGenHelper.FindMetaEnum(u.using_name, enum_name);
                if (c != null)
                {
                    return c;
                }
            }
            return null;

        }
        public IDLClass FindUsingClass(string class_name)
        {
            if (meta_class.TryGetValue(class_name, out IDLClass classtype))
            {
                return classtype;
            }

            foreach (var u in using_meta)
            {
                var c = CodeGenHelper.FindMetaClass(u.using_name, class_name);
                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }
        public bool ParseFieldType(string typename, IDLType fieldtype)
        {
            fieldtype.type_name = typename;
            switch (typename)
            {
                case "int": { fieldtype.type = eIDLType.INT; break; }
                case "float": { fieldtype.type = eIDLType.FLOAT; break; }
                case "string": { fieldtype.type = eIDLType.STRING; break; }
                case "bool": { fieldtype.type = eIDLType.BOOL;break; }
                default:
                    Match m = Regex.Match(typename, @"List<(\w+)>");
                    if (m.Success)
                    {
                        fieldtype.type = eIDLType.LIST;
                        fieldtype.inner_type = new IDLType[1] { new IDLType() };
                        string inner_name = m.Groups[1].Value;
                        if (!ParseFieldType(inner_name, fieldtype.inner_type[0]))
                        {
                            return false;
                        }
                        break;
                    }

                    m = Regex.Match(typename, @"Dict<(\w+),(\w+)>");
                    if (m.Success)
                    {
                        fieldtype.type = eIDLType.DICT;
                        fieldtype.inner_type = new IDLType[2] { new IDLType(), new IDLType() };
                        string key_name = m.Groups[1].Value;
                        if (!ParseFieldType(key_name, fieldtype.inner_type[0]))
                        {
                            return false;
                        }
                        string value_name = m.Groups[2].Value;
                        if (!ParseFieldType(value_name, fieldtype.inner_type[1]))
                        {
                            return false;
                        }
                        break;
                    }

                    if(FindUsingClass(typename)!=null)
                    {
                        fieldtype.type = eIDLType.CLASS;
                        break;
                    }

                    if (FindUsingEnum(typename)!=null)
                    {
                        fieldtype.type = eIDLType.ENUM;
                        break;
                    }

                    Console.WriteLine("生成类型名称错误，未能识别{0}", typename);
                    return false;
            }
            return true;
        }
    }


}
