using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine("usage1: CodeDump --xmldir=test/xml/ --tpldir=template/ --tardir=test/dumpcode/");
            Console.WriteLine("usage2: CodeDump --idl=xxx.idl --lang=cpp;cs");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string xmldir = "./xml/";
            string tpldir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template/");
            string tardir = "./dumpcode/";
            string idlfile = "";
            string[] lang = null;
            for (int i = 0; i < args.Length; i++)
            {
                var ss = args[i].Split('=');
                if (ss[0] == "--xmldir") xmldir = ss[1];
                if (ss[0] == "--tpldir") tpldir = ss[1];
                if (ss[0] == "--tardir") tardir = ss[1];
                if (ss[0] == "--idl") idlfile = ss[1];
                if (ss[0] == "--lang") lang = ss[1].Split(';');
            }

            CodeGenHelper code_gen = new CodeGenHelper();
            code_gen.Init();

            //生成一个文件的某种语言代码
            if (Path.GetExtension(idlfile)==".idl")
            {
                tardir = Path.Combine(Path.GetDirectoryName(idlfile), "codedump");
                foreach (string tpl in Directory.EnumerateFiles(tpldir))
                {
                    if (lang.Contains(Path.GetExtension(tpl)))
                    {
                        code_gen.GenerateAllCodes(idlfile, tpl, tardir);
                    }
                }
                Console.ReadKey();
                return;
            }

            //先特殊处理一下公共引用
            string commonidl = Path.Combine(xmldir, "common.idl");
            if (File.Exists(commonidl))
            {
                code_gen.ParseIDL(commonidl);
            }

            Action<string> DoGenerater = (string _xmldir) =>
            {
                //遍历目录下的idl文件
                foreach (string idl in Directory.EnumerateFiles(_xmldir))
                {
                    if (Path.GetExtension(idl) == ".idl")
                    {
                        //遍历template文件
                        foreach (string tpl in Directory.EnumerateFiles(tpldir))
                        {
                            code_gen.GenerateAllCodes(idl, tpl, tardir);
                        }
                    }
                }
            };

            DoGenerater(xmldir);
            //遍历子目录
            foreach (string dir in Directory.EnumerateDirectories(xmldir))
            {
                DoGenerater(dir);
            }

            code_gen.SaveCodeGenTime();
            sw.Stop();
            Console.WriteLine("生成代码完成！用时{0}秒！", sw.ElapsedMilliseconds / 1000.0f);
        }
    }
}
