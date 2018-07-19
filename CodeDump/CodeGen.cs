﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeDump
{
    enum CodeLanguage
    {
        INVALID,
        CPP,
        CS
    }

    interface ICodeGen
    {
        bool GenerateCode(IDLMeta xmlMeta, string h, string cpp);
    }
    class CodeGen
    {
        FileTimeChecker m_idlchecker = new FileTimeChecker();
        TemplateData m_cpp_template = new TemplateDataCpp();
        TemplateData m_cs_template = new TemplateDataCs();

        public TemplateData GetTemplateData(CodeLanguage m_lang)
        {
            if(m_lang == CodeLanguage.CPP)
            {
                return m_cpp_template;
            }
            return m_cs_template;
        }
        public void Init()
        {
            m_idlchecker.Init() ;
        }
        public bool CheckNeedGenCode(string idlpath, string cpppath)
        {
            if (m_idlchecker.IsModified(idlpath))
            {
                return true;
            }
            if (!File.Exists(cpppath))
            {
                return true;
            }
            return false;
        }

        public bool GenerateCode(CodeLanguage lang, IDLMeta meta, string template, string codefile)
        {
            string[] lines = File.ReadAllLines(template);

            TemplateRuleInfo ruleInfo = new TemplateRuleInfo();
            ruleInfo.rule_lines = lines.ToList();
            ruleInfo.rule_type = eTemplateRule.META_TEXT;
            ITemplateRule rule = TemplateRuleFactory.CreateRule(ruleInfo);

            List<string> code = rule.Apply(meta, GetTemplateData(lang));

            using (FileStream fs = new FileStream(codefile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var line in code)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            return true;
        }

        public bool GenerateCodeCppIndex(List<IDLMeta> all_metas, string template, string codefile)
        {
            string[] lines = File.ReadAllLines(template);

            TemplateRuleInfo ruleInfo = new TemplateRuleInfo();
            ruleInfo.rule_lines = lines.ToList();
            ruleInfo.rule_type = eTemplateRule.AFTER_META_TEXT;
            ITemplateRule rule = TemplateRuleFactory.CreateRule(ruleInfo);

            List<string> code = rule.Apply(all_metas, m_cpp_template);

            using (FileStream fs = new FileStream(codefile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var line in code)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            return true;

        }

    }
}