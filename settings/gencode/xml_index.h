// this file is generated by xml2code tool ${DATETIME}
// DO NOT EDIT IT

#pragma once

//枚举
enum xmlEnum
{
    xml_accouterment,
    xml_cardrole,
    xml_check,
    xml_community_answer_library,
    xml_dress,
    xml_guide,
    xml_home_config,
    xml_music_info,
    xml_section,
    xml_simulation_table,
}

//路径
static const char* xmlPath[]=
{
    "test/testxml/accouterment.xml",
    "test/testxml/cardrole.xml",
    "test/testxml/check.xml",
    "test/testxml/community_answer_library.xml",
    "test/testxml/dress.xml",
    "test/testxml/guide.xml",
    "test/testxml/home_config.xml",
    "test/testxml/music_info.xml",
    "test/testxml/section.xml",
    "test/testxml/simulation_table.xml",
}

//注册
#define REGISTER_ALL_XML \
Register(xml_accouterment, new accouterment_hotfix_xml);\
Register(xml_cardrole, new cardrole_hotfix_xml);\
Register(xml_check, new check_hotfix_xml);\
Register(xml_community_answer_library, new community_answer_library_hotfix_xml);\
Register(xml_dress, new dress_hotfix_xml);\
Register(xml_guide, new guide_hotfix_xml);\
Register(xml_home_config, new home_config_hotfix_xml);\
Register(xml_music_info, new music_info_hotfix_xml);\
Register(xml_section, new section_hotfix_xml);\
Register(xml_simulation_table, new simulation_table_hotfix_xml);\
// new xml add here ...