//this file is generated by xml2code tool at 7/19/2018 17:04:32
//do NOT edit it !

#pragma once
#include <map>
#include <vector>
#include <string>
#include "../shared/xml_config_interface.h"
#include "common.h"

//enum

//class

struct Accouterment : public IXmlParser
{
    int id; 
    std::string name; 
    int gender; 
    int type; 
    int sub_type; 
    bool is_wing; 
    bool can_overlay; 
    std::vector<bodyPart> bodyPartList; 
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};

struct AccoutermentList : public IXmlParser
{
    std::map<int,Accouterment> data; 
    bool LoadFromString(std::string s);
    bool LoadFromVariant(variant* v);
};

//hotfix
class accouterment_hotfix_xml:public IHotfixXml
{
public:
    virtual bool Load(variant* v);
    virtual void* Root(){return root;}
private:
    AccoutermentList* root;
};