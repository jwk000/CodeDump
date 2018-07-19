using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeDump
{
    class CodeGenHelper
    {
        CodeGen m_code_gen = new CodeGen();

        public static Dictionary<string, IDLMeta> all_common_meta = new Dictionary<string, IDLMeta>();
        public static Dictionary<string, IDLMeta> all_xml_meta = new Dictionary<string, IDLMeta>();

        public static IDLEnum FindMetaEnum(string meta_name, string enum_name)
        {
            if (all_common_meta.TryGetValue(meta_name, out IDLMeta meta))
            {
                if (meta.meta_enum.TryGetValue(enum_name, out IDLEnum e))
                {
                    return e;
                }
            }
            if (all_xml_meta.TryGetValue(meta_name, out meta))
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
            if(all_common_meta.TryGetValue(meta_name, out IDLMeta meta))
            {
                if(meta.meta_class.TryGetValue(class_name, out IDLClass cls))
                {
                    return cls;
                }
            }
            if (all_xml_meta.TryGetValue(meta_name, out meta))
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
            m_code_gen.Init();
        }

        public CodeLanguage GetLanguageByTemplateExt(string ext)
        {
            if (ext == ".cs") return CodeLanguage.CS;
            if (ext == ".cpp") return CodeLanguage.CPP;
            if (ext == ".h") return CodeLanguage.CPP;
            return CodeLanguage.INVALID;
        }
        public bool GenerateCode(string idl, string template, string dir, bool isxml)
        {
            if (!File.Exists(idl))
            {
                Console.WriteLine("IDL文件{0}不存在！", idl);
                return false;
            }

            IDLMeta meta = null;
            string meta_name = Path.GetFileNameWithoutExtension(idl);
            if(!all_common_meta.TryGetValue(meta_name, out meta))
            {
                if (!all_xml_meta.TryGetValue(meta_name, out meta))
                {
                    meta = new IDLMeta();
                    if (!meta.Parse(idl))
                    {
                        Console.WriteLine("解析IDL文件{0}失败", idl);
                        return false;
                    }

                    if (isxml)
                    {
                        all_xml_meta.Add(meta_name, meta);
                    }
                    else
                    {
                        all_common_meta.Add(meta_name, meta);
                    }

                }
            }

            string ext = Path.GetExtension(template);
            string codefile = Path.Combine(dir, Path.GetFileNameWithoutExtension(idl))+ Path.GetExtension(template);
            if (m_code_gen.CheckNeedGenCode(idl, codefile))
            {
                if (!m_code_gen.GenerateCode(GetLanguageByTemplateExt(ext), meta, template, codefile))
                {
                    Console.WriteLine("生成IDL文件{0}的代码失败！", idl);
                    return false;
                }
            }
            return true;
        }

        public bool GenerateCppIndexCode(string template, string cppdir)
        {
            string outfile = Path.Combine(cppdir, Path.GetFileName(template));
            return m_code_gen.GenerateCodeCppIndex( all_xml_meta.Values.ToList(), template, outfile);
        }
    }
}
