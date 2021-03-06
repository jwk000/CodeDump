﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CodeDump
{
    enum eTemplateRule
    {
        METATEXT,
        IF,
        SWITCH,
        FOREACH,
    }


    class RuleMatchInfo
    {
        public eTemplateRule rule_type = eTemplateRule.METATEXT;
        public string match_text_begin;
        public string match_text_end;
        public int match_deepth;

    }
    interface ITemplateRule
    {
        RuleMatchInfo match_info { get; set; }
        List<string> rule_lines { get; set; }
        string rule_param_line { get; set; }
        List<string> Apply(TemplateData data);
    }
    class RuleMetaText : ITemplateRule
    {
        public RuleMatchInfo match_info { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_param_line { get; set; }
        public List<string> Apply(TemplateData data)
        {
            List<string> result = new List<string>();
            foreach (var line in rule_lines)
            {
                string s = data.ExtendMetaData(line);
                result.Add(s);
            }

            return result;
        }

    }
    class RuleIf : ITemplateRule
    {
        public RuleMatchInfo match_info { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_param_line { get; set; }

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
        public bool CheckIfLine(string line)
        {
            Regex reg = new Regex(@"@{\w+\((.+)\)}");
            Match m = reg.Match(line);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", match_info.rule_type, rule_param_line);
                return false;
            }

            string condition = m.Groups[1].Value;
            return CheckIfCondition(condition);
        }
        public List<string> Apply(TemplateData data)
        {
            bool condition_is_true = CheckIfLine(rule_param_line);
            List<string> extend_lines = new List<string>();

            bool find_else = false; //寻找else
            foreach (string s in rule_lines)
            {
                if (s.Trim() == ElseMatchText)
                {
                    find_else = true;
                    continue;
                }
                //寻找其他if
                if (Regex.IsMatch(s, @"@{ELSEIF\(.+\)}"))
                {
                    if (condition_is_true)
                    {
                        find_else = true;
                    }
                    else
                    {
                        condition_is_true = CheckIfLine(s);
                    }
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
            List<ITemplateRule> extend_rules = TemplateRuleParser.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(data);
                result.AddRange(ss);
            }

            return result;
        }
    }
    class RuleSwitch : ITemplateRule
    {
        public RuleMatchInfo match_info { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_param_line { get; set; }

        public List<string> Apply(TemplateData data)
        {
            Regex reg = new Regex(@"@{SWITCH\((.+)\)}");
            Match m = reg.Match(rule_param_line);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", match_info.rule_type, rule_param_line);
                return null;
            }

            string condition = m.Groups[1].Value;
            condition = data.ExtraMetaData(condition) as string;
            Regex regcase = new Regex(@"@{CASE\((.+)\)}");
            bool find_case = false; //寻找else

            List<string> extend_lines = new List<string>();
            foreach (string s in rule_lines)
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
            List<ITemplateRule> extend_rules = TemplateRuleParser.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(data);
                result.AddRange(ss);
            }

            return result;
        }

    }
    class RuleForeach : ITemplateRule
    {
        public RuleMatchInfo match_info { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_param_line { get; set; }

        public List<string> Apply(TemplateData data)
        {
            Regex reg = new Regex(@"@{FOREACH\((\w+)\s+IN\s+(.+)\)}");
            Match m = reg.Match(rule_param_line);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", match_info.rule_type, rule_param_line);
                return null;
            }

            string var_name = m.Groups[1].Value;
            string extra = m.Groups[2].Value;
            object[] var_array = data.ExtraMetaData(extra) as object[];

            List<string> result = new List<string>();
            int index = 0;
            foreach (object v in var_array)
            {
                data.SetLocalVariant(var_name, v);
                data.SetLocalVariant("ForeachIndex", index.ToString());
                index++;
                List<string> res = new List<string>();
                List<ITemplateRule> extend_rules = TemplateRuleParser.Parse(rule_lines);
                foreach (var rule in extend_rules)
                {
                    var ss = rule.Apply(data);
                    res.AddRange(ss);
                }
                result.AddRange(res);
            }


            return result;
        }

    }
    //规则解析器
    static class TemplateRuleParser
    {
        static RuleMatchInfo matchtext = new RuleMatchInfo { rule_type = eTemplateRule.METATEXT };
        static List<RuleMatchInfo> matchlist = new List<RuleMatchInfo>();
        static TemplateRuleParser()
        {
            matchlist.Add(new RuleMatchInfo { match_text_begin = @"@{IF\(.+\)}", match_text_end = "@{END_IF}", rule_type = eTemplateRule.IF });
            matchlist.Add(new RuleMatchInfo { match_text_begin = @"@{SWITCH\(.+\)}", match_text_end = "@{END_SWITCH}", rule_type = eTemplateRule.SWITCH });
            matchlist.Add(new RuleMatchInfo { match_text_begin = @"@{FOREACH\(.+\)}", match_text_end = "@{END_FOREACH}", rule_type = eTemplateRule.FOREACH });
        }
        public static ITemplateRule CreateRule(RuleMatchInfo info)
        {
            ITemplateRule rule = null;

            if (info.rule_type == eTemplateRule.METATEXT) rule = new RuleMetaText();
            else if (info.rule_type == eTemplateRule.IF) rule = new RuleIf();
            else if (info.rule_type == eTemplateRule.SWITCH) rule = new RuleSwitch();
            else if (info.rule_type == eTemplateRule.FOREACH) rule = new RuleForeach();

            if (rule != null)
            {
                rule.match_info = info;
                rule.rule_lines = new List<string>();
            }
            return rule;
        }

        public static RuleMatchInfo MatchBegin(string line)
        {
            foreach (var match in matchlist)
            {
                Regex reg = new Regex(match.match_text_begin);
                if (reg.IsMatch(line.Trim()))
                {
                    match.match_deepth++;
                    return match;
                }
            }
            return matchtext;
        }
        public static bool MatchEnd(string line, RuleMatchInfo matchtxt)
        {
            //寻找end的时候要检查深度
            Regex reg = new Regex(matchtxt.match_text_begin);
            if (reg.IsMatch(line.Trim()))
            {
                matchtxt.match_deepth++;
                return false;
            }

            if (matchtxt.match_text_end == line.Trim())
            {
                matchtxt.match_deepth--;
                if (matchtxt.match_deepth == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static List<ITemplateRule> Parse(List<string> rule_lines)
        {
            List<ITemplateRule> extend_rules = new List<ITemplateRule>();

            RuleMatchInfo info = null;
            ITemplateRule rule = null;
            for (int i = 0; i < rule_lines.Count; i++)
            {
                if (info == null || info.rule_type == eTemplateRule.METATEXT)
                {
                    var match_info = MatchBegin(rule_lines[i]);
                    if (match_info != info)
                    {
                        if (rule != null && rule.rule_lines.Count > 0)
                        {
                            extend_rules.Add(rule);
                        }
                        info = match_info;
                        rule = CreateRule(info);
                        if (info.rule_type != eTemplateRule.METATEXT)
                        {
                            //匹配行含有参数
                            rule.rule_param_line = rule_lines[i];
                            //跳过匹配行
                            continue;
                        }
                    }

                    rule.rule_lines.Add(rule_lines[i]);
                    continue;
                }

                if (MatchEnd(rule_lines[i], info))
                {
                    if (rule.rule_lines.Count > 0)
                    {
                        extend_rules.Add(rule);
                    }
                    info = null;
                    rule = null;
                    continue;
                }

                rule.rule_lines.Add(rule_lines[i]);

            }
            if (rule != null && rule.rule_lines.Count > 0)
            {
                extend_rules.Add(rule);
            }
            return extend_rules;
        }

    }
    //模板数据
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
            globalVariantDict[name] = obj;
        }

        public void SetLocalVariant(string name, object obj)
        {
            localVariantDict[name] = obj;
        }


        public object GetVariantObject(string objname, string fieldname)
        {
            object obj = null;
            if (objname == "G")//直接映射表
            {
                if (localVariantDict.TryGetValue(fieldname, out obj))
                {
                    return obj;
                }
                if (globalVariantDict.TryGetValue(fieldname, out obj))
                {
                    return obj;
                }
            }

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
            if (!m.Success)
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
