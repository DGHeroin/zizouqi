using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIView : MonoBehaviour {
    public Text roundText; // 回合数
    public Text prepareTime; // 准备状态倒计时
    public Text fightingText; // 战斗状态
    public BattleFiledGame battleFieldGame;

    private void Start() {
        InvokeRepeating("UpdateUI", 0.5f, 0.2f); // invoke after 0.5s, and repeat every 0.2s
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareEnter, PrepareEnter);
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareLeave, PrepareLeave);
    }

    void UpdateUI() {
        roundText.text = string.Format("回合:{0}", battleFieldGame.gameConfig.round);
        var gameState = battleFieldGame.Game().GameState();
        if (gameState == BattleState.Prepare) { // 准备中
            // 显示倒计时
            prepareTime.gameObject.SetActive(true);
            prepareTime.text = string.Format("倒计时:{0}", battleFieldGame.gameConfig.currentPrepareTime);
        } else {
            // 隐藏倒计时
            prepareTime.gameObject.SetActive(false);
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
        //battleFieldGame.Game().RemoveEvent(BattleEvent.PrepareEnter, PrepareEnter);
    }
}
