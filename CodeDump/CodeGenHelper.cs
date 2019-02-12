using System;
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
        CS
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
                if (char.IsUpper(c[i]))
                {
                    tmp.Add('_');
                }
                tmp.Add(c[i]);
            }

            if (totype == NameManglingType.AaBb || totype == NameManglingType.aaBb)
            {
                bool toup = totype == NameManglingType.AaBb;
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (tmp[i] == '_')
                    {
                        toup = true;
                        continue;
                    }
                    if (toup)
                    {
                        tar.Add(char.ToUpper(c[i]));
                        toup = false;
                        continue;
                    }
                    tar.Add(c[i]);
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
            string meta_name = Path.GetFileNameWithoutExtension(idl);

            if (!all_xml_meta.TryGetValue(meta_name, out meta))
            {
                try
                {
                    meta = IDLParser.Parse(idl);
                }
                catch (Exception e)
                {
                    Console.WriteLine("解析IDL文件{0}失败,{1}", idl,e);
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
            foreach (var rule in rules)
            {
                var code_line = rule.Apply(data);
                code.AddRange(code_line);
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
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            CodeLanguage lang = GetLanguageByTemplateExt(Path.GetExtension(template));

            string idlfile = Path.GetFileName(idl);
            string rawname = idlfile.Substring(0, idlfile.IndexOf('.'));
            string cppname = NameMangling(rawname,  NameManglingType.aa_bb);
            string csname = NameMangling(rawname, NameManglingType.AaBb);
            string tplfile = Path.GetFileName(template).Replace("template", lang == CodeLanguage.CPP ? cppname : csname);
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
            meta.lang = lang;
            meta.code_file_name = tplfile.Substring(0,tplfile.IndexOf('.'));
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
