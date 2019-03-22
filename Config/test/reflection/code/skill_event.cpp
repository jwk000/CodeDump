#include "Skill_event.h"


SERIALIZABLE_IMPLEMENT(CEventRequestSkillInfo);
SERIALIZABLE_IMPLEMENT(CEventReplySkillInfo);

void DeclareSkillEventMetas()
{
    DECLARE_REFLECTION(CEventRequestSkillInfo)
    .default_constructor();

    DECLARE_REFLECTION(CEventReplySkillInfo)
    .field(&CEventReplySkillInfo::m_skill_infos, "m_skill_infos", 1)
    .default_constructor();

   
}
