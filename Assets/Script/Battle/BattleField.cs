using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleField {
    /// <summary>
    /// 游戏配置
    /// </summary>
    private BattleGameConfig gameConfig;

    /// <summary>
    /// 游戏状态
    /// </summary>
    private BattleState state = BattleState.Init;

    private Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>(); // 游戏事件

    bool battleFinished = false; // 所有游戏结束

    private float lastPrepareTime = 0;
    private bool isEnterPrepare = true;

    static BattleField Current = null;
    public BattleField() {
        Current = this;
    }

    /// <summary>
    /// 所有玩家
    /// </summary>
    public Player[] Players = new Player[0];

    public void Setup(BattleGameConfig config) {
        this.gameConfig = config;
    }

    public void AddEvent(string key, UnityAction action) {
        if (!events.ContainsKey(key)) {
            events[key] = new UnityEvent();
        }
        events[key].AddListener(action);
    }

    public void RemoveEvent(string key, UnityAction action) {
        if (!events.ContainsKey(key)) {
            return;
        }
        events[key].RemoveListener(action);
    }

    public void StartGame() {
        Debug.Log("游戏开始");
        
    }

    public void UpdateGame() {
        this.CheckState(); // 检查状态
    }

    public void BattleStart() {
        Debug.Log("进入战斗");
        gameConfig.currentFightingTime = gameConfig.roundTime;
        emitEvent(BattleEvent.PrepareLeave);
        isEnterPrepare = true;
        this.changeState(BattleState.Fighting);
        fightingStartTime = Time.time;
        emitEvent(BattleEvent.FightingEnter);
        AudioManager.Instance.PlaySFX(gameConfig.AudioRoundStart);
    }

    public void RoundPrepare(int leftSeconds) {
        if (leftSeconds == 3) {
            AudioManager.Instance.PlaySFX(gameConfig.AudioRoundCountDown);
        }
    }

    public void RoundFighting(int leftSeconds) {
        
    }

    public void EndGame() {
        Debug.Log("游戏结束");
    }

    public void CheckState() {
        switch (this.state) {
            case BattleState.Init: // 初始化状态, 首次tick之后切到准备状态
                this.onStateInit();
                break;
            case BattleState.Prepare:
                this.onStatePrepare();
                break;
            case BattleState.Finished: // 游戏结束
                this.onStateFinished();
                break;
            case BattleState.Fighting:
                this.onStateFighting();
                break;
            default:
                return;
        }
    }

    private void changeState(BattleState newState) {
        this.state = newState;
    }

    #region 游戏状态
    public BattleState GameState() {
        return this.state;
    }
    private void onStateInit() {
        this.changeState(BattleState.Prepare);
        this.gameConfig.currentPrepareTime = this.gameConfig.prepareTime;
        this.StartGame();
    }

    private void onStateFinished() {
        this.EndGame();
    }

    private void onStatePrepare() {
        if (isEnterPrepare) {
            isEnterPrepare = false;
            Debug.Log("刚进入准备状态");
            emitEvent(BattleEvent.PrepareEnter);
        }
        if (Time.time - lastPrepareTime > 1) {
            lastPrepareTime = Time.time;
            // 更新时间
            gameConfig.currentPrepareTime--;
            if (gameConfig.currentPrepareTime == 0) {
                // 进入战斗
                BattleStart();
            } else {
                RoundPrepare(gameConfig.currentPrepareTime);
            }
        }
    }
    private void emitEvent(string key) {
        if (!events.ContainsKey(key)) {
            return;
        }
        events[key].Invoke();
    }
    private float fightingStartTime = 0;
    private float fightingTime = 0;
    /// <summary>
    /// 战斗
    /// </summary>
    private void onStateFighting() {
        if (Time.time - fightingTime > 1) {
            fightingTime = Time.time;
            gameConfig.currentFightingTime--;
            RoundFighting(gameConfig.currentFightingTime);
            if (gameConfig.currentFightingTime == 0) { // 战斗超时
                emitEvent(BattleEvent.FightingLeave);
                onRoundCompleted();
            }
        }
    }

    #endregion
    /// <summary>
    /// 回合结束
    /// </summary>
    private void onRoundCompleted() {
        bool IsWin = true;
        if (IsWin) {
            AudioManager.Instance.PlaySFX(gameConfig.AudioRoundWin);
        } else {
            AudioManager.Instance.PlaySFX(gameConfig.AudioRoundFail);
        }

        // TODO 检查游戏是否结束
        if (battleFinished) {
            emitEvent(BattleEvent.BattleComplete);
        } else {
            Debug.Log("重新进入准备");
            emitEvent(BattleEvent.RoundComplete);
            // 重新进入准备
            this.gameConfig.currentRound++;
            this.changeState(BattleState.Prepare);
            this.gameConfig.currentPrepareTime = this.gameConfig.prepareTime;
            emitEvent(BattleEvent.PrepareEnter);
        }
    }
}

public enum BattleState {
    Init,
    Prepare,
    Fighting,
    Finished,
}

public enum PlayerState {
    Loading,
    Prepare,
}

public static class BattleEvent {
    public const string PrepareEnter = "prepare.enter";   // 进入准备
    public const string PrepareLeave = "prepare.leave";   // 离开准备
    public const string FightingEnter = "fighting.enter"; // 进入战斗
    public const string FightingLeave = "fighting.leave"; // 离开战斗
    public const string RoundComplete = "round.complete"; // 回合结束
    public const string BattleComplete = "battle.complete"; // 游戏结束, 所有人死亡或者自己死亡
}
