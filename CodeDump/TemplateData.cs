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
        META_DATETIME, //得到当前时间
        META_NAME, //meta名称
        META_HAS_ROOT,//meta是否有root
        META_FILE_NAME,//meta文件名
        META_FILE_PATH, //meta文件路径
        META_ROOT_CLASS_NAME, //根类型名称

        USING_COMMENT, //引用的注释
        USING_META_NAME, //引用meta的名称

        CLASS_COMMENT, //类的注释
        CLASS_NAME, //类型名称
        CLASS_TAG_NAME,//类的xml标签名称
        CLASS_ATTR_NAME,//类特性名称
        CLASS_ATTR_PARAM,//类特性参数
        CLASS_KEY_NAME, //类的key名称

        CLASS_FIELD_COMMENT, //类字段注释
        CLASS_FIELD_NAME,//类字段名称
        CLASS_FIELD_TYPE, //类字段类型
        CLASS_FIELD_TAG,//类字段xml标签名称
        CLASS_FIELD_KEY, //类字段类的key名称
        CLASS_FIELD_DEFAULT_VALUE, //类字段默认值
        CLASS_FIELD_IS_OPTIONAL,//是否可选字段
        CLASS_FIELD_ATTR_NAME,//特性
        CLASS_FIELD_ATTR_PARAM,//特性参数
        CLASS_FIELD_INDEX,//字段序号
        CLASS_FIELD_META_TYPE,//字段元类型

        LIST_VALUE_TYPE, //list类型的value类型
        LIST_VALUE_TAG,//list的value的xml标签

        DICT_KEY_TYPE, //dict类型的key类型
        DICT_VALUE_TYPE, //dict类型的value类型
        DICT_VALUE_TAG,//dict的value的xml标签

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
        public Dictionary<string, Func<object, string>> iteratorFuncDict = new Dictionary<string, Func<object, string>>();

        public TemplateData()
        {
            metaFuncDict.Add("${META_DATETIME}", GetDateTime);
            metaFuncDict.Add("${META_NAME}", GetMetaName);
            metaFuncDict.Add("${META_FILE_PATH}", GetMetaFilePath);
            metaFuncDict.Add("${META_FILE_NAME}", GetCodeFileName);
            metaFuncDict.Add("${META_HAS_ROOT}", GetMetaHasRoot);
            metaFuncDict.Add("${META_ROOT_CLASS_NAME}", GetRootClassName);

            usingFuncDict.Add("${USING_COMMENT}", GetUsingComment);
            usingFuncDict.Add("${USING_META_NAME}", GetUsingMetaName);

            classFuncDict.Add("${CLASS_COMMENT}", GetClassComment);
            classFuncDict.Add("${CLASS_NAME}", GetClassName);
            classFuncDict.Add("${CLASS_TAG_NAME}", GetClassTagName);
            classFuncDict.Add("${CLASS_ATTR_NAME}", GetClassAttrName);
            classFuncDict.Add("${CLASS_ATTR_PARAM}", GetClassAttrParam);
            classFuncDict.Add("${CLASS_KEY_NAME}", GetClassKeyName);

            classFieldFuncDict.Add("${CLASS_FIELD_COMMENT}", GetClassFieldComment);
            classFieldFuncDict.Add("${CLASS_FIELD_NAME}", GetClassFieldName);
            classFieldFuncDict.Add("${CLASS_FIELD_TYPE}", GetClassFieldType);
            classFieldFuncDict.Add("${CLASS_FIELD_TAG}", GetClassFieldTagName);
            classFieldFuncDict.Add("${CLASS_FIELD_KEY}", GetClassDictFieldKeyName);
            classFieldFuncDict.Add("${CLASS_FIELD_DEFAULT_VALUE}", GetClassFieldDefaultValue);
            classFieldFuncDict.Add("${CLASS_FIELD_IS_OPTIONAL}", IsClassFieldOptional);
            classFieldFuncDict.Add("${CLASS_FIELD_ATTR_NAME}", GetClassFieldAttrName);
            classFieldFuncDict.Add("${CLASS_FIELD_ATTR_PARAM}", GetClassFieldAttrParam);
            classFieldFuncDict.Add("${CLASS_FIELD_INDEX}", GetClassFieldIndex);
            classFieldFuncDict.Add("${CLASS_FIELD_META_TYPE}", GetClassFieldMetaType);

            classFieldFuncDict.Add("${LIST_VALUE_TYPE}", GetListValueType);
            classFieldFuncDict.Add("${LIST_VALUE_TAG}", GetListValueTag);

            classFieldFuncDict.Add("${DICT_KEY_TYPE}", GetDictKeyType);
            classFieldFuncDict.Add("${DICT_VALUE_TYPE}", GetDictValueType);
            classFieldFuncDict.Add("${DICT_VALUE_TAG}", GetDictValueTag);

            enumFuncDict.Add("${ENUM_NAME}", GetEnumName);
            enumFuncDict.Add("${ENUM_COMMENT}", GetEnumComment);

            enumFieldFuncDict.Add("${ENUM_FIELD_NAME}", GetEnumFieldName);
            enumFieldFuncDict.Add("${ENUM_FIELD_VALUE}", GetEnumFieldValue);
            enumFieldFuncDict.Add("${ENUM_FIELD_COMMENT}", GetEnumFieldComment);

            iteratorFuncDict.Add("ITERATOR_SELECT", ExecIteratorSelect);
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

        public string ExtendIteratorFunction(string line, int index)
        {
            Regex reg = new Regex(@"(#{(\w+)\((.+?)\)})");
            return reg.Replace(line, m =>
            {
                string s1 = m.Groups[1].Captures[0].Value;
                string s2 = m.Groups[2].Captures[0].Value;
                string s3 = m.Groups[3].Captures[0].Value;
                Func<object, string> f = null;
                if (iteratorFuncDict.TryGetValue(s2, out f))
                {
                    return f(Tuple.Create(index, s3));
                }
                return s1;
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
        public string GetMetaHasRoot(object obj)
        {
            IDLMeta meta = obj as IDLMeta;
            return string.IsNullOrEmpty(meta.root_class_name) ? "false" : "true";
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

        public string GetCodeFileName(object obj)
        {
            IDLMeta meta = obj as IDLMeta;
            return meta.code_file_name;
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

        public string GetClassTagName(object obj)
        {
            IDLClass cls = obj as IDLClass;
            if (cls.class_attr != null && cls.class_attr.attr_type == eIDLAttr.TAG)
            {
                return cls.class_attr.attr_param;
            }
            return cls.class_name;
        }

        public string GetClassAttrName(object obj)
        {
            IDLClass c = obj as IDLClass;
            if (c.class_attr == null) return null;
            return c.class_attr.attr_name;
        }

        public string GetClassAttrParam(object obj)
        {
            IDLClass c = obj as IDLClass;
            if (c.class_attr == null) return null;
            return c.class_attr.attr_param;
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
        public string IsClassFieldOptional(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_attrs != null && f.field_attrs.attr_type == eIDLAttr.OPTIONAL)
            {
                return "true";
            }
            return "false";
        }
        public string GetClassDictFieldKeyName(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_type.type != eIDLType.DICT) return null;

            string dictValueType = GetDictValueType(f);
            IDLClass c = IDLParser.FindUsingClass(f.Class.Meta, dictValueType);
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
        public string GetClassFieldIndex(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            int idx = f.Class.fieldList.IndexOf(f) + 1;
            return idx.ToString();
        }
        public string GetClassFieldAttrName(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_attrs == null) return null;
            return f.field_attrs.attr_name;
        }
        public string GetClassFieldAttrParam(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_attrs == null) return null;
            return f.field_attrs.attr_param;
        }
        public string GetClassFieldTagName(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_attrs != null && f.field_attrs.attr_type == eIDLAttr.TAG)
            {
                return f.field_attrs.attr_param;
            }
            return f.field_name;
        }
        public string GetClassFieldComment(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            return f.comment;
        }

        public string GetClassFieldMetaType(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            switch(f.field_type.type)
            {
                case eIDLType.BOOL:
                    return "bool";
                case eIDLType.STRING:
                    return "string";
                case eIDLType.INT:
                    return "int";
                case eIDLType.FLOAT:
                    return "float";
                case eIDLType.ENUM:
                    return "enum";
                case eIDLType.CLASS:
                    return "class";
                case eIDLType.DICT:
                    {
                        if (f.field_attrs != null && f.field_attrs.attr_type == eIDLAttr.STRING)
                            return "dict_string";
                        if (f.field_type.inner_type[1].type == eIDLType.CLASS)
                            return "dict_class";
                    }
                    return "error";
                case eIDLType.LIST:
                    {
                        if (f.field_attrs != null && f.field_attrs.attr_type == eIDLAttr.STRING)
                            return "list_string";
                    }
                    {
                        if (f.field_type.inner_type[0].type == eIDLType.CLASS)
                            return "list_class";
                    }
                    {
                        if (f.field_type.inner_type[0].type < eIDLType.CLASS)
                            return "list_basic";
                    }
                    return "error";
            }
            return GetFieldTypeName(f.field_type);
        }
        public string GetClassFieldType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.type != eIDLType.CLASS) return null;
            return GetFieldTypeName(f.field_type);
        }
        public string GetDictKeyType(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (f.field_type.type != eIDLType.DICT) return null;
            return GetFieldTypeName(f.field_type.inner_type[0]);

        }
        public string GetDictValueType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.type != eIDLType.DICT) return null;
            return GetFieldTypeName(f.field_type.inner_type[1]);
        }
        public string GetDictValueTag(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.type != eIDLType.DICT) return null;
            IDLType type = f.field_type.inner_type[1];
            IDLClass cls = IDLParser.FindUsingClass(f.Class.Meta, type.type_name);
            if (cls != null)
            {
                return GetClassTagName(cls);
            }
            return GetFieldTypeName(f.field_type.inner_type[1]);
        }

        public string GetListValueType(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.type != eIDLType.LIST) return null;
            return GetFieldTypeName(f.field_type.inner_type[0]);
        }
        public string GetListValueTag(object obj)
        {
            IDLClassField f = obj as IDLClassField;

            if (f.field_type.type != eIDLType.LIST) return null;
            IDLType type = f.field_type.inner_type[0];
            IDLClass cls = IDLParser.FindUsingClass(f.Class.Meta, type.type_name);
            if (cls != null)
            {
                return GetClassTagName(cls);
            }
            return GetFieldTypeName(f.field_type.inner_type[0]);
        }

        public virtual string GetClassFieldDefaultValue(object obj)
        {
            IDLClassField f = obj as IDLClassField;
            if (!string.IsNullOrEmpty(f.default_value))
            {
                return f.default_value;
            }
            if (f.field_type.type == eIDLType.BOOL)
            {
                return "false";
            }
            if (f.field_type.type == eIDLType.INT)
            {
                return "0";
            }
            if (f.field_type.type == eIDLType.FLOAT)
            {
                return "0.0f";
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

        //func

        public string ExecIteratorSelect(object obj)
        {
            var param = obj as Tuple<int, string>;
            var ss = param.Item2.Split(' ');
            if (param.Item1 == 0)
            {
                return ss[0];
            }
            return ss[1];
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
                    return "H3DDictionary<" + GetFieldTypeName(t.inner_type[0]) + "," + GetFieldTypeName(t.inner_type[1]) + ">";
            }
            return null;
        }
    }
}
