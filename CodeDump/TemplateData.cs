using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CodeDump
{
    enum eTemplateData
    {
        DATETIME, //得到当前时间
        META_NAME, //meta名称
        META_FILE_PATH, //meta文件路径
        ROOT_CLASS_NAME, //根类型名称
        USING_META_NAME, //引用meta的名称
        USING_COMMENT, //引用的注释
        CLASS_NAME, //类型名称
        CLASS_ATTR_NAME,//类特性名称
        CLASS_COMMENT, //类的注释
        CLASS_KEY_NAME, //类的key名称
        CLASS_FIELD_NAME,//类字段名称
        CLASS_FIELD_TYPE, //类字段类型
        LIST_VALUE_TYPE, //list类型的value类型
        DICT_VALUE_TYPE, //dict类型的value类型
        CLASS_FIELD_COMMENT, //类字段注释
        CLASS_FIELD_KEY, //类字段类的key名称
        CLASS_FIELD_DEFAULT_VALUE, //类字段默认值
        ENUM_NAME, //枚举名称
        ENUM_COMMENT,//枚举注释
        ENUM_FIELD_NAME, //枚举字段名称
        ENUM_FIELD_VALUE, //枚举字段值
        ENUM_FIELD_COMMENT, //枚举字段注释

        MaxCount
    }

    class TemplateData
    {
        public Dictionary<string, Func<object, string>> metaFuncDict = new Dictionary<string, Func<object, string>>();
        public Dictionary<string, Func<object, string>> usingFuncDict = new Dictionary<string, Func<object, string>>();
        public Dictionary<string, Func<object, string>> classFuncDict = new Dictionary<string, Func<object, string>>();
        public Dictionary<string, Func<object, string>> classFieldFuncDict = new Dictionary<string, Func<object, string>>();
        public Dictionary<string, Func<object, string>> enumFuncDict = new Dictionary<string, Func<object, string>>();
        public Dictionary<string, Func<object, string>> enumFieldFuncDict = new Dictionary<string, Func<object, string>>();
        public TemplateData()
        {
            metaFuncDict.Add("${DATETIME}", GetDateTime);
            metaFuncDict.Add("${META_NAME}", GetMetaName);
            metaFuncDict.Add("${META_FILE_PATH}", GetMetaFilePath);
            metaFuncDict.Add("${ROOT_CLASS_NAME}", GetRootClassName);
            usingFuncDict.Add("${USING_META_NAME}", GetUsingMetaName);
            usingFuncDict.Add("${USING_COMMENT}", GetUsingComment);
            classFuncDict.Add("${CLASS_NAME}", GetClassName);
            classFuncDict.Add("${CLASS_ATTR_NAME}", GetClassAttrName);
            classFuncDict.Add("${CLASS_COMMENT}", GetClassComment);
            classFuncDict.Add("${CLASS_KEY_NAME}", GetClassKeyName);
            classFieldFuncDict.Add("${CLASS_FIELD_NAME}", GetClassFieldName);
            classFieldFuncDict.Add("${CLASS_FIELD_TYPE}", GetClassFieldType);
            classFieldFuncDict.Add("${LIST_VALUE_TYPE}", GetListValueType);
            classFieldFuncDict.Add("${DICT_VALUE_TYPE}", GetDictValueType);
            classFieldFuncDict.Add("${CLASS_FIELD_COMMENT}", GetClassFieldComment);
            classFieldFuncDict.Add("${CLASS_FIELD_KEY}", GetClassDictFieldKeyName);
            classFieldFuncDict.Add("${CLASS_FIELD_DEFAULT_VALUE}", GetClassFieldDefaultValue);
            enumFuncDict.Add("${ENUM_NAME}", GetEnumName);
            enumFuncDict.Add("${ENUM_COMMENT}", GetEnumComment);
            enumFieldFuncDict.Add("${ENUM_FIELD_NAME}", GetEnumFieldName);
            enumFieldFuncDict.Add("${ENUM_FIELD_VALUE}", GetEnumFieldValue);
            enumFieldFuncDict.Add("${ENUM_FIELD_COMMENT}", GetEnumFieldComment);
        }


        public string ExtendMetaData(string line, IDLMeta meta)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                Func<object, string> f = null;
                string s = m.Groups[1].Captures[0].Value;
                if (metaFuncDict.TryGetValue(s, out f))
                {
                    return f(meta);
                }
                return s;
            });

        }

        public string ExtendMetaUsingData(string line, IDLUsing meta_using)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                Func<object, string> f = null;
                string s = m.Groups[1].Captures[0].Value;
                if (usingFuncDict.TryGetValue(s, out f))
                {
                    return f(meta_using);
                }
                return s;
            });
        }

        public string ExtendMetaClassData(string line, IDLClass meta_class)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                Func<object, string> f = null;
                string s = m.Groups[1].Captures[0].Value;
                if (classFuncDict.TryGetValue(s, out f))
                {
                    return f(meta_class);
                }
                return s;
            });

        }

        public string ExtendMetaClassFieldData(string line, IDLClassField field)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                string s = m.Groups[1].Captures[0].Value;
                Func<object, string> f = null;
                if (classFieldFuncDict.TryGetValue(s, out f))
                {
                    return f(field);
                }
                return s;
            });

        }

        public string ExtendMetaEnumData(string line, IDLEnum meta_enum)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                string s = m.Groups[1].Captures[0].Value;
                Func<object, string> f = null;
                if (enumFuncDict.TryGetValue(s, out f))
                {
                    return f(meta_enum);
                }
                return s;
            });
        }

        public string ExtendMetaEnumFieldData(string line, IDLEnumField field)
        {
            Regex reg = new Regex(@"(\${\w+})");
            return reg.Replace(line, m =>
            {
                string s = m.Groups[1].Captures[0].Value;
                Func<object, string> f = null;
                if (enumFieldFuncDict.TryGetValue(s, out f))
                {
                    return f(field);
                }
                return s;
            });
        }

        public string GetDateTime(object obj)
        {
            return DateTime.Now.ToString();
        }
        public virtual string GetFieldTypeName(IDLType t)
        {
            switch (t.type)
            {
                case eIDLType.INT:
                    return "int";
                case eIDLType.FLOAT:
                    return "float";
                case eIDLType.BOOL:
                    return "bool";
                case eIDLType.CLASS:
                    return t.type_name;
                case eIDLType.ENUM:
                    return t.type_name;
            }
            return null;
        }

        //meta
        public string GetMetaFilePath(object obj)
        {
            IDLMeta meta = obj as IDLMeta;

            return meta.meta_file_path;
        }
        public string GetMetaName(object obj)
        {
            IDLMeta meta = obj as IDLMeta;
            return meta.meta_name;
        }
        public string GetRootClassName(object obj)
        {
            IDLMeta meta = obj as IDLMeta;

            return meta.root_class_name;
        }

        //using
        public string GetUsingMetaName(object obj)
        {
            IDLUsing use = obj as IDLUsing;
            return use.using_name + ".h";
        }

        public string GetUsingComment(object obj)
        {
            IDLUsing use = obj as IDLUsing;
            return use.comment;
        }
        //class
        public string GetClassName(object obj)
        {
            IDLClass cls = obj as IDLClass;
            return cls.class_name;
        }

        public string GetClassAttrName(object obj)
        {
            IDLClass c = obj as IDLClass;
            if (c.class_attr == null) return null;
            return c.class_attr.attr_name;
        }
        public string GetClassKeyName(object obj)
        {
            IDLClass c = obj as IDLClass;

            var lst = c.fieldList.Where(f => f.field_attrs != null && f.field_attrs.attr_type == eIDLAttr.KEY);
            if (lst.Count() > 0)
            {
                return GetClassFieldName(lst.First());
            }
            return null;
        }

        public string GetClassComment(object obj)
        {
            IDLClass c = obj as IDLClass;
            return c.comment;
        }

        //field
        public string GetClassDictFieldKeyName(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            string dictValueType = GetDictValueType(f);
            IDLClass c = f.Class.Meta.FindUsingClass(dictValueType);
            if (c != null)
            {
                return GetClassKeyName(c);
            }
            return null;
        }
        public string GetClassFieldName(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            return f.field_name;
        }

        public string GetClassFieldComment(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            return f.comment;
        }

        public string GetClassFieldType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            return GetFieldTypeName(f.field_type);
        }
        public string GetDictValueType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.inner_type == null) return null;
            return GetFieldTypeName(f.field_type.inner_type[1]);
        }

        public string GetListValueType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.inner_type == null) return null;
            return GetFieldTypeName(f.field_type.inner_type[0]);
        }

        //自带 = 处理无默认值的情况
        public virtual string GetClassFieldDefaultValue(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (!string.IsNullOrEmpty(f.default_value))
            {
                return "=" + f.default_value;
            }
            if (f.field_type.type == eIDLType.BOOL)
            {
                return "= false";
            }
            if (f.field_type.type == eIDLType.INT)
            {
                return "= 0";
            }
            if (f.field_type.type == eIDLType.FLOAT)
            {
                return "= 0.0f";
            }

            return null;
        }
        //enum
        public string GetEnumComment(object obj)
        {
            IDLEnum e = obj as IDLEnum;
            return e.comment;
        }

        public string GetEnumName(object obj)
        {
            IDLEnum e = obj as IDLEnum;
            return e.enum_name;
        }

        public string GetEnumFieldName(object obj)
        {
            IDLEnumField f = obj as IDLEnumField;
            return f.item_name;
        }
        public string GetEnumFieldValue(object obj)
        {
            IDLEnumField f = obj as IDLEnumField;
            return f.item_value;
        }
        public string GetEnumFieldComment(object obj)
        {
            IDLEnumField f = obj as IDLEnumField;
            return f.comment;
        }

    }

    class TemplateDataCpp : TemplateData
    {
        //得到类型名字
        public override string GetFieldTypeName(IDLType t)
        {
            string baseName = base.GetFieldTypeName(t);
            if (null != baseName)
            {
                return baseName;
            }
            switch (t.type)
            {
                case eIDLType.STRING:
                    return "std::string";
                case eIDLType.LIST:
                    return "std::vector<" + GetFieldTypeName(t.inner_type[0]) + ">";
                case eIDLType.DICT:
                    return "std::map<" + GetFieldTypeName(t.inner_type[0]) + "," + GetFieldTypeName(t.inner_type[1]) + ">";
            }
            return null;
        }

        public override string GetClassFieldDefaultValue(object obj)
        {
            return base.GetClassFieldDefaultValue(obj);
        }

    }

    class TemplateDataCs : TemplateData
    {
        public override string GetFieldTypeName(IDLType t)
        {
            string baseName = base.GetFieldTypeName(t);
            if (null != baseName)
            {
                return baseName;
            }
            switch (t.type)
            {
                case eIDLType.STRING:
                    return "string";
                case eIDLType.LIST:
                    return "List<" + GetFieldTypeName(t.inner_type[0]) + ">";
                case eIDLType.DICT:
                    return "Dictionary<" + GetFieldTypeName(t.inner_type[0]) + "," + GetFieldTypeName(t.inner_type[1]) + ">";
            }
            return null;
        }

        public override string GetClassFieldDefaultValue(object obj)
        {
            string baseValue = base.GetClassFieldDefaultValue(obj);
            if(baseValue != null)
            {
                return baseValue;
            }
            IDLClassField f = obj as IDLClassField;
            if (f.field_type.type == eIDLType.CLASS ||
                f.field_type.type == eIDLType.DICT ||
                f.field_type.type == eIDLType.LIST)
            {
                return "= new " + GetFieldTypeName(f.field_type) + "()";
            }
            return null;
        }

    }
}
