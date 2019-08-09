using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家匹配器
/// </summary>
public interface PlayerSelector {
    /// <summary>
    /// 获取玩家的阵容配置
    /// </summary>
    LevelConfig GetPlayerLevelConfig();
}
