#pragma once

#include "${Meta.Name}_info.h"
#include "products/Project_Mobile/mobile/mobile/shared/h3d_mobile_event.h"

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
struct ${Class.Name}:public ${Class.Base}
{
    REGISTER_AND_SERIALIZABLE(${Class.Name});
@{FOREACH(Field IN ${Class.FieldList})}
    ${Field.Type} ${Field.Name}; ${Field.Comment}
@{END_FOREACH}
};

@{END_FOREACH}


@{FOREACH(Class IN ${Meta.ClassList})}
REFELCTION_SUPPORT(${Class.Name}, (METAID_BASE(${G.ProjectName}) + EVENT_ID_BASE + ${Meta.AutoIncID}));
@{END_FOREACH}

void Declare${Meta.Name}EventMetas();
