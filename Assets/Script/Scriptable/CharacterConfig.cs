using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "AnimationConfig", menuName = "ScriptableObjects/角色动画配置", order = 1)]
#endif
public class CharacterConfig : ScriptableObject {
    public string Tag;          // tag
    public GameObject Prefab;   // 创建的prefab
    [Space(), Header("动画名称")]
    public string Idle;         // 待机
    public string Walk;         // 行走
    public string TakeDamage;   // 受到攻击
    public string NormalAttack; // 普通攻击
    public string Ability_1;    // 1号技能
    public string Ability_2;    // 2号技能
    public string Ability_3;    // 3号技能
    [Space(), Header("动画效果")]
    public GameObject NormalAttackPrefab;        // 普通攻击预制体
    public bool NormalAttackIsProject;           // 普通攻击时候是抛掷体
    public float NormalAttackYOffset;            // Y高度
    public float NormatlAttackSpped;             // 普通攻击速度
    public float NormatlAttackDelay;             // 动画延迟
    public GameObject NormatlAttackHitEffect;    // 普通攻击 碰撞效果
    public float NormatlAttackHitEffectDuration; // 动画碰撞效果时间
    
    [Space(), Header("音效")]
    public AudioClip AudioTakeDamage; // 收到伤害
    public AudioClip AudioNormalAttack;// 执行普通攻击
    public float AudioNormalAttackDelay;// 攻击音效延迟
    public AudioClip AudioNormalAttackHit;// 普通攻击击中
    public float AudioNormalAttackHitDelay;// 普通攻击击中延迟

    [Space(), Header("购买")]
    public int Price; // 购买价格

    [Space(), Header("AI")]
    public GameObject AIPrefab; // 购买价格
    public float NormalAttackPeriod;             //  攻击周期

}
