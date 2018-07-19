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

    
    public class section :IXmlParser
    {
        public int id = 0; 
        public string name ; 
        public int chapterId = 0; 
        public int sceneId = 0; 
        public int default_env_id = 0; 
        public Pos position = new Pos(); 
        public int section_type = 0; 
        public int unlocklevel = 0; 
        public int difficulty = 0; 
        public int music_id = 0; 
        public int plot_id = 0; 
        public int pre_plot_id = 0; 
        public int latter_plot_id = 0; 
        public List<int> pre_sectionId = new List<int>(); 
        public string describe ; 
        public string force_main ; 
        public string invisible1 ; 
        public string invisible2 ; 
        public string buff ; 
        public string normal_rewards ; 
        public string first_rewards ; 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("pre_sectionId");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node =  mid_node.ChildNodes;
                if(child_node != null)
                {
                    foreach(XmlNode n in child_node)
                    {
                        pre_sectionId.Add(int.Parse(n.InnerText));
                    }
                }
            }
            if(node.Attributes!=null && node.Attributes["position"]!=null)
            {
                var s = node.Attributes["position"].Value;
                position = GenericStringParser.GetObject<Pos>(s);
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
            if(node.Attributes!=null && node.Attributes["describe"]!=null)
            {
                describe = node.Attributes["describe"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("describe");
                if(child!=null)
                {
                    describe = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["force_main"]!=null)
            {
                force_main = node.Attributes["force_main"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("force_main");
                if(child!=null)
                {
                    force_main = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["invisible1"]!=null)
            {
                invisible1 = node.Attributes["invisible1"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("invisible1");
                if(child!=null)
                {
                    invisible1 = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["invisible2"]!=null)
            {
                invisible2 = node.Attributes["invisible2"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("invisible2");
                if(child!=null)
                {
                    invisible2 = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["buff"]!=null)
            {
                buff = node.Attributes["buff"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("buff");
                if(child!=null)
                {
                    buff = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["normal_rewards"]!=null)
            {
                normal_rewards = node.Attributes["normal_rewards"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("normal_rewards");
                if(child!=null)
                {
                    normal_rewards = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["first_rewards"]!=null)
            {
                first_rewards = node.Attributes["first_rewards"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("first_rewards");
                if(child!=null)
                {
                    first_rewards = child.InnerText;
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
            if(node.Attributes!=null && node.Attributes["chapterId"]!=null)
            {
                chapterId = int.Parse(node.Attributes["chapterId"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("chapterId");
                if(child!=null)
                {
                    chapterId = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["sceneId"]!=null)
            {
                sceneId = int.Parse(node.Attributes["sceneId"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("sceneId");
                if(child!=null)
                {
                    sceneId = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["default_env_id"]!=null)
            {
                default_env_id = int.Parse(node.Attributes["default_env_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("default_env_id");
                if(child!=null)
                {
                    default_env_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["section_type"]!=null)
            {
                section_type = int.Parse(node.Attributes["section_type"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("section_type");
                if(child!=null)
                {
                    section_type = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["unlocklevel"]!=null)
            {
                unlocklevel = int.Parse(node.Attributes["unlocklevel"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("unlocklevel");
                if(child!=null)
                {
                    unlocklevel = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["difficulty"]!=null)
            {
                difficulty = int.Parse(node.Attributes["difficulty"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("difficulty");
                if(child!=null)
                {
                    difficulty = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["music_id"]!=null)
            {
                music_id = int.Parse(node.Attributes["music_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("music_id");
                if(child!=null)
                {
                    music_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["plot_id"]!=null)
            {
                plot_id = int.Parse(node.Attributes["plot_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("plot_id");
                if(child!=null)
                {
                    plot_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["pre_plot_id"]!=null)
            {
                pre_plot_id = int.Parse(node.Attributes["pre_plot_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("pre_plot_id");
                if(child!=null)
                {
                    pre_plot_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["latter_plot_id"]!=null)
            {
                latter_plot_id = int.Parse(node.Attributes["latter_plot_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("latter_plot_id");
                if(child!=null)
                {
                    latter_plot_id = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class SectionList :IXmlParser
    {
        public Dictionary<int,section> section_list = new Dictionary<int,section>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("section_list");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("section");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        section value = new section();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        section_list.Add(value.id, value);
                    }
                }
            }

            return true;
        }
    }


    public class Testsection : ITestCode
    {
        public void RunTestCode(string basepath)
        {
            string xml = basepath+"test/testxml/section.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode root = doc.LastChild;
            SectionList data = new SectionList();
            if(!data.Parse(root))
            {
                Console.WriteLine("解析{0}失败", xml);
                return;
            }

            //写入bytes文件
            byte[] b = GenericBuilder.GenericsEncode(data);
            FileStream fs = File.Create("section.bytes");
            fs.Write(b, 0, b.Length);
            fs.Close();

            //dump内容
            SectionList d = GenericParser.GenericsDecode<SectionList>(b);
            string s = GenericDump.Dump(d);
            fs = File.Create("section.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
        }
    }
}
