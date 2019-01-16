2019-1-16
---
增加了switch case规则，用于替代原来的class field类型选择规则

2019-1-14
---
规范了规则命令的命名，方便记忆。
删除了不常用的规则。如@{AFTER_META_TEXT}。
简化了ifelse规则，有IF/IF0/IF1/IF2四级可用。
增加@{BEGIN_FIELD_TYPE_BASIC}规则，表示bool int float string基本类型。


2018-8-1
---
新增tag属性，用来标记变量节点名称，用法如下：

[tag(Scene)]
class SceneData{//类名为SceneData而xml中标签名为Scene
    [tag(SceneLevelList)]
    List<SceneLevel> levels; //变量名为levels而标签名为SceneLevelList
}

[root]
class HomeConfig{
    [tag(SceneList)] //变量名为scenes而xml中标签名为SceneList
    Dict<int,SceneData> scenes;
}

