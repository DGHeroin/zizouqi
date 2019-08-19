using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleField {
    /// <summary>
    /// 游戏配置
    /// </summary>
    public BattleGameConfig gameConfig { get; private set; }

    /// <summary>
    /// 玩家匹配器
    /// </summary>
    private PlayerSelector playerSelector;

    private CharacterConfig[] characterList;

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
    public bool IsGameEnd { get; private set; } = false;

    /// <summary>
    /// 游戏帧数
    /// </summary>
    private uint gameTick = 0;
    public RoundEndType roundEndType = RoundEndType.Draw;
    public static BattleField Current { get; private set; }
    public BattleField() {
        Current = this;
        // 初始化配置
    }

    public void ClearHeroActor(List<HeroActor> list) {
        foreach(var actor in list) {
            if (actor.IsAlive) {
                Object.Destroy(actor.gameObject);
            }
        }
        list.Clear();
    }

    /// <summary>
    /// 所有玩家
    /// </summary>
    public Player[] Players = new Player[0];

    public void Setup(BattleGameConfig config, PlayerSelector selector, 
        CharacterConfig[] CharacterList) {
        this.gameConfig = config;
        this.playerSelector = selector;
        this.characterList = CharacterList;
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
                return p.Prefab;
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
        GameTime.DeltaTime = Time.deltaTime;
        this.gameTick++;
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
        this.changeState(BattleState.RoundFighting);
        RoundStartTime = Time.time; // 回合开始时间
        emitEvent(BattleEvent.FightingEnter);
        AudioManager.Instance.PlaySFX(gameConfig.AudioRoundStart);

        allHero.Clear();
        // 1. 根据玩家匹配器获取玩家阵容
        var config = playerSelector.GetPlayerLevelConfig();
        // 2. 创建对手的替身
        ClearHeroActor(this.otherHero);
        foreach (var p in config.Heroes) {
            var actor = HeroActor.CreateView(p.CharacterTag);
            actor.IsInStage = true;
            actor.TeamId = 1;
            this.MoveActor(actor, p.Position);
            this.otherHero.Add(actor);
            allHero.Add(actor);
            // 面向
            actor.transform.localEulerAngles = new Vector3(0, 180, 0);
            // 回血
            actor.state.deltaHP = 0;
        }
        // 3. 把我自己战场上的角色加到列表中
        myHero.Clear();
        var me = this.gameConfig.Me;
        foreach(var pair in me.Stage) {
            var actor = pair.Value;
            allHero.Add(actor);
            // 回血
            actor.state.deltaHP = 0;
            myHero.Add(actor);
            MoveActor(actor, actor.ChessBoardPosition);
        }
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
            case BattleState.GameFinished: // 游戏结束
                this.onStateFinished();
                break;
            case BattleState.RoundFighting:
                this.onStateFighting();
                break;
            case BattleState.RoundEnding:
                break; // do nothing, just wait
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
    private float RoundStartTime = 0;
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

        // 检查双方是否还有人生存
        var team1 = GetActors(0);
        var team2 = GetActors(1);
        if (team1.Count == 0 && team2.Count == 0) {
            Debug.Log("平局");
            roundEndType = RoundEndType.Draw;
            onRoundCompleted();
            return;
        }
        if (team1.Count > 0 && team2.Count == 0) {
            Debug.Log("team 1 win");
            roundEndType = RoundEndType.Win;
            onRoundCompleted();
            return;
        }
        if (team2.Count > 0 && team1.Count == 0) {
            Debug.Log("team 2 win");
            roundEndType = RoundEndType.Lose;
            onRoundCompleted();
            return;
        }
    }

    #endregion
    /// <summary>
    /// 回合结束
    /// </summary>
    private void onRoundCompleted() {
        changeState(BattleState.RoundEnding);
        emitEvent(BattleEvent.FightingLeave);
        // all game end 检查游戏是否结束
        if (battleFinished) {
            emitEvent(BattleEvent.BattleComplete);
        } else {
            emitEvent(BattleEvent.RoundComplete);
        }
    }
    
    public void RestartRoundGame() {
        Debug.Log("重新进入准备");
        // 重新进入准备
        this.gameConfig.currentRound++;
        this.changeState(BattleState.Prepare);
        this.gameConfig.currentPrepareTime = this.gameConfig.prepareTime;
        emitEvent(BattleEvent.PrepareEnter);

        // 让我的英雄归位
        var me = this.gameConfig.Me;
        foreach (var pair in me.Stage) {
            var actor = pair.Value;
            allHero.Add(actor);
            // 回血
            actor.state.deltaHP = 0;
            myHero.Add(actor);
            MoveActor(actor, actor.ChessBoardPosition);
        }
    }

    public void MoveActor(HeroActor actor, string to) {
        string from = actor.ChessBoardPosition;
        if (from != null) {
            this.CharacterMap.Remove(from);
        }
        this.CharacterMap[to] = actor;
        actor.ChessBoardPosition = to;
        actor.transform.SetParent(null);
        var chessBlock = ChessBlockManager.Current.GetChessBlock(to);
        
        Debug.Log("角色移动" + actor + " => " + to);
        if (actor.IsMyActor) { // 如果是我的英雄, 这里要判断, 是在手牌中, 还是在棋盘上
            var me = this.gameConfig.Me;
            var key = actor.Id;
            if (to.StartsWith("A")) { // 放到手牌
                if (me.Stage.ContainsKey(key)) { // remove if in stage
                    me.Stage.Remove(key);
                }
                if (!me.Bench.ContainsKey(key)) { // add if not in bench
                    me.Bench[key] = actor;
                }
            } else if (to.StartsWith("B")) { // 放到战场
                if (me.Bench.ContainsKey(key)) { // remove if in bench
                    me.Bench.Remove(key);
                }
                if (!me.Stage.ContainsKey(key)) { // add if not in stage
                    me.Stage[key] = actor;
                }
            }
        }

        actor.transform.position = chessBlock.transform.position;
    }

    public HeroActor GetMap(string pos) {
        if(!this.CharacterMap.ContainsKey(pos)) {
            return null;
        }
        return this.CharacterMap[pos];
    }
    public void RemoveActor(string pos) {
        if (!this.CharacterMap.ContainsKey(pos)) {
            return;
        }
        this.CharacterMap.Remove(pos);
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
    public CharacterConfig[] GetCharacterList() {
        return this.characterList;
    }
    public CharacterConfig GetAnimationConfig(string tag) {
        foreach(var config in this.characterList) {
            if (config.Tag == tag) {
                return config;
            }
        }
        Debug.LogError("tag not found:" + tag);
        return null;
    }
    #endregion

    public uint GetTick() {
        return gameTick;
    }

    public List<HeroActor> GetActors() {
        return allHero;
    }

    public List<HeroActor> GetActors(int teamType, bool alive=true) {
        var result = new List<HeroActor>();
        foreach (var actor in allHero) {
            if (actor.TeamId == teamType) {
                if (alive) {
                    if (actor.state.GetHP() > 0) {
                        result.Add(actor);
                    }
                } else {
                    result.Add(actor);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 主动出战
    /// </summary>
    public void StartFightNow() {
        if(this.state != BattleState.Prepare) { return; }
        // 进入战斗
        BattleStart();
    }
    /// <summary>
    /// 增加人口
    /// </summary>
    public void AddPopulation() {
        if (gameConfig.Me.Money >= 5) {
            gameConfig.Me.Money -= 5;
            gameConfig.Me.Population++;
        } else {
            BattleUIView.Current.ShowNotifyText("金币不足");
        }
    }
}

public enum BattleState {
    Init,
    Prepare,
    RoundFighting,
    RoundEnding,  // 回合结束, 但还没进入到下一局准备
    GameFinished,
}

public enum PlayerState {
    Loading,
    Prepare,
}

public static class BattleEvent {
    public const string PrepareEnter = "prepare.enter";   // 进入准备
    public const string PrepareLeave = "prepare.leave";   // 离开准备
    public const string FightingEnter = "round.enter"; // 进入战斗
    public const string FightingLeave = "round.leave"; // 离开战斗
    public const string RoundComplete = "round.complete"; // 回合结束
    public const string BattleComplete = "battle.complete"; // 游戏结束, 所有人死亡或者自己死亡
}

public static class GameTime {
    public static float Time = 0;
    public static float DeltaTime = 0;
    public static uint Tick = 0;
}

public enum RoundEndType {
    Draw,
    Win,
    Lose,
}