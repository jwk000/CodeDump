#pragma once

#include <platform/logic_frame/include/module_base/module_base_interface.h>
#include <platform/logic_frame/include/module_base/id_base.h>
#include <platform/logic_frame/include/module_base/serializable.h>
#include <platform/platform_shared/BiboFrame/BiboInterfaces.h>




struct SkillInfo:public ISerializable
{
    SERIALIZABLE_DECLARE(SkillInfo);
    int id; 
    std::string name; 
    std::string desc; 
};



REFELCTION_SUPPORT(SkillInfo, (METAID_BASE(Project_Skill) + 1));

void DeclareSkillInfoMetas();
