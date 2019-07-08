using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFiledGame : MonoBehaviour {
    BattleField battleField;
    public BattleGameConfig gameConfig;
    private void Awake() {
        // 游戏配置
        // 创建游戏
        battleField = new BattleField();
        battleField.Setup(gameConfig);
    }

    void Update() {
        battleField.UpdateGame();
    }

    public BattleField Game() {
        return battleField;
    }
}
