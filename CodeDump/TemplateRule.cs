using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeDump
{
    enum eTemplateRule
    {
        PLAIN_TEXT,//没有规则

        META_TEXT,//对所有line应用meta替换

        META,//对所有meta应用规则

        USING,//对所有using应用规则

        ENUM,//对所有enum应用规则

        ENUM_FIELD,//对所有enum field应用规则

        CLASS,//对所有class应用规则
        CLASS_ATTR_STRING,//对带有string特性的class应用规则

        CLASS_FIELD,//对所有class field应用规则
        //switch case
        CLASS_FIELD_TYPE_DICT,//根据不同类型选用规则
        CLASS_FIELD_TYPE_LIST,//字段类型为List<class>时使用此规则
        CLASS_FIELD_TYPE_LIST_BASIC,//字段类型为List<基本类型>时使用此规则
        CLASS_FIELD_TYPE_CLASS,//字段类型为class时使用此规则
        CLASS_FIELD_TYPE_DICT_STRING,
        CLASS_FIELD_TYPE_LIST_STRING,
        CLASS_FIELD_TYPE_CLASS_STRING,
        CLASS_FIELD_TYPE_ENUM,
        CLASS_FIELD_TYPE_STRING,
        CLASS_FIELD_TYPE_INT,
        CLASS_FIELD_TYPE_FLOAT,
        CLASS_FIELD_TYPE_BOOL,
        CLASS_FIELD_TYPE_BASIC,//int float bool string

        //if else
        IF,
        IF0,
        IF1,
        IF2,
        //switch case
        SWITCH,

    }

    class RuleMatchText
    {
        public string match_text_begin;
        public string match_text_end;
        public eTemplateRule rule_type = eTemplateRule.PLAIN_TEXT;
    }

    static class TemplateRuleMatcher
    {
        public static List<RuleMatchText> match_text = new List<RuleMatchText>();
        static TemplateRuleMatcher()
        {
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_META}", match_text_end = "@{END_META}", rule_type = eTemplateRule.META });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_USING}", match_text_end = "@{END_USING}", rule_type = eTemplateRule.USING });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_ENUM}", match_text_end = "@{END_ENUM}", rule_type = eTemplateRule.ENUM });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_ENUM_FIELD}", match_text_end = "@{END_ENUM_FIELD}", rule_type = eTemplateRule.ENUM_FIELD });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_CLASS}", match_text_end = "@{END_CLASS}", rule_type = eTemplateRule.CLASS });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_CLASS_FIELD}", match_text_end = "@{END_CLASS_FIELD}", rule_type = eTemplateRule.CLASS_FIELD });

            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_CLASS_ATTR_STRING}", match_text_end = "@{END_CLASS_ATTR_STRING}", rule_type = eTemplateRule.CLASS_ATTR_STRING });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_DICT}", match_text_end = "@{END_FIELD_TYPE_DICT}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_DICT });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_LIST}", match_text_end = "@{END_FIELD_TYPE_LIST}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_LIST });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_LIST_BASIC}", match_text_end = "@{END_FIELD_TYPE_LIST_BASIC}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_LIST_BASIC });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_LIST_STRING}", match_text_end = "@{END_FIELD_TYPE_LIST_STRING}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_LIST_STRING });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_CLASS}", match_text_end = "@{END_FIELD_TYPE_CLASS}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_CLASS });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_CLASS_STRING}", match_text_end = "@{END_FIELD_TYPE_CLASS_STRING}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_CLASS_STRING });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_STRING}", match_text_end = "@{END_FIELD_TYPE_STRING}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_STRING });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_INT}", match_text_end = "@{END_FIELD_TYPE_INT}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_INT });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_FLOAT}", match_text_end = "@{END_FIELD_TYPE_FLOAT}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_FLOAT });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_BOOL}", match_text_end = "@{END_FIELD_TYPE_BOOL}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_BOOL });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_ENUM}", match_text_end = "@{END_FIELD_TYPE_ENUM}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_ENUM });
            match_text.Add(new RuleMatchText { match_text_begin = "@{BEGIN_FIELD_TYPE_BASIC}", match_text_end = "@{END_FIELD_TYPE_BASIC}", rule_type = eTemplateRule.CLASS_FIELD_TYPE_BASIC });

            match_text.Add(new RuleMatchText { match_text_begin = @"@{SWITCH\(.+\)}", match_text_end = "@{END_SWITCH}", rule_type = eTemplateRule.SWITCH });
            match_text.Add(new RuleMatchText { match_text_begin = @"@{IF\(.+\)}", match_text_end = "@{END_IF}", rule_type = eTemplateRule.IF });
            match_text.Add(new RuleMatchText { match_text_begin = @"@{IF0\(.+\)}", match_text_end = "@{END_IF0}", rule_type = eTemplateRule.IF0 });
            match_text.Add(new RuleMatchText { match_text_begin = @"@{IF1\(.+\)}", match_text_end = "@{END_IF1}", rule_type = eTemplateRule.IF1 });
            match_text.Add(new RuleMatchText { match_text_begin = @"@{IF2\(.+\)}", match_text_end = "@{END_IF2}", rule_type = eTemplateRule.IF2 });

        }

        static RuleMatchText text = new RuleMatchText { rule_type = eTemplateRule.PLAIN_TEXT };
        public static RuleMatchText MatchBegin(string line)
        {
            foreach (var matchtxt in match_text)
            {
                Regex reg = new Regex(matchtxt.match_text_begin);
                if (reg.IsMatch(line.Trim()))
                {
                    return matchtxt;
                }
            }
            return text;
        }

        public static bool MatchEnd(string line, RuleMatchText rmt)
        {
            return rmt.match_text_end == line.Trim();
        }
    }

    class TemplateRuleInfo
    {
        public List<string> rule_lines = new List<string>();
        public eTemplateRule rule_type = eTemplateRule.PLAIN_TEXT;
        public string rule_params;

    }
    interface ITemplateRule
    {
        TemplateRuleInfo RuleInfo { get; set; }
        List<string> Apply(object obj, TemplateData data);
    }

    static class TemplateRuleFactory
    {
        public static ITemplateRule CreateRule(TemplateRuleInfo info)
        {
            switch (info.rule_type)
            {
                case eTemplateRule.META:
                    return new RuleMeta() { RuleInfo = info };
                case eTemplateRule.META_TEXT:
                    return new RuleMetaText() { RuleInfo = info };
                case eTemplateRule.PLAIN_TEXT:
                    return new RulePlainText() { RuleInfo = info };
                case eTemplateRule.USING:
                    return new RuleUsing() { RuleInfo = info };
                case eTemplateRule.CLASS:
                    return new RuleClass() { RuleInfo = info };
                case eTemplateRule.CLASS_ATTR_STRING:
                    return new RuleClassAttrString() { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD:
                    return new RuleClassField() { RuleInfo = info };
                case eTemplateRule.ENUM:
                    return new RuleEnum() { RuleInfo = info };
                case eTemplateRule.ENUM_FIELD:
                    return new RuleEnumField() { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_ENUM:
                    return new RuleClassFieldTypeEnum { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_CLASS:
                    return new RuleClassFieldTypeClass { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_CLASS_STRING:
                    return new RuleClassFieldTypeClassString { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_DICT:
                    return new RuleClassFieldTypeDict { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_DICT_STRING:
                    return new RuleClassFieldTypeDictString { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_LIST:
                    return new RuleClassFieldTypeList { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_LIST_BASIC:
                    return new RuleClassFieldTypeListBasic { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_LIST_STRING:
                    return new RuleClassFieldTypeListString { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_STRING:
                    return new RuleClassFieldTypeString { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_INT:
                    return new RuleClassFieldTypeInt { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_FLOAT:
                    return new RuleClassFieldTypeFloat { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_BOOL:
                    return new RuleClassFieldTypeBool { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_TYPE_BASIC:
                    return new RuleClassFieldTypeBasic { RuleInfo = info };
                case eTemplateRule.IF:
                    return new RuleIf { RuleInfo = info };
                case eTemplateRule.IF0:
                    return new RuleIf0 { RuleInfo = info };
                case eTemplateRule.IF1:
                    return new RuleIf1 { RuleInfo = info };
                case eTemplateRule.IF2:
                    return new RuleIf2 { RuleInfo = info };
                case eTemplateRule.SWITCH:
                    return new RuleSwitch { RuleInfo = info };
            }
            return null;
        }

        public static List<ITemplateRule> Parse(List<string> rule_lines)
        {
            List<ITemplateRule> extend_rules = new List<ITemplateRule>();

            RuleMatchText rule_match = null;

            TemplateRuleInfo info = new TemplateRuleInfo();

            for (int i = 0; i < rule_lines.Count; i++)
            {
                if (info.rule_type == eTemplateRule.PLAIN_TEXT)
                {
                    rule_match = TemplateRuleMatcher.MatchBegin(rule_lines[i]);
                    if (rule_match.rule_type == eTemplateRule.PLAIN_TEXT)
                    {
                        info.rule_lines.Add(rule_lines[i]);
                        continue;
                    }

                    if (info.rule_lines.Count > 0)
                    {
                        extend_rules.Add(CreateRule(info));
                    }

                    info = new TemplateRuleInfo();
                    info.rule_type = rule_match.rule_type;
                    info.rule_params = rule_lines[i];

                    continue;
                }

                if (TemplateRuleMatcher.MatchEnd(rule_lines[i], rule_match))
                {
                    if (info.rule_lines.Count > 0)
                    {
                        extend_rules.Add(CreateRule(info));
                    }
                    info = new TemplateRuleInfo();
                    continue;
                }

                info.rule_lines.Add(rule_lines[i]);

            }
            if (info.rule_type == eTemplateRule.PLAIN_TEXT)
            {
                if (info.rule_lines.Count > 0)
                {
                    extend_rules.Add(CreateRule(info));
                }
            }
            return extend_rules;
        }

    }

    class RuleMeta : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            List<string> result = new List<string>();
            List<IDLMeta> meta_list = obj as List<IDLMeta>;
            foreach (var line in RuleInfo.rule_lines)
            {
                foreach (var meta in meta_list)
                {
                    string s = data.ExtendMetaData(line, meta);
                    result.Add(s);
                }
            }
            return result;
        }
    }


    class RuleMetaText : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }

        public List<string> Apply(object obj, TemplateData data)
        {
            List<string> result = new List<string>();
            IDLMeta meta = obj as IDLMeta;
            List<string> extend_lines = new List<string>();
            foreach (var line in RuleInfo.rule_lines)
            {
                string s = data.ExtendMetaData(line, meta);
                extend_lines.Add(s);
            }

            //解析line生成新的rule
            List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(meta, data);
                result.AddRange(ss);
            }
            return result;
        }

    }

    class RulePlainText : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            return RuleInfo.rule_lines;
        }
    }
    class RuleUsing : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLMeta meta = obj as IDLMeta;
            List<string> result = new List<string>();
            foreach (var u in meta.using_meta)
            {
                foreach (string line in RuleInfo.rule_lines)
                {
                    string s = data.ExtendMetaUsingData(line, u);
                    result.Add(s);
                }
            }
            return result;
        }

    }

    class RuleClass : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLMeta meta = obj as IDLMeta;
            List<string> result = new List<string>();
            foreach (var cls in meta.meta_class.Values)
            {
                List<string> extend_lines = new List<string>();
                foreach (string line in RuleInfo.rule_lines)
                {
                    string s = data.ExtendMetaClassData(line, cls);
                    extend_lines.Add(s);
                }

                //继续展开
                List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
                foreach (var rule in extend_rules)
                {
                    var ss = rule.Apply(cls, data);
                    result.AddRange(ss);
                }

            }

            return result;
        }
    }


    class RuleClassAttrCase : ITemplateRule
    {
        protected virtual bool CheckAttrCase(IDLClass c) { return false; }
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLClass cls = obj as IDLClass;
            List<string> result = new List<string>();

            if (CheckAttrCase(cls))
            {
                //继续展开
                List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(RuleInfo.rule_lines);
                foreach (var rule in extend_rules)
                {
                    var ss = rule.Apply(cls, data);
                    result.AddRange(ss);
                }
            }

            return result;
        }
    }
    class RuleClassAttrString : RuleClassAttrCase
    {
        protected override bool CheckAttrCase(IDLClass c)
        {
            return (c.class_attr != null && c.class_attr.attr_type == eIDLAttr.STRING);
        }
    }
    class RuleEnum : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLMeta meta = obj as IDLMeta;
            List<string> result = new List<string>();
            foreach (var e in meta.meta_enum.Values)
            {
                List<string> extend_lines = new List<string>();
                foreach (string line in RuleInfo.rule_lines)
                {
                    string s = data.ExtendMetaEnumData(line, e);
                    extend_lines.Add(s);
                }

                //继续展开
                List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
                foreach (var rule in extend_rules)
                {
                    var ss = rule.Apply(e, data);
                    result.AddRange(ss);
                }

            }

            return result;
        }

    }
    class RuleEnumField : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLEnum enu = obj as IDLEnum;
            List<string> result = new List<string>();

            foreach (var field in enu.enum_fields.Values)
            {
                foreach (string line in RuleInfo.rule_lines)
                {
                    string s = data.ExtendMetaEnumFieldData(line, field);
                    result.Add(s);
                }
            }

            return result;
        }

    }

    class RuleClassField : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        protected virtual bool CheckFieldCase(IDLClassField field) { return true; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLClass cls = obj as IDLClass;
            List<string> result = new List<string>();

            foreach (var field in cls.fieldList)
            {
                if (CheckFieldCase(field))
                {
                    List<string> extend_lines = new List<string>();
                    foreach (string line in RuleInfo.rule_lines)
                    {
                        string s = data.ExtendMetaClassFieldData(line, field);
                        extend_lines.Add(s);
                    }

                    //继续展开，还是field级别
                    List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
                    foreach (var rule in extend_rules)
                    {
                        var ss = rule.Apply(field, data);
                        result.AddRange(ss);
                    }
                }
            }

            return result;
        }
    }



    //dict没有basic情况，因为第二个参数需要包含key
    class RuleClassFieldTypeDict : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.DICT
                && (field.field_attrs == null || field.field_attrs.attr_type != eIDLAttr.STRING)
                && field.field_type.inner_type[1].type == eIDLType.CLASS);
        }
    }

    class RuleClassFieldTypeList : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST
                && (field.field_attrs == null || field.field_attrs.attr_type != eIDLAttr.STRING)
                && field.field_type.inner_type[0].type == eIDLType.CLASS);
        }

    }
    class RuleClassFieldTypeListBasic : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST
                && (field.field_attrs == null || field.field_attrs.attr_type != eIDLAttr.STRING)
                && field.field_type.inner_type[0].type < eIDLType.CLASS);
        }

    }

    class RuleClassFieldTypeDictString : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.DICT
                && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }
    }
    class RuleClassFieldTypeListString : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST
                && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }

    }
    class RuleClassFieldTypeClass : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.CLASS
                && (field.field_attrs == null || field.field_attrs.attr_type != eIDLAttr.STRING));
        }

    }
    class RuleClassFieldTypeClassString : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.CLASS
                && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }

    }
    class RuleClassFieldTypeString : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.STRING);
        }
    }

    class RuleClassFieldTypeInt : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.INT);
        }

    }
    class RuleClassFieldTypeFloat : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.FLOAT);
        }
    }
    class RuleClassFieldTypeEnum : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return field.field_type.type == eIDLType.ENUM;
        }
    }

    class RuleClassFieldTypeBool : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return field.field_type.type == eIDLType.BOOL;
        }
    }

    class RuleClassFieldTypeBasic : RuleClassField
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.BOOL ||
                field.field_type.type == eIDLType.FLOAT ||
                field.field_type.type == eIDLType.INT ||
                field.field_type.type == eIDLType.STRING);
        }
    }

    class RuleIf : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public virtual string ElseMatchText => "@{ELSE}";

        public bool CheckIfCondition(string cond)
        {
            string[] ss = cond.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            if (ss.Length > 1)
            {
                bool ret = false;
                foreach (string s in ss)
                {
                    ret = ret || CheckIfCondition(s);
                    if (ret)
                    {
                        return true;
                    }
                }
                return false;
            }

            ss = cond.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length > 1)
            {
                bool ret = true;
                foreach (string s in ss)
                {
                    ret = ret && CheckIfCondition(s);
                    if (!ret)
                    {
                        return false;
                    }
                }
                return true;

            }

            ss = cond.Split(new string[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length == 2)
            {
                return (ss[0]) == (ss[1]);
            }

            ss = cond.Split(new string[] { "!=" }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length == 2)
            {
                return (ss[0]) != (ss[1]);
            }

            if (cond == "true") return true;
            if (cond == "false") return false;

            return false;
        }

        public List<string> Apply(object obj, TemplateData data)
        {
            Regex reg = new Regex(@"@{\w+\((.+)\)}");
            Match m = reg.Match(RuleInfo.rule_params);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", RuleInfo.rule_type, RuleInfo.rule_params);
                return null;
            }

            string condition = m.Groups[1].Value;
            bool condition_is_true = CheckIfCondition(condition);

            List<string> extend_lines = new List<string>();

            bool find_else = false; //寻找else
            foreach (string s in RuleInfo.rule_lines)
            {
                if (s.Trim() == ElseMatchText)
                {
                    find_else = true;
                    continue;
                }
                if (condition_is_true)
                {
                    if (find_else) break;
                    extend_lines.Add(s);
                }
                else
                {
                    if (find_else)
                    {
                        extend_lines.Add(s);
                    }
                }
            }

            //继续展开
            List<string> result = new List<string>();
            List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(obj, data);
                result.AddRange(ss);
            }

            return result;
        }
    }
    class RuleIf0 : RuleIf
    {
        public override string ElseMatchText => "@{ELSE0}";
    }
    class RuleIf1 : RuleIf
    {
        public override string ElseMatchText => "@{ELSE1}";
    }
    class RuleIf2 : RuleIf
    {
        public override string ElseMatchText => "@{ELSE2}";
    }

    class RuleSwitch : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }

        public List<string> Apply(object obj, TemplateData data)
        {
            Regex reg = new Regex(@"@{SWITCH\((.+)\)}");
            Match m = reg.Match(RuleInfo.rule_params);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", RuleInfo.rule_type, RuleInfo.rule_params);
                return null;
            }

            string condition = m.Groups[1].Value;

            Regex regcase = new Regex(@"@{CASE\((.+)\)}");
            bool find_case = false; //寻找else

            List<string> extend_lines = new List<string>();
            foreach (string s in RuleInfo.rule_lines)
            {
                if (regcase.IsMatch(s))
                {
                    if (find_case)
                        break;

                    string casee = regcase.Match(s).Groups[1].Value;
                    string[] ss = casee.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Contains(condition))
                    {
                        find_case = true;
                        continue;
                    }
                }
                if (find_case)
                {
                    extend_lines.Add(s);
                }
            }

            //继续展开
            List<string> result = new List<string>();
            List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(obj, data);
                result.AddRange(ss);
            }

            return result;
        }

    }
}
