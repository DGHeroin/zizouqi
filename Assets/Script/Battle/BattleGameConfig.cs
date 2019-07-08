﻿using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "BattleGameConfig", menuName = "ScriptableObjects/战场配置", order = 1)]
#endif
public class BattleGameConfig : ScriptableObject {
    public int[] Players;
    public int round       = 1;
    public int roundTime   = 60; // 60s
    public int prepareTime = 30; // 30s
    // runtime value
    public int currentPrepareTime = 0;
    public int currentFightingTime = 0;
}
