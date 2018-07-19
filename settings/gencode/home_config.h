//this file is generated by xml2code tool at 7/19/2018 17:04:32
//do NOT edit it !

#pragma once
#include <map>
#include <vector>
#include <string>
#include "../shared/xml_config_interface.h"
#include "common.h"

//enum

enum HomeType
{
    Home001 = 1, 
    Home222 = 2, 
    Home333 = 3, 
};

//class

struct SceneLevel : public IXmlParser
{
    int level; 
    int size; 
    std::vector<CostItem> upgrade_cost; 
    std::vector<ObjectLayout> add_furniture; 
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};

struct Scene : public IXmlParser
{
    int id; //作为dict的key
    HomeType type; //枚举
    std::string name; 
    int open_level; 
    int def_ground; 
    int def_terrain; 
    int def_bg; 
    std::vector<ObjectLayout> def_layout; 
    std::vector<CostItem> open_cost; 
    std::vector<SceneLevel> SceneLevelList; //默认从下级节点解析
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};

struct HomeConfig : public IXmlParser
{
    std::map<int,Scene> SceneList; 
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};

//hotfix
class home_config_hotfix_xml:public IHotfixXml
{
public:
    virtual bool Load(variant* v);
    virtual void* Root(){return root;}
private:
    HomeConfig* root;
};
