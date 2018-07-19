using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;

namespace CodeDump
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string setting_path = "settings/"+ Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)+".json";
            string jsontext = File.ReadAllText(setting_path);

            //解析json
            ExeJsonConfig exejson = JsonConvert.DeserializeObject<ExeJsonConfig>(jsontext);

            CodeGenHelper code_gen = new CodeGenHelper();
            code_gen.Init();

            //遍历common目录
            foreach(var idl in Directory.EnumerateFiles(exejson.common.idl_dir))
            {
                //遍历目录下的idl文件
                if (Path.GetExtension(idl)==".idl")
                {
                    code_gen.GenerateCode(idl, exejson.template.cpp_common_h, exejson.common.cpp_dir, false);
                    code_gen.GenerateCode(idl, exejson.template.cpp_cpp, exejson.common.cpp_dir, false);
                    code_gen.GenerateCode(idl, exejson.template.cs, exejson.common.cs_dir, false);
                }
            }

            //遍历xml目录
            foreach(var cfg in exejson.xml)
            {
                //遍历目录下的idl文件
                foreach(string idl in Directory.EnumerateFiles(cfg.xml_dir))
                {
                    if(Path.GetExtension(idl)==".idl")
                    {
                        code_gen.GenerateCode(idl, exejson.template.cpp_h, exejson.common.cpp_dir, true);
                        code_gen.GenerateCode(idl, exejson.template.cpp_cpp, exejson.common.cpp_dir, true);
                        code_gen.GenerateCode(idl, exejson.template.cs, exejson.common.cs_dir, true);

                    }
                }
            }

            //为c++生成index文件
            code_gen.GenerateCppIndexCode(exejson.template.cpp_xml_head, exejson.common.cpp_dir);
            code_gen.GenerateCppIndexCode(exejson.template.cpp_xml_index, exejson.common.cpp_dir);

            sw.Stop();
            Console.WriteLine("生成代码完成！用时{0}秒！", sw.ElapsedMilliseconds/1000.0f);
        }
    }
}
