using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIView : MonoBehaviour {
    public Text stateText; // 回合数
    public Text fightingText; // 战斗状态
    public BattleFiledGame battleFieldGame;

    const string templateStringPrepare = "<color=\"#00ff00\">倒计时:{0}</color>";
    const string templateStringFighing = "<color=\"#ff0000\">回合:{0}</color>";

    private void Start() {
        InvokeRepeating("UpdateUI", 0.5f, 0.2f); // invoke after 0.5s, and repeat every 0.2s
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareEnter, PrepareEnter);
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareLeave, PrepareLeave);
    }

    void UpdateUI() {
        var gameState = battleFieldGame.Game().GameState();
        if (gameState == BattleState.Prepare) { // 准备中
            // 显示倒计时
            stateText.text = string.Format(templateStringPrepare, battleFieldGame.gameConfig.currentPrepareTime);
        } else {
            // 隐藏倒计时
            stateText.text = string.Format(templateStringFighing, battleFieldGame.gameConfig.currentRound);
        }

        if (gameState == BattleState.Fighting) { // 战斗中 等待战斗结束
            fightingText.gameObject.SetActive(true);
        } else {
            fightingText.gameObject.SetActive(false);
        }
    }

    void PrepareEnter() {
        Debug.Log("展示购买英雄列表...................");
    }

    void PrepareLeave() {
        Debug.Log("隐藏列表, 显示战斗");
    }
}
