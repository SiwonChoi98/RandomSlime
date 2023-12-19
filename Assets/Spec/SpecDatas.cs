// <auto-generated>
// SpecDataGenerator에서 만들어진 파일입니다. 수정하지 마세요.
// Copyright (c) CookApps.
// 이진호(jhlee8@cookapps.com)
// </auto-generated>

using CookApps.SpecData.Generator;


[GeneratorSpecData]
public partial class GameConfig
{
    /// 아이디
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    /// 키
    public string key;
    /// 값
    public float value;
}

[GeneratorSpecData]
public partial class Skill
{
    /// 아이디
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    /// 스킬명
    public string name;
    /// 스킬 타입
    public global::SkillType skill_type;
}

[GeneratorSpecData]
public partial class SkillLevel
{
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    /// 스킬테이블의 아이디
    public int skill_id;
    public int level;
    public string projectile_name;
    public string folder_name;
    public string sprite;
    /// 스킬 레벨 설명
    public string desc;
    /// 다음 레벨
    public int next_level_id;
    public global::PrjType prj_type;
    /// 스킬 쿨타임
    public float skill_cooltime;
    /// 투사체 개수
    public int base_obj_count;
    /// 기본 대미지 비율
    public float base_damage_rate;
    public float prj_scale;
    public global::SkillValueType desc_valueType;
    public float value1;
    public global::SkillValueType scale_valueType;
    public float value2;
}

[GeneratorSpecData]
public partial class InGameExp
{
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    public int lv;
    public int exp_min;
    public int exp_max;
    /// 필요 경험치
    public int need;
}

[GeneratorSpecData]
public partial class Stage
{
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    public int stage;
    public string name;
}

[GeneratorSpecData]
public partial class Monster
{
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    public string name;
    public string prefab_name;
    public string sprite_name;
    public global::MonsterType monster_type;
    public global::MonsterAttackType atk_type;
    public global::MonsterMoveType move_type;
    public global::SpawnType spawn_type;
    public float move_speed;
    public int atk;
    public int hp;
    public float atk_range;
    public float atk_speed;
    public float prj_speed;
    public float multiple;
    public float cool_time;
    public float frequency;
    public int monster_count;
    public int reward_group_id;
}

[GeneratorSpecData]
public partial class RewardGroup
{
    [GeneratorId(nameof(id), typeof(int))]
    public int id;
    public global::RewardType reward_type1;
    public int reward_value1;
    public global::RewardType reward_type2;
    public int reward_value2;
    public global::RewardType reward_type3;
    public int reward_value3;
    public global::RewardType reward_type4;
    public int reward_value4;
}