using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GYXMLData
{
    interface IXmlParser
    {
        bool Parse(XmlNode node);
    }
}
