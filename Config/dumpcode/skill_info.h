#pragma once

#include <platform/logic_frame/include/module_base/module_base_interface.h>
#include <platform/logic_frame/include/module_base/id_base.h>
#include <platform/logic_frame/include/module_base/serializable.h>
#include <platform/platform_shared/BiboFrame/BiboInterfaces.h>
#include <platform/platform_shared/object_system/refcls.h>




struct skill_data_info:public ISerializable
{
    SERIALIZABLE_DECLARE(skill_data_info);
    int dbid; 
    TPersistID ownerid; 
    int skillid; 
    std::string name; 
    std::string desc; 
    int level; 
    time_t create_time; 
};



REFELCTION_SUPPORT(skill_data_info, (METAID_BASE(Project_Skill) + 1));

void DeclareSkillInfoMetas();
