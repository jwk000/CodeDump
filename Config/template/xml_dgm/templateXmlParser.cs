//this file is generated by xml2code tool ${DATETIME}
//DO NOT EDIT IT !

using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BaseUtil;

namespace GYXMLData
{
@{BEGIN_ENUM}
    ${ENUM_COMMENT}
    public enum ${ENUM_NAME}
    {
        @{BEGIN_ENUM_FIELD}
        ${ENUM_FIELD_NAME} = ${ENUM_FIELD_VALUE}, ${ENUM_FIELD_COMMENT}
        @{END_ENUM_FIELD}
    }
@{END_ENUM}

@{BEGIN_CLASS}
    ${CLASS_COMMENT}
    public class ${CLASS_NAME} :IXMLData
    {
        @{BEGIN_CLASS_FIELD}
        public ${CLASS_FIELD_TYPE} ${CLASS_FIELD_NAME} ${CLASS_FIELD_DEFAULT_VALUE}; ${CLASS_FIELD_COMMENT}
        @{END_CLASS_FIELD}

        public override void WriteToBuf(NetOutStream outs)
        {
        @{BEGIN_FIELD_TYPE_DICT}
            outs.Write(${CLASS_FIELD_NAME}.Count);
            foreach(var kv in ${CLASS_FIELD_NAME})
            {
                outs.Write(kv.Key);
                kv.Value.WriteToBuf(outs);
            }
        @{END_FIELD_TYPE_DICT}
        @{BEGIN_FIELD_TYPE_LIST}
            outs.Write(${CLASS_FIELD_NAME}.Count);
            foreach(var value in ${CLASS_FIELD_NAME})
            {
                value.WriteToBuf(outs);
            }
        @{END_FIELD_TYPE_LIST}
        @{BEGIN_FIELD_TYPE_LIST_BASIC}
            outs.Write(${CLASS_FIELD_NAME}.Count);
            foreach(var value in ${CLASS_FIELD_NAME})
            {
                outs.Write(value);
            }
        @{END_FIELD_TYPE_LIST_BASIC}
        @{BEGIN_FIELD_TYPE_LIST_STRING}
            outs.Write(${CLASS_FIELD_NAME}.Count);
            foreach(var value in ${CLASS_FIELD_NAME})
            {
                @{BEGIN_CLASS_FIELD_IF(EQ,${IS_LIST_VALUE_CLASS},true)}
                value.WriteToBuf(outs);
                @{BEGIN_CLASS_FIELD_ELSE}
                outs.Write(value);
                @{END_CLASS_FIELD_IF}
            }
        @{END_FIELD_TYPE_LIST_STRING}
        @{BEGIN_FIELD_TYPE_CLASS}
            ${CLASS_FIELD_NAME}.WriteToBuf(outs);
        @{END_FIELD_TYPE_CLASS}
        @{BEGIN_FIELD_TYPE_CLASS_STRING}
            ${CLASS_FIELD_NAME}.WriteToBuf(outs);
        @{END_FIELD_TYPE_CLASS_STRING}
        @{BEGIN_FIELD_TYPE_STRING}
            outs.Write(${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_STRING}
        @{BEGIN_FIELD_TYPE_ENUM}
            outs.Write((int)${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_ENUM}
        @{BEGIN_FIELD_TYPE_INT}
            outs.Write(${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_INT}
        @{BEGIN_FIELD_TYPE_FLOAT}
            outs.Write(${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_FLOAT}
        @{BEGIN_FIELD_TYPE_BOOL}
            outs.Write(${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_BOOL}
        }

        public override void ReadFromBuf(NetInStream ins)
        {
        @{BEGIN_FIELD_TYPE_DICT}
            {
                int len=0;
                ins.Read(ref len);
                for(int i=0;i<len;i++)
                {
                    ${DICT_KEY_TYPE} key = default(${DICT_KEY_TYPE});
                    ins.Read(ref key);
                    ${DICT_VALUE_TYPE} value = new ${DICT_VALUE_TYPE}();
                    value.ReadFromBuf(ins);
                    ${CLASS_FIELD_NAME}.Add(key, value);
                }
            }
        @{END_FIELD_TYPE_DICT}
        @{BEGIN_FIELD_TYPE_LIST}
            {
                int len=0;
                ins.Read(ref len);
                for(int i=0;i<len;i++)
                {
                    ${LIST_VALUE_TYPE} value = new ${LIST_VALUE_TYPE}();
                    value.ReadFromBuf(ins);
                    ${CLASS_FIELD_NAME}.Add(value);
                }
            }
        @{END_FIELD_TYPE_LIST}
        @{BEGIN_FIELD_TYPE_LIST_BASIC}
            {
                int len=0;
                ins.Read(ref len);
                for(int i=0;i<len;i++)
                {
                    ${LIST_VALUE_TYPE} value = default(${LIST_VALUE_TYPE});
                    ins.Read(ref value);
                    ${CLASS_FIELD_NAME}.Add(value);
                }
            }
        @{END_FIELD_TYPE_LIST_BASIC}
        @{BEGIN_FIELD_TYPE_LIST_STRING}
            {
                int len=0;
                ins.Read(ref len);
                for(int i=0;i<len;i++)
                {
                    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_LIST_VALUE_CLASS},true)}
                    ${LIST_VALUE_TYPE} value = new ${LIST_VALUE_TYPE}();
                    value.ReadFromBuf(ins);
                    @{BEGIN_CLASS_FIELD_ELSE}
                    ${LIST_VALUE_TYPE} value = default(${LIST_VALUE_TYPE});
                    ins.Read(ref value)
                    @{END_CLASS_FIELD_IF}
                    ${CLASS_FIELD_NAME}.Add(value);
                }
            }
        @{END_FIELD_TYPE_LIST_STRING}
        @{BEGIN_FIELD_TYPE_CLASS}
            ${CLASS_FIELD_NAME}.ReadFromBuf(ins);
        @{END_FIELD_TYPE_CLASS}
        @{BEGIN_FIELD_TYPE_CLASS_STRING}
            ${CLASS_FIELD_NAME}.ReadFromBuf(ins);
        @{END_FIELD_TYPE_CLASS_STRING}
        @{BEGIN_FIELD_TYPE_STRING}
            ins.Read(ref ${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_STRING}
        @{BEGIN_FIELD_TYPE_ENUM}
            {
                int value=0;
                ins.Read(ref value);
                ${CLASS_FIELD_NAME} = (${CLASS_FIELD_TYPE})value;
            }
        @{END_FIELD_TYPE_ENUM}
        @{BEGIN_FIELD_TYPE_INT}
            ins.Read(ref ${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_INT}
        @{BEGIN_FIELD_TYPE_FLOAT}
            ins.Read(ref ${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_FLOAT}
        @{BEGIN_FIELD_TYPE_BOOL}
            ins.Read(ref ${CLASS_FIELD_NAME});
        @{END_FIELD_TYPE_BOOL}
        }

        public bool Parse(XmlNode node)
        {
            @{BEGIN_FIELD_TYPE_DICT}
            {
                var mid_node = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("${DICT_VALUE_TAG}");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        ${DICT_VALUE_TYPE} value = new ${DICT_VALUE_TYPE}();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        ${CLASS_FIELD_NAME}.Add(value.${CLASS_FIELD_KEY}, value);
                    }
                }
            }
            @{END_FIELD_TYPE_DICT}
            @{BEGIN_FIELD_TYPE_LIST}
            {
                var mid_node = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node = mid_node.SelectNodes("${LIST_VALUE_TAG}");
                if(child_node == null)
                {
                    child_node = mid_node.ChildNodes;
                }
                foreach(XmlNode n in child_node)
                {
                    ${LIST_VALUE_TYPE} value = new ${LIST_VALUE_TYPE}();
                    if(!value.Parse(n))
                    {
                        return false;
                    }
                    ${CLASS_FIELD_NAME}.Add(value);
                }
            }
            @{END_FIELD_TYPE_LIST}
            @{BEGIN_FIELD_TYPE_LIST_BASIC}
            {
                var mid_node = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var child_node =  mid_node.ChildNodes;
                if(child_node != null)
                {
                    foreach(XmlNode n in child_node)
                    {
                    @{BEGIN_CLASS_FIELD_IF(EQ,"${LIST_VALUE_TYPE}","string")}
                        ${CLASS_FIELD_NAME}.Add(n.InnerText);
                    @{BEGIN_CLASS_FIELD_ELSE}
                        ${CLASS_FIELD_NAME}.Add(${LIST_VALUE_TYPE}.Parse(n.InnerText));
                    @{END_CLASS_FIELD_IF}
                    }
                }
            }
            @{END_FIELD_TYPE_LIST_BASIC}
            @{BEGIN_FIELD_TYPE_CLASS}
            {
                var child_node = node.SelectSingleNode("${CLASS_FIELD_TYPE}");
                if(child_node != null)
                {
                    if(!${CLASS_FIELD_NAME}.Parse(child_node))
                    {
                        return false;
                    }
                }
            }
            @{END_FIELD_TYPE_CLASS}
            @{BEGIN_FIELD_TYPE_LIST_STRING}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                var s = node.Attributes["${CLASS_FIELD_TAG}"].Value;
                ${CLASS_FIELD_NAME} = GenericStringParser.GetObjectList<${LIST_VALUE_TYPE}>(s);
            }
            @{END_FIELD_TYPE_LIST_STRING}
            @{BEGIN_FIELD_TYPE_CLASS_STRING}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                var s = node.Attributes["${CLASS_FIELD_TAG}"].Value;
                ${CLASS_FIELD_NAME} = GenericStringParser.GetObject<${CLASS_FIELD_TYPE}>(s);
            }
            @{END_FIELD_TYPE_CLASS_STRING}
            @{BEGIN_FIELD_TYPE_STRING}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                ${CLASS_FIELD_NAME} = node.Attributes["${CLASS_FIELD_TAG}"].Value;
            }
            else
            {
                var child = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(child!=null)
                {
                    ${CLASS_FIELD_NAME} = child.InnerText;
                }
            }
            @{END_FIELD_TYPE_STRING}
            @{BEGIN_FIELD_TYPE_ENUM}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                ${CLASS_FIELD_NAME} = (${CLASS_FIELD_TYPE})int.Parse(node.Attributes["${CLASS_FIELD_TAG}"].Value);
            }
            @{END_FIELD_TYPE_ENUM}
            @{BEGIN_FIELD_TYPE_INT}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                ${CLASS_FIELD_NAME} = int.Parse(node.Attributes["${CLASS_FIELD_TAG}"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(child!=null)
                {
                    ${CLASS_FIELD_NAME} = int.Parse(child.InnerText);
                }
            }
            @{END_FIELD_TYPE_INT}
            @{BEGIN_FIELD_TYPE_FLOAT}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                ${CLASS_FIELD_NAME} = float.Parse(node.Attributes["${CLASS_FIELD_TAG}"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(child!=null)
                {
                    ${CLASS_FIELD_NAME} = float.Parse(child.InnerText);
                }
            }
            @{END_FIELD_TYPE_FLOAT}
            @{BEGIN_FIELD_TYPE_BOOL}
            if(node.Attributes!=null && node.Attributes["${CLASS_FIELD_TAG}"]!=null)
            {
                ${CLASS_FIELD_NAME} = bool.Parse(node.Attributes["${CLASS_FIELD_TAG}"].Value);
            }
            else
            {
                var child = node.SelectSingleNode("${CLASS_FIELD_TAG}");
                if(child!=null)
                {
                    ${CLASS_FIELD_NAME} = bool.Parse(child.InnerText);
                }
            }
            @{END_FIELD_TYPE_BOOL}

            return true;
        }

    }

@{END_CLASS}

@{BEGIN_META_IF(EQ,${META_HAS_ROOT},true)}
public class ${ROOT_CLASS_NAME}Parser:XMLParserBase
{
    public override object ParserXML(XmlNode node, byte[] bytes)
    {
        ${ROOT_CLASS_NAME} data = new ${ROOT_CLASS_NAME}();
        if(!data.Parse(node))
        {
            return null;
        }
        return data;
    }
}
@{BEGIN_META_ELSE}
@{END_META_IF}
}