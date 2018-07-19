using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDump
{
    class ExeJsonConfig
    {
        public ExeBaseDir base_dir;
        public ExeTemplate template;
        public ExeCommonIdlDir common;
        public ExeXmlDir[] xml;
    }

    class ExeBaseDir
    {
        public string xml_base_dir;
        public string cpp_base_dir;
        public string cs_base_dir;
    }

    class ExeTemplate
    {
        public string cpp_xml_head;
        public string cpp_xml_index;
        public string cpp_common_h;
        public string cpp_h;
        public string cpp_cpp;
        public string cs;
        public string cs_common;
    }
    class ExeCommonIdlDir
    {
        public string idl_dir;
        public string cpp_dir;
        public string cs_dir;

    }

    class ExeXmlDir
    {
        public string xml_dir;
        public string cpp_dir;
        public string cs_dir;

    }

}
