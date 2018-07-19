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
        AFTER_META_TEXT,//不作meta替换直接解析
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
        CLASS_FIELD_TYPE_ENUM,
        CLASS_FIELD_TYPE_DICT_STRING,
        CLASS_FIELD_TYPE_LIST_STRING,
        CLASS_FIELD_TYPE_CLASS_STRING,
        CLASS_FIELD_TYPE_STRING,
        CLASS_FIELD_TYPE_INT,
        CLASS_FIELD_TYPE_FLOAT,
        CLASS_FIELD_TYPE_BOOL,
        //if else
        CLASS_IF,
        CLASS_FIELD_IF,
    }

    class RuleMatchText
    {
        public string begin;
        public string end;
        public eTemplateRule type;
    }

    static class TemplateRuleMatcher
    {
        public static Dictionary<eTemplateRule, RuleMatchText> match_text = new Dictionary<eTemplateRule, RuleMatchText>();
        static TemplateRuleMatcher()
        {
            match_text.Add(eTemplateRule.META, new RuleMatchText { begin = "@{BEGIN_META}", end = "@{END_META}", type = eTemplateRule.META });
            match_text.Add(eTemplateRule.USING, new RuleMatchText { begin = "@{BEGIN_USING}", end = "@{END_USING}", type = eTemplateRule.USING });
            match_text.Add(eTemplateRule.ENUM, new RuleMatchText { begin = "@{BEGIN_ENUM}", end = "@{END_ENUM}", type = eTemplateRule.ENUM });
            match_text.Add(eTemplateRule.ENUM_FIELD, new RuleMatchText { begin = "@{BEGIN_ENUM_FIELD}", end = "@{END_ENUM_FIELD}", type = eTemplateRule.ENUM_FIELD });
            match_text.Add(eTemplateRule.CLASS, new RuleMatchText { begin = "@{BEGIN_CLASS}", end = "@{END_CLASS}", type = eTemplateRule.CLASS });
            match_text.Add(eTemplateRule.CLASS_ATTR_STRING, new RuleMatchText { begin = "@{BEGIN_CLASS_ATTR_STRING}", end = "@{END_CLASS_ATTR_STRING}", type = eTemplateRule.CLASS_ATTR_STRING });
            match_text.Add(eTemplateRule.CLASS_FIELD, new RuleMatchText { begin = "@{BEGIN_CLASS_FIELD}", end = "@{END_CLASS_FIELD}", type = eTemplateRule.CLASS_FIELD });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_DICT, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_DICT}", end = "@{END_FIELD_TYPE_DICT}", type = eTemplateRule.CLASS_FIELD_TYPE_DICT });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_LIST, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_LIST}", end = "@{END_FIELD_TYPE_LIST}", type = eTemplateRule.CLASS_FIELD_TYPE_LIST });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_LIST_BASIC, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_LIST_BASIC}", end = "@{END_FIELD_TYPE_LIST_BASIC}", type = eTemplateRule.CLASS_FIELD_TYPE_LIST_BASIC });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_LIST_STRING, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_LIST_STRING}", end = "@{END_FIELD_TYPE_LIST_STRING}", type = eTemplateRule.CLASS_FIELD_TYPE_LIST_STRING });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_CLASS, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_CLASS}", end = "@{END_FIELD_TYPE_CLASS}", type = eTemplateRule.CLASS_FIELD_TYPE_CLASS });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_CLASS_STRING, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_CLASS_STRING}", end = "@{END_FIELD_TYPE_CLASS_STRING}", type = eTemplateRule.CLASS_FIELD_TYPE_CLASS_STRING });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_STRING, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_STRING}", end = "@{END_FIELD_TYPE_STRING}", type = eTemplateRule.CLASS_FIELD_TYPE_STRING });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_INT, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_INT}", end = "@{END_FIELD_TYPE_INT}", type = eTemplateRule.CLASS_FIELD_TYPE_INT });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_FLOAT, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_FLOAT}", end = "@{END_FIELD_TYPE_FLOAT}", type = eTemplateRule.CLASS_FIELD_TYPE_FLOAT });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_BOOL, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_BOOL}", end = "@{END_FIELD_TYPE_BOOL}", type = eTemplateRule.CLASS_FIELD_TYPE_BOOL });
            match_text.Add(eTemplateRule.CLASS_FIELD_TYPE_ENUM, new RuleMatchText { begin = "@{BEGIN_FIELD_TYPE_ENUM}", end = "@{END_FIELD_TYPE_ENUM}", type = eTemplateRule.CLASS_FIELD_TYPE_ENUM });
            match_text.Add(eTemplateRule.CLASS_IF, new RuleMatchText { begin = @"@{BEGIN_CLASS_IF\(.+\)}", end = "@{END_CLASS_IF}", type = eTemplateRule.CLASS_IF });
            match_text.Add(eTemplateRule.CLASS_FIELD_IF, new RuleMatchText { begin = @"@{BEGIN_CLASS_FIELD_IF\(.+\)}", end = "@{END_CLASS_FIELD_IF}", type = eTemplateRule.CLASS_FIELD_IF });
        }

        static RuleMatchText text = new RuleMatchText { type = eTemplateRule.PLAIN_TEXT };
        public static RuleMatchText MatchBegin(string line)
        {
            foreach (var rmt in match_text.Values)
            {
                Regex reg = new Regex(rmt.begin);
                if (reg.IsMatch(line.Trim()))
                {
                    return rmt;
                }
            }
            return text;
        }

        public static bool MatchEnd(string line, RuleMatchText rmt)
        {
            return rmt.end == line.Trim();
        }
    }

    class TemplateRuleInfo
    {
        public List<string> rule_lines = new List<string>();
        public eTemplateRule rule_type = eTemplateRule.PLAIN_TEXT;
        public string rule_if_condition;

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
                case eTemplateRule.AFTER_META_TEXT:
                    return new RuleAfterMetaText() { RuleInfo = info };
                case eTemplateRule.PLAIN_TEXT:
                    return new RuleText() { RuleInfo = info };
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
                case eTemplateRule.CLASS_IF:
                    return new RuleClassIf() { RuleInfo = info };
                case eTemplateRule.CLASS_FIELD_IF:
                    return new RuleClassFieldIf { RuleInfo = info };
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
                    if (rule_match.type == eTemplateRule.PLAIN_TEXT)
                    {
                        info.rule_lines.Add(rule_lines[i]);
                        continue;
                    }

                    if (info.rule_lines.Count > 0)
                    {
                        extend_rules.Add(TemplateRuleFactory.CreateRule(info));
                    }

                    info = new TemplateRuleInfo();
                    info.rule_type = rule_match.type;
                    if (info.rule_type == eTemplateRule.CLASS_FIELD_IF || info.rule_type == eTemplateRule.CLASS_IF)
                    {
                        info.rule_if_condition = rule_lines[i];
                    }
                    continue;
                }

                if (TemplateRuleMatcher.MatchEnd(rule_lines[i], rule_match))
                {
                    if (info.rule_lines.Count > 0)
                    {
                        extend_rules.Add(TemplateRuleFactory.CreateRule(info));
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
                    extend_rules.Add(TemplateRuleFactory.CreateRule(info));
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

    class RuleAfterMetaText : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }

        public List<string> Apply(object obj, TemplateData data)
        {
            List<string> result = new List<string>();
            List<IDLMeta> meta = obj as List<IDLMeta>;

            //解析line生成新的rule
            List<ITemplateRule> extend_rules = TemplateRuleFactory.Parse(RuleInfo.rule_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(meta, data);
                result.AddRange(ss);
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

    class RuleText : ITemplateRule
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

    abstract class RuleIf : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public virtual string ConditionMatchText { get; }
        public virtual string ElseMatchText { get; }

        public bool CheckIfCondition(string s)
        {
            string[] ss = s.Split(',');
            if (ss == null || ss.Length == 0)
            {
                return false;
            }
            switch (ss[0])
            {
                case "EQ":
                    return ss[1] == ss[2];
            }

            return false;
        }

        public List<string> Apply(object obj, TemplateData data)
        {
            Regex reg = new Regex(@"@{\w+\((.+)\)}");
            Match m = reg.Match(RuleInfo.rule_if_condition);
            if (!m.Success)
            {
                Console.WriteLine("{0}条件不匹配 {1}", RuleInfo.rule_type, RuleInfo.rule_if_condition);
                return null;
            }

            string condition = m.Groups[1].Value;
            bool ok = CheckIfCondition(condition);

            List<string> extend_lines = new List<string>();

            bool begin_else = false; //寻找else
            foreach (string s in RuleInfo.rule_lines)
            {
                if (s.Trim() == ElseMatchText)
                {
                    begin_else = true;
                    continue;
                }
                if (ok)
                {
                    if (begin_else) break;
                    extend_lines.Add(s);
                }
                else
                {
                    if (begin_else)
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
    class RuleClassIf : RuleIf
    {
        public override string ConditionMatchText { get { return TemplateRuleMatcher.match_text[eTemplateRule.CLASS_IF].begin; } }
        public override string ElseMatchText { get { return "@{BEGIN_CLASS_ELSE}"; } }
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
    class RuleClassField : ITemplateRule
    {
        public TemplateRuleInfo RuleInfo { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            IDLClass cls = obj as IDLClass;
            List<string> result = new List<string>();

            foreach (var field in cls.fieldList)
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

            return result;
        }
    }

    //已经field展开过了
    class RuleClassFieldIf : RuleIf
    {
        public override string ConditionMatchText { get { return TemplateRuleMatcher.match_text[eTemplateRule.CLASS_FIELD_IF].begin; } }
        public override string ElseMatchText { get { return "@{BEGIN_CLASS_FIELD_ELSE}"; } }
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

    abstract class RuleClassFieldTypeCase : ITemplateRule
    {
        protected virtual bool CheckFieldCase(IDLClassField field) { return false; }
        public TemplateRuleInfo RuleInfo { get; set; }
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
    class RuleClassFieldTypeDict : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.DICT && (field.field_attrs == null || field.field_attrs.attr_type == eIDLAttr.NOATTR));
        }
    }

    class RuleClassFieldTypeList : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST
                && (field.field_attrs == null || field.field_attrs.attr_type == eIDLAttr.NOATTR)
                && field.field_type.inner_type[0].type == eIDLType.CLASS);
        }

    }
    class RuleClassFieldTypeListBasic : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST
                && (field.field_attrs == null || field.field_attrs.attr_type == eIDLAttr.NOATTR)
                && field.field_type.inner_type[0].type < eIDLType.CLASS);
        }

    }

    class RuleClassFieldTypeDictString : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.DICT && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }

    }
    class RuleClassFieldTypeListString : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.LIST && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }

    }
    class RuleClassFieldTypeClass : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.CLASS && (field.field_attrs == null || field.field_attrs.attr_type == eIDLAttr.NOATTR));
        }

    }
    class RuleClassFieldTypeClassString : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.CLASS && field.field_attrs != null && field.field_attrs.attr_type == eIDLAttr.STRING);
        }

    }
    class RuleClassFieldTypeString : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.STRING);
        }
    }

    class RuleClassFieldTypeInt : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.INT);
        }

    }
    class RuleClassFieldTypeFloat : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return (field.field_type.type == eIDLType.FLOAT);
        }
    }
    class RuleClassFieldTypeEnum : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return field.field_type.type == eIDLType.ENUM;
        }
    }

    class RuleClassFieldTypeBool : RuleClassFieldTypeCase
    {
        protected override bool CheckFieldCase(IDLClassField field)
        {
            return field.field_type.type == eIDLType.BOOL;
        }
    }


}
