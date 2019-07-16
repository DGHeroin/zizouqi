using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFiledGame : MonoBehaviour {
    BattleField battleField;
    public BattleGameConfig gameConfig;
    /// <summary>
    /// 手牌管理器
    /// </summary>
    public OnHandChessManager onHandChessManager;

    private void Awake() {
        // 创建游戏
        battleField = new BattleField();
        // 创建玩家
        gameConfig.Players = CreatePlayers();
        gameConfig.Me = gameConfig.Players[0];
        // 游戏配置
        battleField.Setup(gameConfig);
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
