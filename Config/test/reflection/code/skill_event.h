#pragma once

#include "Skill_info.h"
#include "products/Project_Mobile/mobile/mobile/shared/h3d_mobile_event.h"



struct CEventRequestSkillInfo:public CEventMobileRequest
{
    REGISTER_AND_SERIALIZABLE(CEventRequestSkillInfo);
};


struct CEventReplySkillInfo:public CEventMobileReply
{
    REGISTER_AND_SERIALIZABLE(CEventReplySkillInfo);
    std::map<int,SkillInfo> m_skill_infos; 
};



REFELCTION_SUPPORT(CEventRequestSkillInfo, (METAID_BASE(Project_Skill) + EVENT_ID_BASE + 1));
REFELCTION_SUPPORT(CEventReplySkillInfo, (METAID_BASE(Project_Skill) + EVENT_ID_BASE + 2));

void DeclareSkillEventMetas();
