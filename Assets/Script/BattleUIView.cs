using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUIView : MonoBehaviour {
    public Text stateText; // 回合数
    public Text countDownText; // 倒计时
    public Text moneyText; // 金币数
    public BattleFiledGame battleFieldGame;
    public Text gameStateText;// 游戏状态切换提示
    public Text populationText; // 人口
    public Text gameNotifyText;// 游戏提示

    const string templateStringPrepare = "<color=\"#31f33e\">准备回合 {0}</color>";
    const string templateStringFighing = "<color=\"#ff0000\">战斗回合 {0}</color>";
    public static BattleUIView Current;

    #region Camera
    public Transform cameraPreparePosition;
    public Transform cameraBattlePosition;
    public Transform theCamera;
    #endregion

    [SerializeField]
    private GameObject startFightButton = null;

    private void Awake() {
        Current = this;
    }

    private void Start() {
        InvokeRepeating("UpdateUI", 0.5f, 0.2f); // invoke after 0.5s, and repeat every 0.2s
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareEnter, PrepareEnter);
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareLeave, PrepareLeave);

        battleFieldGame.Game().AddEvent(BattleEvent.FightingEnter, RoundStart);
        battleFieldGame.Game().AddEvent(BattleEvent.RoundComplete, RoundEnd);
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
        } else if (gameState == BattleState.RoundFighting) { // 战斗中显示战斗超时
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

        // 人口
        var me = BattleField.Current.gameConfig.Me;
        if (me != null) {
            var c = me.Stage.Values.Count;
            populationText.text = string.Format("{0}/{1}", c, me.Population);
        }
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
        ShowStageText("准备战斗");
        if (!BattleField.Current.IsGameEnd) {
            startFightButton.SetActive(true);
        }
        
        if (IsPurchasePanelShowing) {
            return;
        }
        purchasePanel.SetActive(true);
        IsPurchasePanelAutoOpen = true;
        IsPurchasePanelShowing = true;
    }

    void PrepareLeave() {
        Debug.Log("隐藏列表, 显示战斗");
        startFightButton.SetActive(false);

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
    #region 状态切换提示
    bool isShowingStateText = false;
    public void ShowStageText(string text) {
        StartCoroutine(EShowStageText(text));
    }
    private void privShowStageText(string text) {
        gameStateText.text = text;
        gameStateText.gameObject.SetActive(true);
        gameStateText.DOFade(0f, 2f).From().SetEase(Ease.OutQuad).OnComplete(() => {
            gameStateText.gameObject.SetActive(false);
            isShowingStateText = false;
        });
    }
    private IEnumerator EShowStageText(string text) {
        do {
            if (isShowingStateText) {
                yield return new WaitForEndOfFrame();
                continue;
            }
            isShowingStateText = true;
            privShowStageText(text);
            break;
        } while (true);
    }
    #endregion

    #region 状态切换提示
    bool isShowingNotifyText = false;
    public void ShowNotifyText(string text) {
        StartCoroutine(EShowNotifyText(text));
    }
    private void privShowNotifyText(string text) {
        gameNotifyText.text = text;
        gameNotifyText.gameObject.SetActive(true);
        gameNotifyText.DOFade(0f, 2f).From().SetEase(Ease.OutQuad).OnComplete(() => {
            gameNotifyText.gameObject.SetActive(false);
            isShowingNotifyText = false;
        });
    }
    private IEnumerator EShowNotifyText(string text) {
        do {
            if (isShowingNotifyText) {
                yield return new WaitForEndOfFrame();
                continue;
            }
            isShowingNotifyText = true;
            privShowNotifyText(text);
            break;
        } while (true);
    }
    #endregion

    public void RoundStart() {
        ShowStageText("回合开始");

        Invoke("CameraMove1", 0.5f);
    }


    public void RoundEnd() {
        Debug.Log("回合结束");
        StartCoroutine(ERoundEndSeq()); // 回合结束流程
        Invoke("CameraMove2", 1);
    }

    private void CameraMove1() {
        var seq = DOTween.Sequence();
        var m0 = theCamera.DOMove(cameraBattlePosition.position, 0.5f);
        var r0 = theCamera.DORotate(cameraBattlePosition.localEulerAngles, 0.5f);

        seq.Append(m0);
        seq.Append(r0);
    }
    private void CameraMove2() {
        var seq = DOTween.Sequence();
        var m0 = theCamera.DOMove(cameraPreparePosition.position, 0.5f);
        var r0 = theCamera.DORotate(cameraPreparePosition.localEulerAngles, 0.5f);

        seq.Append(m0);
        seq.Append(r0);
    }

    private IEnumerator ERoundEndSeq() {
        yield return new WaitForSeconds(2.0f);
        var gameConfig = BattleField.Current.gameConfig;
        switch (BattleField.Current.roundEndType) {
            case RoundEndType.Draw:
                ShowStageText("平局");
                break;
            case RoundEndType.Win:
                ShowStageText("胜利");
                AudioManager.Instance.PlaySFX(gameConfig.AudioRoundWin);
                break;
            case RoundEndType.Lose:
                ShowStageText("失败");
                AudioManager.Instance.PlaySFX(gameConfig.AudioRoundFail);
                break;
        }
        yield return new WaitForSeconds(2);
        BattleField.Current.RestartRoundGame(); // 重新开始
    }

    public void OnClickedStartFightNow() {
        BattleField.Current.StartFightNow();
        startFightButton.SetActive(false);
    }

    /// <summary>
    /// 增加人口
    /// </summary>
    public void OnClickedAddPopulation() {
        BattleField.Current.AddPopulation();
    }
}
