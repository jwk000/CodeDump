#include "Skill_info.h"


SERIALIZABLE_IMPLEMENT(skill_data_info);


void DeclareSkillInfoMetas()
{
    DECLARE_REFLECTION(skill_data_info)
    .field(&skill_data_info::dbid, "dbid", 1 , PERSIST_FLAG_FIELD|PERSIST_FLAG_AUTOINC)
    .field(&skill_data_info::ownerid, "ownerid", 2 , PERSIST_FLAG_FIELD|PERSIST_FLAG_PRIMARY_KEY)
    .field(&skill_data_info::skillid, "skillid", 3 , PERSIST_FLAG_FIELD)
    .field(&skill_data_info::name, "name", 4 )
    .field(&skill_data_info::desc, "desc", 5 )
    .field(&skill_data_info::level, "level", 6 , PERSIST_FLAG_FIELD)
    .field(&skill_data_info::create_time, "create_time", 7 , PERSIST_FLAG_FIELD|PERSIST_FLAG_DATETIME)
    .default_constructor();

   
}
