﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace CodeDump
{
    class FileTimeChecker
    {
        XDocument m_fileTimeXml = new XDocument();
        bool m_fileExist = true;
        Dictionary<string, string> m_filetime = new Dictionary<string, string>();

        public void Init()
        {
            if (!File.Exists("filetime.xml"))
            {
                m_fileExist = false;
                return;
            }
            m_fileTimeXml = XDocument.Load("filetime.xml");
            foreach(XElement e in m_fileTimeXml.Root.Elements())
            {
                m_filetime.Add(e.Attribute("file").Value, e.Attribute("time").Value);
            }
        }

        public bool IsModified(string idl)
        {
            if (!m_fileExist)
            {
                return true;
            }

            var elements = m_fileTimeXml.Root.Elements().Where(e => e.Attribute("file").Value == idl);
            if (elements == null || elements.Count() == 0)
            {
                return true;
            }

            string t = elements.First().Attribute("time").Value;
            string w = File.GetLastWriteTime(idl).ToString();
            if (t != w)
            {
                return true;
            }

            return false;
        }

        public void SetFileTime(string idl)
        {
            if(m_filetime.ContainsKey(idl))
            {
                m_filetime[idl] = File.GetLastWriteTime(idl).ToString();
                return;
            }
            m_filetime.Add(idl, File.GetLastWriteTime(idl).ToString());
        }

        public void SaveFileTime()
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("root"));
            foreach (var kv in m_filetime)
            {
                XElement node = new XElement("item", new XAttribute("file", kv.Key), new XAttribute("time", kv.Value));
                doc.Root.Add(node);
            }
            doc.Save("filetime.xml");
        }
    }
}
