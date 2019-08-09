using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFiledGame : MonoBehaviour {
    BattleField battleField;
    public BattleGameConfig gameConfig;
    public PlayerSelector selector;

    [Header("英雄列表")]
    public CharacterPurchaseConfig[] CharacterList;
    public CharacterAnimationConfig[] AnimationConfig;

    private void Awake() {
        // 创建游戏
        battleField = new BattleField();
        // 创建玩家
        gameConfig.Init();
        gameConfig.Players = CreatePlayers();
        gameConfig.Me = gameConfig.Players[0];
        // 玩家选择器
        selector = GetComponent<LocalGamePlayerSelector>();
        // 游戏配置
        battleField.Setup(gameConfig, selector, CharacterList, AnimationConfig);
    }

    void Update() {
        battleField.UpdateGame();
    }

    public BattleField Game() {
        return battleField;
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <returns></returns>
    private Player[] CreatePlayers() {
        Player[] players = new Player[4];
        for (int i = 0; i < 2; i++) {
            players[0] = DummyPlayer();
        }
        return players;
    }

    Player DummyPlayer() {
        var player = new Player();
        player.HP = gameConfig.InitHP;
        player.NickName = "玩家";
        player.Money = gameConfig.InitPrice; // 金币数量
        player.Population = gameConfig.InitPopulation; // 人口数
        return player;
    }
}

