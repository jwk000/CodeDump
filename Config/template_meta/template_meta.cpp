#include "${Meta.Name}_info.h"
#include "${Meta.Name}_event.h"
#include "${Meta.Name}_meta.h"

BIBO_DLL_EXPORT int _Init${Meta.Name}Library()
{
    Declare${Meta.Name}InfoMetas();
    Declare${Meta.Name}EventMetas();
	return 0;
}
