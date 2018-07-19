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

    
    public class XmlMusicInfoMode :IXmlParser
    {
        public int value = 0; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["value"]!=null)
            {
                value = int.Parse(node.Attributes["value"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("value");
                if(child!=null)
                {
                    value = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class XmlMusic :IXmlParser
    {
        public int id = 0; 
        public string name ; 
        public string singer_name ; 
        public int bpm = 0; 
        public int duration = 0; 
        public int ogg_id = 0; 
        public int star = 0; 
        public List<XmlMusicInfoMode> XmlMusicInfoModeList = new List<XmlMusicInfoMode>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("XmlMusicInfoModeList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("XmlMusicInfoMode");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    XmlMusicInfoMode value = new XmlMusicInfoMode();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    XmlMusicInfoModeList.Add(value);
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
            if(node.Attributes!=null && node.Attributes["singer_name"]!=null)
            {
                singer_name = node.Attributes["singer_name"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("singer_name");
                if(child!=null)
                {
                    singer_name = child.InnerText;
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
            if(node.Attributes!=null && node.Attributes["bpm"]!=null)
            {
                bpm = int.Parse(node.Attributes["bpm"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("bpm");
                if(child!=null)
                {
                    bpm = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["duration"]!=null)
            {
                duration = int.Parse(node.Attributes["duration"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("duration");
                if(child!=null)
                {
                    duration = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["ogg_id"]!=null)
            {
                ogg_id = int.Parse(node.Attributes["ogg_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("ogg_id");
                if(child!=null)
                {
                    ogg_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["star"]!=null)
            {
                star = int.Parse(node.Attributes["star"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("star");
                if(child!=null)
                {
                    star = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class XmlMusicDifficulty :IXmlParser
    {
        public int diff = 0; 
        public int max_star = 0; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["diff"]!=null)
            {
                diff = int.Parse(node.Attributes["diff"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("diff");
                if(child!=null)
                {
                    diff = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["max_star"]!=null)
            {
                max_star = int.Parse(node.Attributes["max_star"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("max_star");
                if(child!=null)
                {
                    max_star = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class XmlMusicInfo :IXmlParser
    {
        public Dictionary<int,XmlMusic> XmlMusicList = new Dictionary<int,XmlMusic>(); 
        public List<XmlMusicDifficulty> XmlMusicDifficultyList = new List<XmlMusicDifficulty>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("XmlMusicList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("XmlMusic");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        XmlMusic value = new XmlMusic();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        XmlMusicList.Add(value.id, value);
                    }
                }
            }
            {
                var mid_node = node.SelectSingleNode("XmlMusicDifficultyList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("XmlMusicDifficulty");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    XmlMusicDifficulty value = new XmlMusicDifficulty();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    XmlMusicDifficultyList.Add(value);
                }
            }

            return true;
        }
    }


    public class Testmusic_info : ITestCode
    {
        public void RunTestCode(string basepath)
        {
            string xml = basepath+"test/testxml/music_info.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode root = doc.LastChild;
            XmlMusicInfo data = new XmlMusicInfo();
            if(!data.Parse(root))
            {
                Console.WriteLine("解析{0}失败", xml);
                return;
            }

            //写入bytes文件
            byte[] b = GenericBuilder.GenericsEncode(data);
            FileStream fs = File.Create("music_info.bytes");
            fs.Write(b, 0, b.Length);
            fs.Close();

            //dump内容
            XmlMusicInfo d = GenericParser.GenericsDecode<XmlMusicInfo>(b);
            string s = GenericDump.Dump(d);
            fs = File.Create("music_info.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
        }
    }
}
