#include "${Meta.Name}_info.h"
#include "${Meta.Name}_event.h"
#include "${Meta.Name}_meta.h"

@{FOREACH(Class IN ${Meta.ClassList})}
REFELCTION_SUPPORT(${Class.Name});
@{END_FOREACH}