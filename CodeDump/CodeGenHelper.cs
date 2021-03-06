﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeDump
{
    enum NameManglingType
    {
        unknown, aaBb, AaBb, aa_bb
    }
    enum CodeLanguage
    {
        INVALID,
        CPP,
        CS,
        LUA,
        JS
    }

    class CodeGenHelper
    {
        FileTimeChecker m_file_time_checker = new FileTimeChecker();

        public static Dictionary<string, IDLMeta> all_xml_meta = new Dictionary<string, IDLMeta>();

        public static IDLEnum FindMetaEnum(string meta_name, string enum_name)
        {
            if (all_xml_meta.TryGetValue(meta_name, out IDLMeta meta))
            {
                if (meta.meta_enum.TryGetValue(enum_name, out IDLEnum e))
                {
                    return e;
                }
            }

            return null;

        }
        public static IDLClass FindMetaClass(string meta_name, string class_name)
        {
            if (all_xml_meta.TryGetValue(meta_name, out IDLMeta meta))
            {
                if (meta.meta_class.TryGetValue(class_name, out IDLClass cls))
                {
                    return cls;
                }
            }

            return null;
        }
        public void Init()
        {
            m_file_time_checker.Init();
        }

        public CodeLanguage GetLanguageByTemplateExt(string ext)
        {
            if (ext == ".cs") return CodeLanguage.CS;
            if (ext == ".cpp") return CodeLanguage.CPP;
            if (ext == ".h") return CodeLanguage.CPP;
            if (ext == ".lua") return CodeLanguage.LUA;
            if (ext == ".js") return CodeLanguage.JS;
            return CodeLanguage.INVALID;
        }

        public static NameManglingType CheckNameManglingType(string name)
        {
            if (name.IndexOf('_') >= 0) return NameManglingType.aa_bb;
            if (name[0] >= 'a' && name[0] <= 'z') return NameManglingType.aaBb;
            if (name[0] >= 'A' && name[0] <= 'Z') return NameManglingType.AaBb;
            return NameManglingType.unknown;
        }
        public static string NameMangling(string name, NameManglingType totype)
        {
            NameManglingType t = CheckNameManglingType(name);
            char[] c = name.ToCharArray();
            List<char> tmp = new List<char>();
            List<char> tar = new List<char>();
            for (int i = 0; i < c.Length; i++)
            {
                if (i > 0 && char.IsUpper(c[i]))
                {
                    tmp.Add('_');
                }
                tmp.Add(c[i]);
            }

            if (totype == NameManglingType.AaBb || totype == NameManglingType.aaBb)
            {
                bool toup = totype == NameManglingType.AaBb;
                for (int i = 0, j = 0; i < tmp.Count; i++)
                {
                    if (tmp[i] == '_')
                    {
                        toup = true;
                        continue;
                    }
                    if (toup)
                    {
                        tar.Add(char.ToUpper(c[j]));
                        toup = false;
                    }
                    else
                    {
                        tar.Add(c[j]);
                    }
                    j++;
                }

                return new string(tar.ToArray());

            }
            if (totype == NameManglingType.aa_bb)
            {
                string s = new string(tmp.ToArray());
                return s.ToLower();
            }

            return name;
        }


        public IDLMeta ParseIDL(string idl)
        {
            IDLMeta meta = null;
            string meta_name = Path.GetFileNameWithoutExtension(idl).Split('.')[0];

            if (!all_xml_meta.TryGetValue(meta_name, out meta))
            {
                try
                {
                    meta = IDLParser.Parse(idl);
                }
                catch (Exception e)
                {
                    Console.WriteLine("解析IDL文件{0}失败,{1}", idl, e);
                    return null;
                }
                all_xml_meta.Add(meta_name, meta);
            }
            return meta;
        }

        public bool GenerateCode(IDLMeta meta, string template, string codefile)
        {
            string[] lines = File.ReadAllLines(template);
            //解析规则
            var rules = TemplateRuleParser.Parse(lines.ToList());
            //展开规则
            List<string> code = new List<string>();
            TemplateData data = new TemplateData();
            data.SetGlobalVariant("Meta", meta);
            //meta变量注入
            foreach (var kv in meta.meta_variant)
            {
                data.SetGlobalVariant(kv.Key, kv.Value);
            }
            foreach (var rule in rules)
            {
                var code_line = rule.Apply(data);
                code.AddRange(code_line);
            }
            //删除旧文件
            if (File.Exists(codefile))
            {
                File.SetAttributes(codefile, FileAttributes.Normal);
                File.Delete(codefile);
            }
            //写入文件
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

        public bool GenerateAllCodes(string idl, string template, string dir)
        {
            if (!File.Exists(idl))
            {
                Console.WriteLine("IDL文件{0}不存在！", idl);
                return false;
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }


            string idlfile = Path.GetFileName(idl);
            string rawname = idlfile.Substring(0, idlfile.IndexOf('.'));
            string aa_bb_name = NameMangling(rawname, NameManglingType.aa_bb);
            string AaBbName = NameMangling(rawname, NameManglingType.AaBb);

            string tplfile = Path.GetFileName(template).Replace("Template", AaBbName).Replace("template", aa_bb_name);
            string codefile = Path.Combine(dir, tplfile);
            if (File.Exists(codefile) &&
                !m_file_time_checker.IsModified(idl) &&
                !m_file_time_checker.IsModified(template))
            {
                return true;
            }

            IDLMeta meta = ParseIDL(idl);
            if (meta == null)
            {
                return false;
            }
            meta.lang = GetLanguageByTemplateExt(Path.GetExtension(template));
            meta.code_file_name = tplfile.Substring(0, tplfile.IndexOf('.'));
            if (!GenerateCode(meta, template, codefile))
            {
                Console.WriteLine("生成IDL文件{0}的代码失败！", idl);
                return false;
            }
            Console.WriteLine("生成代码文件：{0}成功", codefile);

            m_file_time_checker.SetFileTime(idl);
            m_file_time_checker.SetFileTime(template);
            return true;
        }

        public void SaveCodeGenTime()
        {
            m_file_time_checker.SaveFileTime();
        }
    }
}
