//this file is generated by codedump at ${DATETIME}
//do NOT edit it !

#include "${CODE_FILE_NAME}.h"
#include "platform/platform_shared/read_complex_str.h"
#include "platform/platform_shared/read_class_str.h"
#include <platform/platform_shared/uio/convert/convert_interface.h>

namespace GYXmlData
{
    @{BEGIN_CLASS}
    ${CLASS_NAME}::${CLASS_NAME}()
    {
        @{BEGIN_FIELD_TYPE_BASIC}
        ${CLASS_FIELD_NAME} ${CLASS_FIELD_DEFAULT_VALUE};
        @{END_FIELD_TYPE_BASIC}
    }

    @{BEGIN_CLASS_IF(EQ,${CLASS_ATTR_NAME},root)}
    void ${CLASS_NAME}::Init(std::string xmlfile)
    {
        m_reader.Init(NULL, this, NULL);
        m_reader.Register("${META_NAME}", xmlfile.c_str());
    }

    bool ${CLASS_NAME}::HandleXml(const char *key, variant *var, bool init)
    {
        if(std::string("${META_NAME}") == key)
        {
            return LoadFromVariant(var);
        }
        return false;
    }

    @{END_CLASS_IF}

    @{BEGIN_CLASS_IF(EQ,${CLASS_ATTR_NAME},string)}
    bool ${CLASS_NAME}::LoadFromString(std::string s)
    {
        char _delimiter;
        std::stringstream ss(s);
        ss
        @{BEGIN_CLASS_FIELD}
        >> ${CLASS_FIELD_NAME} >> _delimiter
        @{END_CLASS_FIELD}
        ;
        return true;
    }
    @{BEGIN_CLASS_ELSE}
    bool ${CLASS_NAME}::LoadFromVariant(variant* v)
    {
    @{BEGIN_FIELD_TYPE_DICT}
        {
            variant* v_dict = v->get_child("${CLASS_FIELD_TAG}");
            if(v_dict == NULL)
            {
                v_dict = v;
            }
            int n = v_dict->get_child_count();
            for(int i=0; i<n; i++)
            {
                if(!strcmp(v_dict->get_key(i), "${DICT_VALUE_TAG}"))
                {
                    ${DICT_VALUE_TYPE} element;
                    variant* v_element = v_dict->get_child(i);
                    if(!element.LoadFromVariant(v_element))
                    {
                        return false;
                    }
                    ${CLASS_FIELD_NAME}.emplace(element.${CLASS_FIELD_KEY}, element);
                }
            }
        }
    @{END_FIELD_TYPE_DICT}
    @{BEGIN_FIELD_TYPE_LIST}
        {
            variant* v_list = v->get_child("${CLASS_FIELD_TAG}");
            if(v_list == NULL)
            {
                v_list = v;
            }
            int n = v_list->get_child_count();
            for(int i=0; i<n; i++)
            {
                if(!strcmp(v_list->get_key(i), "${LIST_VALUE_TAG}"))
                {
                    ${LIST_VALUE_TYPE} element;
                    variant* v_element = v_list->get_child(i);
                    if(!element.LoadFromVariant(v_element))
                    {
                        return false;
                    }
                    ${CLASS_FIELD_NAME}.emplace_back(element);
                }
            }
        }
    @{END_FIELD_TYPE_LIST}
    @{BEGIN_FIELD_TYPE_LIST_BASIC}
        {
            variant* v_list = v->get_child("${CLASS_FIELD_TAG}");
            if(v_list == NULL)
            {
                v_list = v;
            }
            int n = v_list->get_child_count();
            for(int i=0; i<n; i++)
            {
                ${LIST_VALUE_TYPE} element;
                if(from_str_variant(v_list->get_child(i), element)<0)
                {
                    return false;
                }
                ${CLASS_FIELD_NAME}.emplace_back(element);
            }
        }
    @{END_FIELD_TYPE_LIST_BASIC}
    @{BEGIN_FIELD_TYPE_LIST_STRING}
        {
            std::string s;
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
            from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), s);
    @{BEGIN_CLASS_FIELD_ELSE}
            if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), s)<0) {return false;}
    @{END_CLASS_FIELD_IF}
            if(!s.empty())
            {
                ClassStringReader cr;
                ${CLASS_FIELD_NAME} = cr.GetObjectList<${LIST_VALUE_TYPE}>(s);
            }
        }
    @{END_FIELD_TYPE_LIST_STRING}
    @{BEGIN_FIELD_TYPE_CLASS}
        {
            variant* v_class = v->get_child("${CLASS_FIELD_TAG}");
            if(v_class && !${CLASS_FIELD_NAME}.LoadFromVariant(v_class))
            {
                return false;
            }
        }
    @{END_FIELD_TYPE_CLASS}
    @{BEGIN_FIELD_TYPE_ENUM}
        {
            int e=0;
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
            from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), e);
    @{BEGIN_CLASS_FIELD_ELSE}
            if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), e)<0) {return false;}
    @{END_CLASS_FIELD_IF}
            ${CLASS_FIELD_NAME}=(${CLASS_FIELD_TYPE})e;
        }
    @{END_FIELD_TYPE_ENUM}
    @{BEGIN_FIELD_TYPE_CLASS_STRING}
        {
            std::string s;
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
            from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), s);
    @{BEGIN_CLASS_FIELD_ELSE}
            if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), s)<0) {return false;}
    @{END_CLASS_FIELD_IF}
            if(!s.empty() && !${CLASS_FIELD_NAME}.LoadFromString(s))
            {
                return false;
            }
        }
    @{END_FIELD_TYPE_CLASS_STRING}
    @{BEGIN_FIELD_TYPE_STRING}
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
        from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME});
    @{BEGIN_CLASS_FIELD_ELSE}
        if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME})<0) {return false;}
    @{END_CLASS_FIELD_IF}
    @{END_FIELD_TYPE_STRING}
    @{BEGIN_FIELD_TYPE_INT}
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
        from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME});
    @{BEGIN_CLASS_FIELD_ELSE}
        if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME})<0) {return false;}
    @{END_CLASS_FIELD_IF}
    @{END_FIELD_TYPE_INT}
    @{BEGIN_FIELD_TYPE_FLOAT}
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
        from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME});
    @{BEGIN_CLASS_FIELD_ELSE}
        if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME})<0) {return false;}
    @{END_CLASS_FIELD_IF}
    @{END_FIELD_TYPE_FLOAT}
    @{BEGIN_FIELD_TYPE_BOOL}
    @{BEGIN_CLASS_FIELD_IF(EQ,${IS_CLASS_FIELD_OPTIONAL},true)}
        from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME});
    @{BEGIN_CLASS_FIELD_ELSE}
        if(from_str_variant(v->get_child("${CLASS_FIELD_TAG}"), ${CLASS_FIELD_NAME})<0) {return false;}
    @{END_CLASS_FIELD_IF}
    @{END_FIELD_TYPE_BOOL}

        return true;
    }
    @{END_CLASS_IF}

    @{END_CLASS}

}