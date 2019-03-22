#pragma once

#include <platform/logic_frame/include/module_base/module_base_interface.h>
#include <platform/logic_frame/include/module_base/id_base.h>
#include <platform/logic_frame/include/module_base/serializable.h>
#include <platform/platform_shared/BiboFrame/BiboInterfaces.h>


@{FOREACH(Enum IN ${Meta.EnumList})}
${Enum.Comment}
enum ${Enum.Name}
{
@{FOREACH(Field IN ${Enum.FieldList})}
    ${Field.Name} = ${Field.Value}, ${Field.Comment}
@{END_FOREACH}
};

@{END_FOREACH}

@{FOREACH(Class IN ${Meta.ClassList})}
${Class.Comment}
struct ${Class.Name}:public ISerializable
{
    SERIALIZABLE_DECLARE(${Class.Name});
@{FOREACH(Field IN ${Class.FieldList})}
    ${Field.Type} ${Field.Name}; ${Field.Comment}
@{END_FOREACH}
};

@{END_FOREACH}


@{FOREACH(Class IN ${Meta.ClassList})}
REFELCTION_SUPPORT(${Class.Name}, (METAID_BASE(${G.ProjectName}) + ${Meta.AutoIncID}));
@{END_FOREACH}

void Declare${Meta.Name}InfoMetas();
