using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "BattleGameConfig", menuName = "ScriptableObjects/战场配置", order = 1)]
#endif
public class BattleGameConfig : ScriptableObject {
    [Tooltip("战斗最大时间")]
    public int roundTime = 60; // 60s
    [Tooltip("回合准备最大时间")]
    public int prepareTime = 30; // 30s
    [Tooltip("初始金币")]
    public int InitPrice = 1;// 初始金币
    [Tooltip("初始人口数")]
    public int InitPopulation;
    [Tooltip("初始血量")]
    public int InitHP;
    [Tooltip("最大手牌数量")]
    public int gamePlayMaxOnHandPlayer = 8;

    public AudioClip AudioRoundWin;
    public AudioClip AudioRoundFail;
    public AudioClip AudioRoundStart;
    public AudioClip AudioRoundCountDown;

    // runtime value
    [HideInInspector()]
    public int currentPrepareTime = 0;
    [HideInInspector()]
    public int currentFightingTime = 0;
    [HideInInspector()]
    public int currentRound = 1;
    [HideInInspector()]
    public Player[] Players; // 玩家
    [HideInInspector()]
    public Player Me;// 自己的视角
    
    public void Init() {
        currentPrepareTime = 0;
        currentFightingTime = 0;
        currentRound = 1;
    }
}

