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
    /// 玩家匹配器
    /// </summary>
    private PlayerSelector playerSelector;

    private CharacterPurchaseConfig[] characterList;
    private CharacterAnimationConfig[] animationConfig;

    /// <summary>
    /// 游戏状态
    /// </summary>
    private BattleState state = BattleState.Init;

    private Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>(); // 游戏事件

    private Dictionary<string, HeroActor> CharacterMap = new Dictionary<string, HeroActor>(); // 站位信息

    bool battleFinished = false; // 所有游戏结束

    private float lastPrepareTime = 0;
    private bool isEnterPrepare = true;

    private List<HeroActor> allHero = new List<HeroActor>();
    private List<HeroActor> myHero = new List<HeroActor>();
    private List<HeroActor> otherHero = new List<HeroActor>();

    static BattleField _Current = null;
    public static BattleField Current {
        get {
            return _Current;
        }
    }
    public BattleField() {
        _Current = this;
        // 初始化配置
    }

    public void ClearHeroActor(List<HeroActor> list) {
        foreach(var actor in list) {
            Object.Destroy(actor.gameObject);
        }
        list.Clear();
    }

    /// <summary>
    /// 所有玩家
    /// </summary>
    public Player[] Players = new Player[0];

    public void Setup(BattleGameConfig config, PlayerSelector selector, 
        CharacterPurchaseConfig[] CharacterList,
        CharacterAnimationConfig[] AnimationConfig) {
        this.gameConfig = config;
        this.playerSelector = selector;
        this.characterList = CharacterList;
        this.animationConfig = AnimationConfig;
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

    public GameObject GetCharacterPrefab(string tag) {
        foreach(var p in this.characterList) {
            if (p.Tag == tag) {
                return p.prefab;
            }
        }
        return null;
    }

    public void StartGame() {
        Debug.Log("游戏开始");
        if (gameConfig.currentRound == 1) {
            Debug.Log("新来的玩家? 来一杯阿帕茶.");
        }
    }

    public void UpdateGame() {
        GameTime.Time += Time.deltaTime;
        this.CheckState(); // 检查状态

        foreach(var actor in allHero) {
            actor.UpdateGame();
        }
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

        allHero.Clear();
        // 1. 根据玩家匹配器获取玩家阵容
        var config = playerSelector.GetPlayerLevelConfig();
        // 2. 创建对手的替身
        ClearHeroActor(this.otherHero);
        foreach (var p in config.Heroes) {
            var actor = HeroActor.CreateView(p.CharacterTag);
            this.MoveCharacter(actor, p.Position);
            this.otherHero.Add(actor);
            allHero.Add(actor);
            // 面向
            actor.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        // 3. TODO 把我自己战场上的角色加到列表中
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

    public void MoveCharacter(HeroActor actor, string to) {
        string from = actor.ChessBoardPosition;
        if (from != null) {
            this.CharacterMap.Remove(from);
        }
        this.CharacterMap[to] = actor;
        actor.ChessBoardPosition = to;
        actor.transform.SetParent(null);
        var chessBlock = ChessBlockManager.Current.GetChessBlock(to);
        actor.transform.position = chessBlock.transform.position;
        Debug.Log("角色移动" + actor + " => " + to);
        
    }

    public HeroActor GetMap(string pos) {
        if(!this.CharacterMap.ContainsKey(pos)) {
            return null;
        }
        return this.CharacterMap[pos];
    }
    #region 手牌
    readonly string[] fixedHandPosition = new string[] {
            "AA1", "AA2", "AA3", "AA4",
            "AA5", "AA6", "AA7", "AA8",
        };
    /// <summary>
    /// 获取手牌空余位置
    /// </summary>
    /// <returns></returns>
    public string GetEmptyHandPosition() {
        foreach(var p in fixedHandPosition) {
            if (!this.CharacterMap.ContainsKey(p)) {
                return p;
            }
        }

        return null;
    }

    public int GetHandCharacterCount() {
        int c = 0;
        foreach (var p in fixedHandPosition) {
            if (this.CharacterMap.ContainsKey(p)) {
                c++;
            }
        }

        return c;
    }
    #endregion

    #region GameConfig

    /// <summary>
    /// 获取当前回合数
    /// </summary>
    /// <returns></returns>
    public int GetRound() {
        return gameConfig.currentRound;
    }
    public CharacterPurchaseConfig[] GetCharacterList() {
        return this.characterList;
    }
    public CharacterAnimationConfig GetAnimationConfig(string tag) {
        foreach(var config in this.animationConfig) {
            if (config.Tag == tag) {
                return config;
            }
        }
        Debug.LogError("tag not found:" + tag);
        return null;
    }
    #endregion
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

public static class GameTime {
    public static float Time = 0;
}