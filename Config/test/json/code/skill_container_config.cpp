//this file is generated by codedump at 3/22/2019 13:47:48 do NOT edit it !

#include "skill_container_config.h"


bool SkillContainerParser::Initialize(const char* file)
{
	Init(this);
	return Register(file, &SkillContainerParser::LoadConfig);
}

bool SkillContainerParser::LoadConfig(nlohmann::json& root, bool init)
{
    try
    {
        return ParseSkillContainer(root, m_config);
    }
    catch(nlohmann::detail::exception ex)
    {
     	LogError("SkillContainerParser::LoadConfig error: ", ex.what());
		lightAssert(false && "SkillContainerParser::LoadConfig error ");
		return false;   
    }
}

bool SkillContainerParser::ParseSkillContainer(nlohmann::json& root, SkillContainer& info)
{
    {
        auto& v_dict = root["mapDesignData"];
        for(auto it=v_dict.begin(); it!=v_dict.end(); ++it)
        {
            SkillData element;
            if(!ParseSkillData(*it, element))
            {
                return false;
            }
            info.mapDesignData.emplace(element.id, element);
        }
    }
    {
        auto& v_dict = root["mapEffect"];
        for(auto it=v_dict.begin(); it!=v_dict.end(); ++it)
        {
            EffectData element;
            if(!ParseEffectData(*it, element))
            {
                return false;
            }
            info.mapEffect.emplace(element.id, element);
        }
    }
    {
        auto& v_dict = root["mapBuff"];
        for(auto it=v_dict.begin(); it!=v_dict.end(); ++it)
        {
            BuffData element;
            if(!ParseBuffData(*it, element))
            {
                return false;
            }
            info.mapBuff.emplace(element.id, element);
        }
    }
    {
        auto& v_dict = root["mapProjectile"];
        for(auto it=v_dict.begin(); it!=v_dict.end(); ++it)
        {
            ProjectileData element;
            if(!ParseProjectileData(*it, element))
            {
                return false;
            }
            info.mapProjectile.emplace(element.id, element);
        }
    }
    {
        auto& v_dict = root["mapSupportSkill"];
        for(auto it=v_dict.begin(); it!=v_dict.end(); ++it)
        {
            SupportSkillData element;
            if(!ParseSupportSkillData(*it, element))
            {
                return false;
            }
            info.mapSupportSkill.emplace(element.id, element);
        }
    }

    return true;
}

bool SkillContainerParser::ParseSkillData(nlohmann::json& root, SkillData& info)
{
    info.id = root["id"].get<std::string>();
    info.name = root["name"].get<std::string>();
    info.desc = root["desc"].get<std::string>();
    info.descSpecial = root["descSpecial"].get<std::string>();
    info.icon = root["icon"].get<std::string>();
    info.memorySlot = root["memorySlot"].get<int>();
    info.actionPt = root["actionPt"].get<int>();
    info.cd = root["cd"].get<float>();
    info.resistedBy = root["resistedBy"].get<std::string>();
    info.range = root["range"].get<float>();
    info.category = root["category"].get<std::string>();
    {
        auto& v_list = root["effList"];

        for(auto it=v_list.begin(); it!=v_list.end(); ++it)
        {
            SkillEffectInfo element;
            if(!ParseSkillEffectInfo(*it, element))
            {
                return false;
            }
            info.effList.push_back(element);
            
        }
    }

    return true;
}

bool SkillContainerParser::ParseSkillEffectInfo(nlohmann::json& root, SkillEffectInfo& info)
{
    info.effId = root["effId"].get<std::string>();
    info.effType = root["effType"].get<std::string>();
    info.locateType = root["locateType"].get<std::string>();
    info.targetPosOffsetX = root["targetPosOffsetX"].get<float>();
    info.targetPosOffsetY = root["targetPosOffsetY"].get<float>();
    info.targetPosOffsetZ = root["targetPosOffsetZ"].get<float>();

    return true;
}

bool SkillContainerParser::ParseEffectData(nlohmann::json& root, EffectData& info)
{
    info.id = root["id"].get<std::string>();
    info.name = root["name"].get<std::string>();
    info.desc = root["desc"].get<std::string>();
    info.elementType = root["elementType"].get<std::string>();
    {
        auto& v_class = root["comTargeting"];
        if(!ParseComTargeting(v_class, info.comTargeting))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comEvaluate"];
        if(!ParseComEvaluate(v_class, info.comEvaluate))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comTrigger"];
        if(!ParseComTrigger(v_class, info.comTrigger))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comRender"];
        if(!ParseComRender(v_class, info.comRender))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comDisplace"];
        if(!ParseComDisplace(v_class, info.comDisplace))
        {
            return false;
        }
    }

    return true;
}

bool SkillContainerParser::ParseComTargeting(nlohmann::json& root, ComTargeting& info)
{
    info.refSubject = root["refSubject"].get<std::string>();
    info.shape = root["shape"].get<std::string>();
    info.target = root["target"].get<std::string>();
    info.mapParams = root["mapParams"].get<std::map<std::string,float>>();

    return true;
}

bool SkillContainerParser::ParseComEvaluate(nlohmann::json& root, ComEvaluate& info)
{
    info.minExpression = root["minExpression"].get<std::string>();
    info.maxExpression = root["maxExpression"].get<std::string>();

    return true;
}

