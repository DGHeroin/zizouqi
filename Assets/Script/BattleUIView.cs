using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIView : MonoBehaviour {
    public Text stateText; // 回合数
    public Text countDownText; // 倒计时
    public Text moneyText; // 金币数
    public BattleFiledGame battleFieldGame;

    const string templateStringPrepare = "<color=\"#31f33e\">准备回合 {0}</color>";
    const string templateStringFighing = "<color=\"#ff0000\">战斗回合 {0}</color>";

    private void Start() {
        InvokeRepeating("UpdateUI", 0.5f, 0.2f); // invoke after 0.5s, and repeat every 0.2s
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareEnter, PrepareEnter);
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareLeave, PrepareLeave);
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateUI() {
        var gameState = battleFieldGame.Game().GameState();
        if (gameState == BattleState.Prepare) { // 准备中
            // 显示倒计时
            Debug.Log("xxx:" + battleFieldGame.gameConfig.currentRound);
            stateText.text = string.Format(templateStringPrepare, battleFieldGame.gameConfig.currentRound);
            if (!countDownText.gameObject.activeInHierarchy) {
                countDownText.gameObject.SetActive(true);
            }
            countDownText.text = "" + battleFieldGame.gameConfig.currentPrepareTime;
        } else if (gameState == BattleState.Fighting) { // 战斗中显示战斗超时
            stateText.text = string.Format(templateStringFighing, battleFieldGame.gameConfig.currentRound);
            if (!countDownText.gameObject.activeInHierarchy) {
                countDownText.gameObject.SetActive(true);
            }
            countDownText.text = string.Format("<color=\"#ff0000\">{0}</color>", battleFieldGame.gameConfig.currentFightingTime);
        } else {
            // 隐藏倒计时
            countDownText.gameObject.SetActive(false);
        }
        moneyText.text = "" + battleFieldGame.gameConfig.Me.Money;
    }

    void PrepareEnter() {
        Debug.Log("展示购买英雄列表...................");
    }

    void PrepareLeave() {
        Debug.Log("隐藏列表, 显示战斗");
    }
}
