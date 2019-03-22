#include "${Meta.Name}_event.h"


@{FOREACH(Class IN ${Meta.ClassList})}
SERIALIZABLE_IMPLEMENT(${Class.Name});
@{END_FOREACH}

void Declare${Meta.Name}EventMetas()
{
@{FOREACH(Class IN ${Meta.ClassList})}
    DECLARE_REFLECTION(${Class.Name})
    @{FOREACH(Field IN ${Class.FieldList})}
    .field(&${Class.Name}::${Field.Name}, "${Field.Name}", ${Field.Index})
    @{END_FOREACH}
    .default_constructor();

@{END_FOREACH}
   
}