bool SkillContainerParser::ParseComTrigger(nlohmann::json& root, ComTrigger& info)
{
    {
        auto& v_list = root["entries"];

        for(auto it=v_list.begin(); it!=v_list.end(); ++it)
        {
            ComTriggerEntry element;
            if(!ParseComTriggerEntry(*it, element))
            {
                return false;
            }
            info.entries.push_back(element);
            
        }
    }

    return true;
}

bool SkillContainerParser::ParseComTriggerEntry(nlohmann::json& root, ComTriggerEntry& info)
{
    info.timing = root["timing"].get<std::string>();
    info.type = root["type"].get<std::string>();
    info.triggerTargetId = root["triggerTargetId"].get<std::string>();
    info.targetAffected = root["targetAffected"].get<std::string>();
    info.triggerParams = root["triggerParams"].get<std::map<std::string,std::string>>();

    return true;
}

bool SkillContainerParser::ParseComRender(nlohmann::json& root, ComRender& info)
{
    info.renderEntityID = root["renderEntityID"].get<std::string>();

    return true;
}

bool SkillContainerParser::ParseComDisplace(nlohmann::json& root, ComDisplace& info)
{
    info.subject = root["subject"].get<std::string>();
    info.type = root["type"].get<std::string>();
    info.mapParams = root["mapParams"].get<std::map<std::string,float>>();

    return true;
}

bool SkillContainerParser::ParseBuffData(nlohmann::json& root, BuffData& info)
{
    info.id = root["id"].get<std::string>();
    info.name = root["name"].get<std::string>();
    info.desc = root["desc"].get<std::string>();
    info.status = root["status"].get<std::string>();
    info.removeStatus = root["removeStatus"].get<std::string>();
    {
        auto& v_list = root["comGroups"];

        for(auto it=v_list.begin(); it!=v_list.end(); ++it)
        {
            BuffComGroup element;
            if(!ParseBuffComGroup(*it, element))
            {
                return false;
            }
            info.comGroups.push_back(element);
            
        }
    }
    {
        auto& v_class = root["comTrigger"];
        if(!ParseComTrigger(v_class, info.comTrigger))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comRender"];
        if(!ParseComRender(v_class, info.comRender))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comRemove"];
        if(!ParseComRemove(v_class, info.comRemove))
        {
            return false;
        }
    }

    return true;
}

bool SkillContainerParser::ParseComRemove(nlohmann::json& root, ComRemove& info)
{
    info.refSubject = root["refSubject"].get<std::string>();
    info.removeTiming = root["removeTiming"].get<std::string>();
    info.mapParams = root["mapParams"].get<std::map<std::string,float>>();
    info.forceRemoveTime = root["forceRemoveTime"].get<float>();

    return true;
}

bool SkillContainerParser::ParseBuffComGroup(nlohmann::json& root, BuffComGroup& info)
{
    {
        auto& v_class = root["comEvaluate"];
        if(!ParseComEvaluate(v_class, info.comEvaluate))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comActivate"];
        if(!ParseComActivate(v_class, info.comActivate))
        {
            return false;
        }
    }

    return true;
}

bool SkillContainerParser::ParseComActivate(nlohmann::json& root, ComActivate& info)
{
    info.activateType = root["activateType"].get<std::string>();
    info.mapParams = root["mapParams"].get<std::map<std::string,float>>();

    return true;
}

bool SkillContainerParser::ParseComReplace(nlohmann::json& root, ComReplace& info)
{
    info.targetType = root["targetType"].get<std::string>();
    info.targetId = root["targetId"].get<std::string>();

    return true;
}

bool SkillContainerParser::ParseSupportSkillData(nlohmann::json& root, SupportSkillData& info)
{
    info.id = root["id"].get<std::string>();
    info.name = root["name"].get<std::string>();
    info.desc = root["desc"].get<std::string>();
    info.elementType = root["elementType"].get<std::string>();
    {
        auto& v_class = root["comEvaluate"];
        if(!ParseComEvaluate(v_class, info.comEvaluate))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comActivate"];
        if(!ParseComActivate(v_class, info.comActivate))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comReplace"];
        if(!ParseComReplace(v_class, info.comReplace))
        {
            return false;
        }
    }

    return true;
}

bool SkillContainerParser::ParseProjectileData(nlohmann::json& root, ProjectileData& info)
{
    info.id = root["id"].get<std::string>();
    info.name = root["name"].get<std::string>();
    info.desc = root["desc"].get<std::string>();
    info.elementType = root["elementType"].get<std::string>();
    {
        auto& v_class = root["comTargeting"];
        if(!ParseComTargeting(v_class, info.comTargeting))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comEvaluate"];
        if(!ParseComEvaluate(v_class, info.comEvaluate))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comTrigger"];
        if(!ParseComTrigger(v_class, info.comTrigger))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comRender"];
        if(!ParseComRender(v_class, info.comRender))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comDisplace"];
        if(!ParseComDisplace(v_class, info.comDisplace))
        {
            return false;
        }
    }
    {
        auto& v_class = root["comRemove"];
        if(!ParseComRemove(v_class, info.comRemove))
        {
            return false;
        }
    }

    return true;
}



