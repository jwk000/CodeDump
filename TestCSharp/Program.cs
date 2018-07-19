using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using GYGeneric;
using GYXMLData;
using System.Reflection;

namespace TestCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach(Type t in assembly.GetTypes())
            {
                if(typeof(ITestCode).IsAssignableFrom(t) && typeof(ITestCode)!= t)
                {
                    ITestCode test = Activator.CreateInstance(t) as ITestCode;
                    test.RunTestCode("../../../CodeDump/bin/Debug/");
                }
            }
        }
    }
}
