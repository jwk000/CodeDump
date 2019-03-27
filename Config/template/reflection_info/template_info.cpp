#include "${Meta.Name}_info.h"


@{FOREACH(Class IN ${Meta.ClassList})}
SERIALIZABLE_IMPLEMENT(${Class.Name});
@{END_FOREACH}


void Declare${Meta.Name}InfoMetas()
{
@{FOREACH(Class IN ${Meta.ClassList})}
    DECLARE_REFLECTION(${Class.Name})
    @{FOREACH(Field IN ${Class.FieldList})}
    .field(&${Class.Name}::${Field.Name}, "${Field.Name}", ${Field.Index} ${Field.PstFlags})
    @{END_FOREACH}
    .default_constructor();

@{END_FOREACH}
   
}
