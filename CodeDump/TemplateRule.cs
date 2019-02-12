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


        public ITemplateRule CreateRule()
        {
            ITemplateRule rule = null;

            if (rule_type == eTemplateRule.METATEXT) rule = new RuleMeta();
            else if (rule_type == eTemplateRule.IF) rule = new RuleIf();
            else if (rule_type == eTemplateRule.SWITCH) rule = new RuleSwitch();
            else if (rule_type == eTemplateRule.FOREACH) rule = new RuleForeach();

            if (rule != null)
            {
                rule.RuleInfo = this;
            }
            return rule;

        }
    }
    interface ITemplateRule
    {
        RuleMatchInfo RuleInfo { get; set; }
        List<string> rule_lines { get; set; }
        string rule_params { get; set; }

        List<string> Apply(object obj, TemplateData data);
    }

    static class TemplateRuleParser
    {
        static RuleMatchInfo text = new RuleMatchInfo { rule_type = eTemplateRule.METATEXT };
        static List<RuleMatchInfo> match_rule_info = new List<RuleMatchInfo>();
        static TemplateRuleParser()
        {
            match_rule_info.Add(new RuleMatchInfo { match_text_begin = @"@{IF\(.+\)}", match_text_end = "@{END_IF}", rule_type = eTemplateRule.IF});
            match_rule_info.Add(new RuleMatchInfo { match_text_begin = @"@{SWITCH\(.+\)}", match_text_end = "@{END_SWITCH}", rule_type = eTemplateRule.SWITCH });
            match_rule_info.Add(new RuleMatchInfo { match_text_begin = @"@{FOREACH\(.+\)}", match_text_end = "@{END_FOREACH}", rule_type = eTemplateRule.FOREACH });
        }

        public static RuleMatchInfo MatchBegin(string line)
        {
            foreach (var matchtxt in match_rule_info)
            {
                Regex reg = new Regex(matchtxt.match_text_begin);
                if (reg.IsMatch(line.Trim()))
                {
                    matchtxt.match_deepth++;
                    return matchtxt;
                }
            }
            return text;
        }

        public static bool MatchEnd(string line, RuleMatchInfo rmt)
        {
            if (rmt.match_text_end == line.Trim())
            {
                rmt.match_deepth--;
                if (rmt.match_deepth == 0)
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
                if (info==null || info.rule_type == eTemplateRule.METATEXT)
                {
                    info = MatchBegin(rule_lines[i]);
                    rule = info.CreateRule();
                    if (info.rule_type == eTemplateRule.METATEXT)
                    {
                        rule.rule_lines.Add(rule_lines[i]);
                        continue;
                    }

                    if (rule.rule_lines.Count > 0)
                    {
                        extend_rules.Add(rule);
                    }
                    //匹配行含有参数
                    rule.rule_params = rule_lines[i];
                    info = null;
                    continue;
                }

                if (MatchEnd(rule_lines[i], info))
                {
                    if (rule.rule_lines.Count > 0)
                    {
                        extend_rules.Add(rule);
                    }
                    info = null;
                    continue;
                }

                rule.rule_lines.Add(rule_lines[i]);

            }

            return extend_rules;
        }

    }

    class RuleMeta:ITemplateRule
    {
        public RuleMatchInfo RuleInfo { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_params { get; set; }
        public List<string> Apply(object obj, TemplateData data)
        {
            List<string> result = new List<string>();
            foreach (var line in rule_lines)
            {
                string s = data.ExtendMetaData(obj, line);
                result.Add(s);
            }

            return result;
        }

    }
    class RulePlainText : ITemplateRule
    {
        public RuleMatchInfo RuleInfo { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_params { get; set; }

        public List<string> Apply(object obj, TemplateData data)
        {
            return rule_lines;
        }
    }


    class RuleIf : ITemplateRule
    {
        public RuleMatchInfo RuleInfo { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_params { get; set; }

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
            Match m = reg.Match(rule_params);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", RuleInfo.rule_type, rule_params);
                return null;
            }

            string condition = m.Groups[1].Value;
            bool condition_is_true = CheckIfCondition(condition);

            List<string> extend_lines = new List<string>();

            bool find_else = false; //寻找else
            foreach (string s in rule_lines)
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
            List<ITemplateRule> extend_rules = TemplateRuleParser.Parse(extend_lines);
            foreach (var rule in extend_rules)
            {
                var ss = rule.Apply(obj, data);
                result.AddRange(ss);
            }

            return result;
        }
    }
    class RuleSwitch : ITemplateRule
    {
        public RuleMatchInfo RuleInfo { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_params { get; set; }

        public List<string> Apply(object obj, TemplateData data)
        {
            Regex reg = new Regex(@"@{SWITCH\((.+)\)}");
            Match m = reg.Match(rule_params);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", RuleInfo.rule_type, rule_params);
                return null;
            }

            string condition = m.Groups[1].Value;

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
                var ss = rule.Apply(obj, data);
                result.AddRange(ss);
            }

            return result;
        }

    }
    class RuleForeach : ITemplateRule
    {
        public RuleMatchInfo RuleInfo { get; set; }
        public List<string> rule_lines { get; set; }
        public string rule_params { get; set; }
        
        public List<string> Apply(object obj, TemplateData data)
        {
            Regex reg = new Regex(@"@{FOREACH\((\w+)\s+IN\s+(.+)\)}");
            Match m = reg.Match(rule_params);
            if (!m.Success)
            {
                Console.WriteLine("{0}格式错误 {1}", RuleInfo.rule_type, rule_params);
                return null;
            }

            string var_name = m.Groups[1].Value;
            string extra = m.Groups[2].Value;
            Array var_array = data.ExtraMetaData(obj, extra) as Array;

            List<string> result = new List<string>();
            foreach (object v in var_array)
            {
                if(!data.localVariantDict.ContainsKey(var_name))
                {
                    data.localVariantDict.Add(var_name, v);
                }
                else
                {
                    data.localVariantDict[var_name] = v;
                }

                List<string> res = new List<string>();
                List<ITemplateRule> extend_rules = TemplateRuleParser.Parse(rule_lines);
                foreach (var rule in extend_rules)
                {
                    var ss = rule.Apply(obj, data);
                    res.AddRange(ss);
                }
                result.AddRange(res);
            }


            return result;
        }

    }

}
