#include "Skill_info.h"


SERIALIZABLE_IMPLEMENT(SkillInfo);


void DeclareSkillInfoMetas()
{
    DECLARE_REFLECTION(SkillInfo)
    .field(&SkillInfo::id, "id", 1)
    .field(&SkillInfo::name, "name", 2)
    .field(&SkillInfo::desc, "desc", 3)
    .default_constructor();

   
}
