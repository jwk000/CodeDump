//this file is generated by xml2code tool 7/19/2018 17:04:32
//DO NOT EDIT IT !

using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GYGeneric;

namespace GYXMLData
{

    
    public class Dress :IXmlParser
    {
        public int id = 0; 
        public string name ; 
        public int gender = 0; 
        public int type = 0; 
        public int sub_type = 0; 
        public bool can_overlay = false; 
        public bool show_inbag = false; 
        public List<bodyPart> bodyPartList = new List<bodyPart>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("bodyPartList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("bodyPart");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    bodyPart value = new bodyPart();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    bodyPartList.Add(value);
                }
            }
            if(node.Attributes!=null && node.Attributes["name"]!=null)
            {
                name = node.Attributes["name"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("name");
                if(child!=null)
                {
                    name = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["id"]!=null)
            {
                id = int.Parse(node.Attributes["id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("id");
                if(child!=null)
                {
                    id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["gender"]!=null)
            {
                gender = int.Parse(node.Attributes["gender"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("gender");
                if(child!=null)
                {
                    gender = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["type"]!=null)
            {
                type = int.Parse(node.Attributes["type"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("type");
                if(child!=null)
                {
                    type = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["sub_type"]!=null)
            {
                sub_type = int.Parse(node.Attributes["sub_type"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("sub_type");
                if(child!=null)
                {
                    sub_type = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["can_overlay"]!=null)
            {
                can_overlay = bool.Parse(node.Attributes["can_overlay"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("can_overlay");
                if(child!=null)
                {
                    can_overlay = bool.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["show_inbag"]!=null)
            {
                show_inbag = bool.Parse(node.Attributes["show_inbag"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("show_inbag");
                if(child!=null)
                {
                    show_inbag = bool.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class DressListInfo :IXmlParser
    {
        public Dictionary<int,Dress> DressList = new Dictionary<int,Dress>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("DressList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("Dress");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        Dress value = new Dress();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        DressList.Add(value.id, value);
                    }
                }
            }

            return true;
        }
    }


    public class Testdress : ITestCode
    {
        public void RunTestCode(string basepath)
        {
            string xml = basepath+"test/testxml/dress.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode root = doc.LastChild;
            DressListInfo data = new DressListInfo();
            if(!data.Parse(root))
            {
                Console.WriteLine("解析{0}失败", xml);
                return;
            }

            //写入bytes文件
            byte[] b = GenericBuilder.GenericsEncode(data);
            FileStream fs = File.Create("dress.bytes");
            fs.Write(b, 0, b.Length);
            fs.Close();

            //dump内容
            DressListInfo d = GenericParser.GenericsDecode<DressListInfo>(b);
            string s = GenericDump.Dump(d);
            fs = File.Create("dress.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
        }
    }
}
