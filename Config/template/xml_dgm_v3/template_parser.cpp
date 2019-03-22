//this file is generated by codedump at ${G.DATETIME} do NOT edit it !

#include "${Meta.FileName}.h"
#include <platform/platform_shared/uio/convert/convert_interface.h>


bool ${Meta.Name}Parser::Init(const char* xmlfile)
{
    Attach(xmlfile);
    return ReLoadConfig();
}

bool ${Meta.Name}Parser::LoadConfig(variant* root)
{
    return Parse${Meta.RootClassName}(root, m_config);
}

@{FOREACH(Class IN ${Meta.ClassList})}
bool ${Meta.Name}Parser::Parse${Class.Name}(variant* root, ${Class.Name}& info)
{
@{FOREACH(Field IN ${Class.FieldList})}
@{SWITCH(${Field.MetaType})}
@{CASE(class)}
    {
        variant* v_class = root->get_child("${Field.Tag}");
        if(v_class &&
        @{IF(${Field.AttrName}==reward)}
        !G_Read_RewardInfo(v_class, info.${Field.Name}))
        @{ELSE}
        !Parse${Field.Type}(v_class, info.${Field.Name}))
        @{END_IF}
        {
            return false;
        }
    }
@{CASE(dict_class)}
    {
        variant* v_dict = root->get_child("${Field.Tag}");
        if(v_dict == NULL)
        {
            v_dict = root;
        }
        int n = v_dict->get_child_count();
        for(int i=0; i<n; i++)
        {
            if(!strcmp(v_dict->get_key(i), "${Field.DictValueTag}"))
            {
                ${Field.DictValueType} element;
                variant* v_element = v_dict->get_child(i);
                if(!Parse${Field.DictValueType}(v_element, element))
                {
                    return false;
                }
                info.${Field.Name}.insert(std::make_pair(element.${Field.DictKeyName}, element));
            }
        }
    }
@{CASE(list_class)}
    {
        variant* v_list = root->get_child("${Field.Tag}");
        if(v_list == NULL)
        {
            v_list = v;
        }
        int n = v_list->get_child_count();
        for(int i=0; i<n; i++)
        {
            if(!strcmp(v_list->get_key(i), "${Field.ListValueTag}"))
            {
                ${Field.ListValueType} element;
                variant* v_element = v_list->get_child(i);
                if(!Parse${Field.ListValueType}(v_element, element))
                {
                    return false;
                }
                info.${Field.Name}.push_back(element);
            }
        }
    }
@{CASE(list_basic)}
    {
        variant* v_list = root->get_child("${Field.Tag}");
        if(v_list == NULL)
        {
            v_list = v;
        }
        int n = v_list->get_child_count();
        for(int i=0; i<n; i++)
        {
            ${Field.ListValueType} element;
            if(from_str_variant(v_list->get_child(i), element)<0)
            {
                return false;
            }
            info.${Field.Name}.push_back(element);
        }
    }
@{CASE(bool||string||int||float)}
@{IF(${Field.IsOptional})}
    from_str_variant(root->get_child("${Field.Tag}"), info.${Field.Name});
@{ELSE}
    if(from_str_variant(root->get_child("${Field.Tag}"), info.${Field.Name})<0) {return false;}
@{END_IF}
@{END_SWITCH}
@{END_FOREACH}

    return true;
}

@{END_FOREACH}

