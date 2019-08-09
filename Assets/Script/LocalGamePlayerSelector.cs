using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 本地游戏的玩家选择器
/// </summary>
public class LocalGamePlayerSelector : MonoBehaviour, PlayerSelector {

    [SerializeField]
    private List<LevelConfig> levels = new List<LevelConfig>();

    /// <summary>
    /// 本地游戏就固定配置
    /// </summary>
    /// <returns></returns>
    public LevelConfig GetPlayerLevelConfig() {
        int r = BattleField.Current.GetRound();
        if (r >= 0 && r < levels.Count) {
            return levels[r - 1];
        }
        // 如果达到最后一关了, 则无限打最后一关
        return levels[levels.Count - 1];
    }
}
