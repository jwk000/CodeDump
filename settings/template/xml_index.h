// this file is generated by xml2code tool ${DATETIME}
// DO NOT EDIT IT

#pragma once

//枚举
enum xmlEnum
{
@{BEGIN_META}
    xml_${META_NAME},
@{END_META}
}

//路径
static const char* xmlPath[]=
{
@{BEGIN_META}
    "${META_FILE_PATH}.xml",
@{END_META}
}

//注册
#define REGISTER_ALL_XML \
@{BEGIN_META}
Register(xml_${META_NAME}, new ${META_NAME}_hotfix_xml);\
@{END_META}
// new xml add here ...