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
    #region 购买面板
    /// <summary>
    /// 系统自动打开
    /// </summary>
    public bool IsPurchasePanelAutoOpen = false;
    public bool IsPurchasePanelShowing = false;
    public GameObject purchasePanel;

    public void SetPanelOpenState(bool b) {
        IsPurchasePanelAutoOpen = false;
        IsPurchasePanelShowing = true;
    }

    void PrepareEnter() {
        Debug.Log("展示购买英雄列表...................");
        if (IsPurchasePanelShowing) {
            return;
        }
        purchasePanel.SetActive(true);
        IsPurchasePanelAutoOpen = true;
        IsPurchasePanelShowing = true;
    }

    void PrepareLeave() {
        Debug.Log("隐藏列表, 显示战斗");
        if (!IsPurchasePanelShowing) {
            return;
        }
        if (!IsPurchasePanelAutoOpen) { // 如果不是自动打开, 就不要自动隐藏
            return;
        }
        
        purchasePanel.SetActive(false);
        IsPurchasePanelAutoOpen = false;
        IsPurchasePanelShowing = false;
    }
    #endregion
}
