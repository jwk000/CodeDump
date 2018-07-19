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

    
    public class lockui :IXmlParser
    {
        public float locktime = 0.0f; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["locktime"]!=null)
            {
                locktime = float.Parse(node.Attributes["locktime"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("locktime");
                if(child!=null)
                {
                    locktime = float.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class maskinfo :IXmlParser
    {
        public int winShowType = 0; 
        public Pos maskPos = new Pos(); 
        public Size masksize = new Size(); 
        public string anchorType ; 
        public int rotz = 0; 
        public bool isMaskTransparent = false; 
        public Pos windowpos = new Pos(); 

        public bool Parse(XmlNode node)
        {
            {
                var child_node = node.SelectSingleNode("Pos");
                if(child_node != null)
                {
                    if(!maskPos.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("Size");
                if(child_node != null)
                {
                    if(!masksize.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("Pos");
                if(child_node != null)
                {
                    if(!windowpos.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            if(node.Attributes!=null && node.Attributes["anchorType"]!=null)
            {
                anchorType = node.Attributes["anchorType"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("anchorType");
                if(child!=null)
                {
                    anchorType = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["winShowType"]!=null)
            {
                winShowType = int.Parse(node.Attributes["winShowType"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("winShowType");
                if(child!=null)
                {
                    winShowType = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["rotz"]!=null)
            {
                rotz = int.Parse(node.Attributes["rotz"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("rotz");
                if(child!=null)
                {
                    rotz = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["isMaskTransparent"]!=null)
            {
                isMaskTransparent = bool.Parse(node.Attributes["isMaskTransparent"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("isMaskTransparent");
                if(child!=null)
                {
                    isMaskTransparent = bool.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class action :IXmlParser
    {
        public int type = 0; 
        public string param1 ; 
        public int npc = 0; 
        public float delay = 0.0f; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["param1"]!=null)
            {
                param1 = node.Attributes["param1"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("param1");
                if(child!=null)
                {
                    param1 = child.InnerText;
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
            if(node.Attributes!=null && node.Attributes["npc"]!=null)
            {
                npc = int.Parse(node.Attributes["npc"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("npc");
                if(child!=null)
                {
                    npc = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["delay"]!=null)
            {
                delay = float.Parse(node.Attributes["delay"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("delay");
                if(child!=null)
                {
                    delay = float.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class condition :IXmlParser
    {
        public int type = 0; 
        public string param1 ; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["param1"]!=null)
            {
                param1 = node.Attributes["param1"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("param1");
                if(child!=null)
                {
                    param1 = child.InnerText;
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

            return true;
        }
    }

    
    public class Step :IXmlParser
    {
        public int id = 0; 
        public int type = 0; 
        public int backto = 0; 
        public bool isLocalStep = false; 
        public string padView ; 
        public lockui lockuiv = new lockui(); 
        public maskinfo maskinfov = new maskinfo(); 
        public List<string> pop_tip_list = new List<string>(); 
        public List<action> comm_actions = new List<action>(); 
        public List<condition> finish_conditions = new List<condition>(); 
        public List<condition> trigger_conditions = new List<condition>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("comm_actions");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("action");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    action value = new action();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    comm_actions.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("finish_conditions");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("condition");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    condition value = new condition();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    finish_conditions.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("trigger_conditions");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("condition");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    condition value = new condition();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    trigger_conditions.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("pop_tip_list");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node =  mid_node.ChildNodes;
                if(child_node != null)
                {
                    foreach(XmlNode n in child_node)
                    {
                        pop_tip_list.Add(n.InnerText);
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("lockui");
                if(child_node != null)
                {
                    if(!lockuiv.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("maskinfo");
                if(child_node != null)
                {
                    if(!maskinfov.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            if(node.Attributes!=null && node.Attributes["padView"]!=null)
            {
                padView = node.Attributes["padView"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("padView");
                if(child!=null)
                {
                    padView = child.InnerText;
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
            if(node.Attributes!=null && node.Attributes["backto"]!=null)
            {
                backto = int.Parse(node.Attributes["backto"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("backto");
                if(child!=null)
                {
                    backto = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["isLocalStep"]!=null)
            {
                isLocalStep = bool.Parse(node.Attributes["isLocalStep"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("isLocalStep");
                if(child!=null)
                {
                    isLocalStep = bool.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class Group :IXmlParser
    {
        public int groupid = 0; 
        public int guidetype = 0; 
        public List<Step> steps = new List<Step>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("steps");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("Step");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    Step value = new Step();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    steps.Add(value);
                }
            }
            if(node.Attributes!=null && node.Attributes["groupid"]!=null)
            {
                groupid = int.Parse(node.Attributes["groupid"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("groupid");
                if(child!=null)
                {
                    groupid = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["guidetype"]!=null)
            {
                guidetype = int.Parse(node.Attributes["guidetype"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("guidetype");
                if(child!=null)
                {
                    guidetype = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class flower :IXmlParser
    {
        public int id = 0; 
        public int num = 0; 
        public int type = 0; 

        public bool Parse(XmlNode node)
        {
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
            if(node.Attributes!=null && node.Attributes["num"]!=null)
            {
                num = int.Parse(node.Attributes["num"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("num");
                if(child!=null)
                {
                    num = int.Parse(child.InnerText);
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

            return true;
        }
    }

    
    public class item :IXmlParser
    {
        public int type_id = 0; 
        public Pos pos = new Pos(); 
        public Pos xiaoyao_pos = new Pos(); 

        public bool Parse(XmlNode node)
        {
            {
                var child_node = node.SelectSingleNode("Pos");
                if(child_node != null)
                {
                    if(!pos.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("Pos");
                if(child_node != null)
                {
                    if(!xiaoyao_pos.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            if(node.Attributes!=null && node.Attributes["type_id"]!=null)
            {
                type_id = int.Parse(node.Attributes["type_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("type_id");
                if(child!=null)
                {
                    type_id = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class res :IXmlParser
    {
        public string picture ; 

        public bool Parse(XmlNode node)
        {
            if(node.Attributes!=null && node.Attributes["picture"]!=null)
            {
                picture = node.Attributes["picture"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("picture");
                if(child!=null)
                {
                    picture = child.InnerText;
                }
            }

            return true;
        }
    }

    
    public class other_guide_config :IXmlParser
    {
        public int garden_guide_seed_id = 0; 
        public int garden_per_seed_harvest_num = 0; 
        public int garden_synthetic_flower_per_need = 0; 
        public List<flower> garden_synthetic_flower_list = new List<flower>(); 
        public List<item> collection_position_config = new List<item>(); 
        public int forge_product_id_boy = 0; 
        public int forge_product_id_girl = 0; 
        public string happy_match_title_res ; 
        public List<res> happy_match_res = new List<res>(); 
        public string arena2v2_match_title_res ; 
        public List<res> arena2v2_match_res = new List<res>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("garden_synthetic_flower_list");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("flower");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    flower value = new flower();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    garden_synthetic_flower_list.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("collection_position_config");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("item");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    item value = new item();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    collection_position_config.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("happy_match_res");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("res");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    res value = new res();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    happy_match_res.Add(value);
                }
            }
            {
                var mid_node = node.SelectSingleNode("arena2v2_match_res");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("res");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    res value = new res();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    arena2v2_match_res.Add(value);
                }
            }
            if(node.Attributes!=null && node.Attributes["happy_match_title_res"]!=null)
            {
                happy_match_title_res = node.Attributes["happy_match_title_res"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("happy_match_title_res");
                if(child!=null)
                {
                    happy_match_title_res = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["arena2v2_match_title_res"]!=null)
            {
                arena2v2_match_title_res = node.Attributes["arena2v2_match_title_res"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("arena2v2_match_title_res");
                if(child!=null)
                {
                    arena2v2_match_title_res = child.InnerText;
                }
            }
            if(node.Attributes!=null && node.Attributes["garden_guide_seed_id"]!=null)
            {
                garden_guide_seed_id = int.Parse(node.Attributes["garden_guide_seed_id"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("garden_guide_seed_id");
                if(child!=null)
                {
                    garden_guide_seed_id = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["garden_per_seed_harvest_num"]!=null)
            {
                garden_per_seed_harvest_num = int.Parse(node.Attributes["garden_per_seed_harvest_num"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("garden_per_seed_harvest_num");
                if(child!=null)
                {
                    garden_per_seed_harvest_num = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["garden_synthetic_flower_per_need"]!=null)
            {
                garden_synthetic_flower_per_need = int.Parse(node.Attributes["garden_synthetic_flower_per_need"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("garden_synthetic_flower_per_need");
                if(child!=null)
                {
                    garden_synthetic_flower_per_need = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["forge_product_id_boy"]!=null)
            {
                forge_product_id_boy = int.Parse(node.Attributes["forge_product_id_boy"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("forge_product_id_boy");
                if(child!=null)
                {
                    forge_product_id_boy = int.Parse(child.InnerText);
                }
            }
            if(node.Attributes!=null && node.Attributes["forge_product_id_girl"]!=null)
            {
                forge_product_id_girl = int.Parse(node.Attributes["forge_product_id_girl"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("forge_product_id_girl");
                if(child!=null)
                {
                    forge_product_id_girl = int.Parse(child.InnerText);
                }
            }

            return true;
        }
    }

    
    public class Groups :IXmlParser
    {
        public Dictionary<int,Group> groups = new Dictionary<int,Group>(); 
        public other_guide_config other_cfg = new other_guide_config(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("groups");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("Group");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        Group value = new Group();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        groups.Add(value.groupid, value);
                    }
                }
            }
            {
                var child_node = node.SelectSingleNode("other_guide_config");
                if(child_node != null)
                {
                    if(!other_cfg.Parse(child_node))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }


    public class Testguide : ITestCode
    {
        public void RunTestCode(string basepath)
        {
            string xml = basepath+"test/testxml/guide.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode root = doc.LastChild;
            Groups data = new Groups();
            if(!data.Parse(root))
            {
                Console.WriteLine("解析{0}失败", xml);
                return;
            }

            //写入bytes文件
            byte[] b = GenericBuilder.GenericsEncode(data);
            FileStream fs = File.Create("guide.bytes");
            fs.Write(b, 0, b.Length);
            fs.Close();

            //dump内容
            Groups d = GenericParser.GenericsDecode<Groups>(b);
            string s = GenericDump.Dump(d);
            fs = File.Create("guide.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
        }
    }
}
