using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "AnimationConfig", menuName = "ScriptableObjects/角色动画配置", order = 1)]
#endif
public class CharacterAnimationConfig : ScriptableObject {
    public string Tag;          // tag
    public GameObject Prefab;   // 创建的prefab
    public string Idle;         // 待机
    public string Walk;         // 行走
    public string TakeDamage;   // 受到攻击
    public string NormalAttack; // 普通攻击
    public string Ability_1;    // 1号技能
    public string Ability_2;    // 2号技能
    public string Ability_3;    // 3号技能
}
