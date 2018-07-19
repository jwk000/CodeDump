//this file is generated by xml2code tool at ${DATETIME}
//do NOT edit it !

#pragma once
#include <map>
#include <vector>
#include <string>
#include "../shared/xml_config_interface.h"
@{BEGIN_USING}
#include "${USING_META_NAME}"
@{END_USING}

//enum
@{BEGIN_ENUM}
${ENUM_COMMENT}
enum ${ENUM_NAME}
{
@{BEGIN_ENUM_FIELD}
    ${ENUM_FIELD_NAME} = ${ENUM_FIELD_VALUE}, ${ENUM_FIELD_COMMENT}
@{END_ENUM_FIELD}
};
@{END_ENUM}

//class
@{BEGIN_CLASS}
${CLASS_COMMENT}
struct ${CLASS_NAME} : public IXmlParser
{
@{BEGIN_CLASS_FIELD}
    ${CLASS_FIELD_TYPE} ${CLASS_FIELD_NAME}; ${CLASS_FIELD_COMMENT}
@{END_CLASS_FIELD}
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};
@{END_CLASS}