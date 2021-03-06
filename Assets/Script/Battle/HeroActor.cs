﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {

    public string Id { get; private set; }

    public string ChessBoardPosition = null;
    public AIBase ai = null;
    public CharacterConfig config;
    public bool IsMyActor = false;
    public bool IsInStage = false; // 是否在棋盘中
    public int TeamId = -1;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static HeroActor CreateView(string tag) {
        var actorConfig = BattleField.Current.GetAnimationConfig(tag);
        Debug.LogFormat("创建英雄:{0} {1}", tag, actorConfig);
        // 创建模型
        var obj = Instantiate(actorConfig.Prefab);
        obj.transform.localScale = Vector3.one * 5f;
        var actor = obj.AddComponent<HeroActor>();
        // id
        actor.Id = System.Guid.NewGuid().ToString();
        actor.tag = "character";
        actor.config = actorConfig;
        // animate
        var anim = obj.AddComponent<CharacterBase>();
        anim.config = actorConfig;
        actor.anim = anim;
        // AI
        var ai = Instantiate(actorConfig.AIPrefab);
        ai.transform.SetParent(obj.transform);
        // actor state
        actor.state.aiState = ActorAIState.Idle;
        actor.state.HP = actorConfig.HP;
        actor.state.Damage = actorConfig.Damage;
        return actor;
    }

    
    public void UpdateGame() {
        if (!IsAlive) { return; }

        updateView(); // 更新视野
        if (ai != null) {
            ai.UpdateGame();
        }
    }
    /// <summary>
    /// 更新视野, 构建我能看到的队友和敌人列表
    /// </summary>
    void updateView() {
        var bf = BattleField.Current;
        var actors = bf.GetActors();
        this.teamMate.Clear();
        this.teamEnemy.Clear();
        foreach(var actor in actors) {
            if(actor.Id == this.Id) { continue; }
            ActorView view = null;
            if (cacheView.ContainsKey(actor.Id)) {
                view = cacheView[actor.Id]; // 之前已经遇见了
            } else { // 发现新敌人
                view = new ActorView();
                view.actor = actor;
                cacheView[actor.Id] = view;
            }
            // TODO 计算它的值
            view.Hate = 1;
            view.Distance = 1;

            if (actor.TeamId == this.TeamId) {
                teamMate.Add(view);
            } else {
                teamEnemy.Add(view);
            }
        }
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(string whoAttackMe, int damage) {
        state.deltaHP -= damage;
        anim.PerformTakeDamage();
        if (state.GetHP() == 0) {
            Debug.LogFormat("我{0}被{1}, 打死了", this.Id, whoAttackMe);
            anim.PerformDie(()=> {
                if(this.IsMyActor) {
                    this.gameObject.transform.position = Vector3.one * 10000; // 如果是我的英雄, 则放到一个超远距离, 以备下一局使用
                } else {
                    Destroy(this.gameObject); // 释放gameobject
                }
                
                BattleField.Current.RemoveActor(this.ChessBoardPosition); // 在地图上删除
            });

        }
        Debug.LogFormat("打中扣血{0} {1} => {2} {3}/{4}", this.Id, whoAttackMe, damage, state.GetHP(), state.HP);
    }

    public bool IsAlive { get { return state.GetHP() > 0; } }

    public CharacterBase anim;
    public List<ActorView> teamMate = new List<ActorView>();
    public List<ActorView> teamEnemy = new List<ActorView>();
    private Dictionary<string, ActorView> cacheView = new Dictionary<string, ActorView>();
    public ActorState state = new ActorState();
    
}

public enum ActorAIState {
    Idle,     // 空闲 不做其他AI计算
    Guarding, // 警戒中, 此时做寻路, 搜索最仇恨值最高的敌人
    Dead,     // 死亡了
}

public class ActorView {
    public HeroActor actor;
    /// <summary>
    /// 仇恨值
    /// </summary>
    public int Hate;
    public int Distance;
}

public class ActorState {
    public int HP;
    public int Damage;
    public int deltaHP;
    public int DeltaDamage;
    public ActorAIState aiState = ActorAIState.Idle;

    public int GetHP() {
        var result = HP + deltaHP;
        if (result <= 0) {
            return 0;
        }
        return result;
    }

    public int GetDamage() {
        return Damage + DeltaDamage;
    }
}