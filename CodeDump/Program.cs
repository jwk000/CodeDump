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
            Console.WriteLine("usage1: CodeDump --idldir=test/xml/ --tpldir=template/ --tardir=test/dumpcode/");
            Console.WriteLine("usage2: CodeDump --idl=xxx.idl --tpldir=template/ --tardir=test/dumpcode/");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string idldir = "./xml/";
            string tpldir = "./template/";
            string tardir = "./dumpcode/";
            string idlfile = "";

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(" {0}", args[i]);

                var ss = args[i].Split('=');
                if (ss[0] == "--idldir") idldir = ss[1];
                if (ss[0] == "--tpldir") tpldir = ss[1];
                if (ss[0] == "--tardir") tardir = ss[1];
                if (ss[0] == "--idl") idlfile = ss[1];
            }

            CodeGenHelper code_gen = new CodeGenHelper();
            code_gen.Init();

            //生成一个idl文件的代码
            if (Path.GetExtension(idlfile)==".idl")
            {
                foreach (string tpl in Directory.EnumerateFiles(tpldir))
                {
                    code_gen.GenerateAllCodes(idlfile, tpl, tardir);
                }
                Console.ReadKey();
                return;
            }

            //为一个目录下的所有idl文件生成代码
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

            DoGenerater(idldir);
            //遍历子目录
            foreach (string dir in Directory.EnumerateDirectories(idldir))
            {
                DoGenerater(dir);
            }

            code_gen.SaveCodeGenTime();
            sw.Stop();
            Console.WriteLine("生成代码完成！用时{0}秒！", sw.ElapsedMilliseconds / 1000.0f);
        }
    }
}
