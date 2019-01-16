//this file is generated by codedump at ${DATETIME}
//do NOT edit it !

#pragma once
#include <map>
#include <vector>
#include <string>
#include <sstream>

#include "platform/platform_shared/bulk_reader.h"
@{BEGIN_USING}
#include "${USING_META_NAME}"
@{END_USING}

namespace GYXmlData
{
    @{BEGIN_ENUM}
    ${ENUM_COMMENT}
    enum ${ENUM_NAME}
    {
    @{BEGIN_ENUM_FIELD}
        ${ENUM_FIELD_NAME} = ${ENUM_FIELD_VALUE}, ${ENUM_FIELD_COMMENT}
    @{END_ENUM_FIELD}
    };
    @{END_ENUM}

    @{BEGIN_CLASS}   
    ${CLASS_COMMENT}
    @{BEGIN_CLASS_IF(EQ,${CLASS_ATTR_NAME},root)}
    class ${CLASS_NAME} :  public IXmlDispatcher
    {
    private:
    	BulkReader m_reader;
    @{BEGIN_CLASS_ELSE}
    class ${CLASS_NAME}
    {
    @{END_CLASS_IF}
    public:
        ${CLASS_NAME}();
    @{BEGIN_CLASS_FIELD}
        ${CLASS_FIELD_TYPE} ${CLASS_FIELD_NAME}; ${CLASS_FIELD_COMMENT}
    @{END_CLASS_FIELD}
    public:
    @{BEGIN_CLASS_IF(EQ,${CLASS_ATTR_NAME},root)}
        void Init(std::string xmlfile);
        virtual bool HandleXml(const char *key, variant *var, bool init) override;
    @{END_CLASS_IF}
    @{BEGIN_CLASS_IF(EQ,${CLASS_ATTR_NAME},string)}
        bool LoadFromString(std::string s);
    @{BEGIN_CLASS_ELSE}
        bool LoadFromVariant(variant* v);
    @{END_CLASS_IF}
    };
    @{END_CLASS}

}