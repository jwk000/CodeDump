# CodeDump 一种通用的代码生成方案


## 概述
一种通过数据结构描述文件（meta文件）和目标代码模板文件（template文件）生成目标代码的方案。基本思路是先解析meta文件得到数据结构信息，然后解析template文件得到目标代码的规则信息，对template文件展开，填充meta信息，就得到了目标代码。

## CodeDump工具使用规则
以xml解析生成C#代码为例介绍使用规则。这里使用home_config.xml。
![](https://github.com/jwk000/CodeDump/blob/master/doc/1.gif)

### 1.	填写xml对应的meta文件
![](https://github.com/jwk000/CodeDump/blob/master/doc/2.gif)

说明：
- Home_config.idl即为描述类型信息的meta文件。
- 采用类似C#语法的规则，Dictionary比较长，简写成了Dict，目前支持类（class）、枚举（enum）、基本数据类型、容器（只支持List和Dict）、特性（attribute）、注释、默认值、引用（using）。
- 特性是为了辅助生成代码使用的，比如[key]标记的字段是类的key字段，用于解析Dict的时候找到key的名字。
- [string]表示这个字段从xml节点的一个属性字符串解析，而不是从一个xml节点解析。
- [range]表示这个字段要检查值范围，暂时没实现，不要使用。
- [root]表示这个类是配置文件的根类型，在生成C++代码时需要知道根类型。
- Using引用的meta文件会在C++中解析为#include 头文件。

### 2.	填写目标代码template文件(部分）
 ![](https://github.com/jwk000/CodeDump/blob/master/doc/3.gif)
 
说明：
这是生成C#代码的模板文件，其基本规则如下：

- `@{\w+}` 为“规则标记”，通常成对出现，@{BEGIN_XXX} @{END_XXX}，用来指示中间的文本展开规则。规则的分类和数据结构有关，从meta级别到text级别处理方式不同。
- `${\w+}` 为“数据标记”，数据处理和规则对应，每个级别的规则都有相应级别的数据，比如${META_NAME}表示meta级别的名称，${CLASS_FIELD_NAME}为类字段级别名称。

目前支持的规则标记
```C#
    enum eTemplateRule
    {
        PLAIN_TEXT,//没有规则
        META_TEXT,//对所有line应用meta替换
        AFTER_META_TEXT,//不作meta替换直接解析
        META,//对所有meta应用规则
        USING,//对所有using应用规则
        ENUM,//对所有enum应用规则
        ENUM_FIELD,//对所有enum field应用规则
        CLASS,//对所有class应用规则
        CLASS_ATTR_STRING,//对带有string特性的class应用规则
        CLASS_FIELD,//对所有class field应用规则
        //switch case
        CLASS_FIELD_TYPE_DICT,//根据不同类型选用规则
        CLASS_FIELD_TYPE_LIST,//字段类型为List<class>时使用此规则
        CLASS_FIELD_TYPE_LIST_BASIC,//字段类型为List<基本类型>时使用此规则
        CLASS_FIELD_TYPE_CLASS,//字段类型为class时使用此规则
        CLASS_FIELD_TYPE_ENUM,
        CLASS_FIELD_TYPE_DICT_STRING,
        CLASS_FIELD_TYPE_LIST_STRING,
        CLASS_FIELD_TYPE_CLASS_STRING,
        CLASS_FIELD_TYPE_STRING,
        CLASS_FIELD_TYPE_INT,
        CLASS_FIELD_TYPE_FLOAT,
        CLASS_FIELD_TYPE_BOOL,
        //if else
        CLASS_IF,
        CLASS_FIELD_IF,
    }
```

目前支持的数据标记
```C#
    enum eTemplateData
    {
        DATETIME, //得到当前时间
        META_NAME, //meta名称
        META_FILE_PATH, //meta文件路径
        ROOT_CLASS_NAME, //根类型名称
        USING_META_NAME, //引用meta的名称
        USING_COMMENT, //引用的注释
        CLASS_NAME, //类型名称
        CLASS_ATTR_NAME,//类特性名称
        CLASS_COMMENT, //类的注释
        CLASS_KEY_NAME, //类的key名称
        CLASS_FIELD_NAME,//类字段名称
        CLASS_FIELD_TYPE, //类字段类型
        LIST_VALUE_TYPE, //list类型的value类型
        DICT_VALUE_TYPE, //dict类型的value类型
        CLASS_FIELD_COMMENT, //类字段注释
        CLASS_FIELD_KEY, //类字段类的key名称
        CLASS_FIELD_DEFAULT_VALUE, //类字段默认值
        ENUM_NAME, //枚举名称
        ENUM_COMMENT,//枚举注释
        ENUM_FIELD_NAME, //枚举字段名称
        ENUM_FIELD_VALUE, //枚举字段值
        ENUM_FIELD_COMMENT, //枚举字段注释

        MaxCount
    }

```

### 3.	使用CodeDump工具生成代码
CodeDump工具使用同名json文件作为配置文件，解析并遍历配置的idl文件目录，使用模板文件生成代码到指定目录。CodeDump.json配置如下：
```json
{
    "base_dir":{
        "xml_base_dir":"test/",
        "cpp_base_dir":"test/gencode/",
        "cs_base_dir":"test/gencode/"
    },
    "template":
    {
        "cpp_xml_head":"test/template/xml_head.h",
        "cpp_xml_index":"test/template/xml_index.h",
        "cpp_common_h":"test/template/cpp_common.h",
        "cpp_h":"test/template/cpp_template.h",
        "cpp_cpp":"test/template/cpp_template.cpp",
        "cs":"test/template/cs_data_template.cs",
        "cs_common":"test/template/cs_common.cs"
    },
    "common":
    {
        "idl_dir":"test/testxml/commonidl/",
        "cpp_dir":"test/gencode/",
        "cs_dir":"test/gencode/"
    },
    "xml":
    [
        {
            "xml_dir": "test/testxml/",
            "cpp_dir": "test/gencode/",
            "cs_dir": "test/gencode/"
        }
    ]
}
```

本例中生成的C#代码为home_config.cs内容如下（部分）
```C#

    public class HomeConfig :IXmlParser
    {
        public Dictionary<int,Scene> SceneList = new Dictionary<int,Scene>(); 

        public bool Parse(XmlNode node)
        {
            {
                var mid_node = node.SelectSingleNode("SceneList");
                if(mid_node==null)
                {
                    mid_node = node;
                }
                var nodes = mid_node.SelectNodes("Scene");
                if(nodes!=null)
                {
                    foreach(XmlNode n in nodes)
                    {
                        Scene value = new Scene();
                        if(!value.Parse(n))
                        {
                            return false;
                        }
                        SceneList.Add(value.id, value);
                    }
                }
            }

            return true;
        }
    }
```

## IDL填写规范
1.	大括号不能换行。
2.	目前支持类（class）、枚举（enum）、基本数据类型（int,bool,string,float）、容器（只支持List和Dict）、特性（attribute）、注释、默认值、引用（using）。
3.	需要指定xml的root节点对应的类为[root]。
4.	需要指定dict<k,v>的v类中作为key的字段为[key]。
5.	不存在Dict<int,int>这种配置。存在List<int>配置。
6.	[string]表示这个字段（class/list）从”1,2,3;4,5,6”这种字符串解析。不指定的话默认从下级节点解析。字符串分隔符是comma(,)和simicolon(;)
7.	[string]还可以修饰类，默认类从xml节点解析，添加此特性则允许其从string解析。需要配合以[string]修饰的字段使用。
8.	类型有依赖关系，所以被依赖的class要写在前面，如果引用了其他idl文件里的类型需要在文件开始的地方引用一下比如：using common_meta。

## Xml填写规范
1.	尽量不要混搭风格！节点尽量写完整！
2.	每个具名节点，在IDL中填写为class、List、Dict。Class的成员对应xml中的属性。举例：

![](https://github.com/jwk000/CodeDump/blob/master/doc/6.gif)
![](https://github.com/jwk000/CodeDump/blob/master/doc/7.gif)

3.	Class、dict、list成员如果为普通类型，也可以用inner_text的配置方式。
例子一：

![](https://github.com/jwk000/CodeDump/blob/master/doc/8.gif)
![](https://github.com/jwk000/CodeDump/blob/master/doc/9.gif)

例子二：

![](https://github.com/jwk000/CodeDump/blob/master/doc/10.gif)
![](https://github.com/jwk000/CodeDump/blob/master/doc/11.gif)

4.	可以省略dict和list的中间节点。
标准写法：

![](https://github.com/jwk000/CodeDump/blob/master/doc/12.gif)

省略写法：

![](https://github.com/jwk000/CodeDump/blob/master/doc/13.gif)

对应idl类：

![](https://github.com/jwk000/CodeDump/blob/master/doc/14.gif)

5.	不支持的配置方式：
例子一：既有attribute又有inner text，说明tip节点是个类，但丢失了一种字段描述。

![](https://github.com/jwk000/CodeDump/blob/master/doc/15.gif)

例子二：不确定的配置格式，根据需求扩展节点。这种情况只能拆表，根据道具类型拆分。否则就要在idl中定义所有类型，结果会非常冗余。

![](https://github.com/jwk000/CodeDump/blob/master/doc/16.gif)
![](https://github.com/jwk000/CodeDump/blob/master/doc/17.gif)

例子三：不支持复杂嵌套类从string解析。混搭格式也不支持。

![](https://github.com/jwk000/CodeDump/blob/master/doc/18.gif)

