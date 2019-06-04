
@{FOREACH(Enum IN ${Meta.EnumList})}
--${Enum.Comment}
---@class ${Enum.Name}
local ${Enum.Name} =
{
@{FOREACH(Field IN ${Enum.FieldList})}
    ${Field.Name} = ${Field.Value}, --${Field.Comment}
@{END_FOREACH}
}
_enum("${Enum.Name}", ${Enum.Name})

@{END_FOREACH}

@{FOREACH(Class IN ${Meta.ClassList})}
--${Class.Comment}
@{IF(${Class.HasAttr(Base)}==true)}
---@class ${Class.Name}:${Class.Base}
_class("${Class.Name}",${Class.Base})
@{ELSE}
---@class ${Class.Name}
_class("${Class.Name}")
@{END_IF}

function ${Class.Name}:Constructor()
@{FOREACH(Field IN ${Class.FieldList})}
@{IF(${Field.MetaType}==class && ${Field.Type != TPersistID && ${Field.Type}!=time_t})}
    self.${Field.Name} = ${Field.Type}:New() --${Field.Comment}
@{ELSE}
    self.${Field.Name} = ${Field.DefaultValue} --${Field.Comment}
@{END_IF}
@{END_FOREACH}
end

---@private
${Class.Name}._proto=
{
@{FOREACH(Field IN ${Class.FieldList})}
    [${Field.Index}] = {"${Field.Name}", "${Field.Type}"},
@{END_FOREACH} 
}

@{END_FOREACH}
