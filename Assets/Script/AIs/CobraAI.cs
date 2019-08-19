using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraAI : MonoBehaviour, AIBase {
    HeroActor actor;
    float attackTimeDt = 0;
    float ultimateTimeDt = 0;
    private CommonAI cai;

    bool isAttackPlaying = false;
    private void Start() {
        actor = GetComponentInParent<HeroActor>();
        actor.ai = this;
        cai = this.gameObject.AddComponent<CommonAI>();
    }

    public void UpdateGame() {
        int rs = cai.UpdateAI();
        switch (rs) {
            case 0: // continue
                break;
            default:
                return;// block other AI
        }

        var bf = BattleField.Current;
        switch (bf.GameState()) {
            case BattleState.RoundFighting:
                DoFighing();
                break;
            case BattleState.Prepare:
                break;
        }
    }
    void DoFighing() {
        attackTimeDt += GameTime.DeltaTime;
        ultimateTimeDt += GameTime.DeltaTime;

        // if (isAttackPlaying) { return; }

        // Debug.LogFormat("大招时间: {0} => {1}", attackTimeDt, actor.config.ultimateTime);
        // 大招
        if (ultimateTimeDt >= actor.config.ultimateTime) {
            HeroActor nearActor = FindNearActor();
            if (nearActor != null) {
                actor.anim.AttackTransform = nearActor.gameObject.transform;
                isAttackPlaying = true; // 开始动画
                actor.anim.DoUltimateAttack(
                    actor.Id,               // 谁打的
                    actor.state.GetDamage(), // 攻击量
                    ()=>{
                        // 攻击结束
                        isAttackPlaying = false;
                    }
                    );
            }

            ultimateTimeDt = 0;
            attackTimeDt = 0; // 释放大招后, 普通攻击的时间也要重新计算
            return;
        }

        // 普通攻击
        if (attackTimeDt >= actor.config.NormalAttackPeriod) {
            attackTimeDt = 0;
            HeroActor nearActor = FindNearActor();
            if (nearActor != null) {
                actor.anim.AttackTransform = nearActor.gameObject.transform;
                isAttackPlaying = true; // 开始动画
                actor.anim.DoNormalAttack(
                    actor.Id,               // 谁打的
                    actor.state.GetDamage(), // 攻击量
                    () => {
                        // 攻击结束
                        isAttackPlaying = false;
                    }
                    );
            }
        }
    }

    private HeroActor FindNearActor() {
        var teamId = actor.TeamId == 0 ? 1 : 0;
        var others = BattleField.Current.GetActors(teamId);
        // 查找最接近自己的敌人
        var myPos = GameUtility.MapToPosition(actor.ChessBoardPosition);
        HeroActor nearActor = null;
        int minDistance = int.MaxValue;
        foreach (var o in others) {
            var pos = GameUtility.MapToPosition(o.ChessBoardPosition);
            var dis = pos.Distance(myPos);
            if (dis < minDistance) {
                minDistance = dis;
                nearActor = o;
            }
        }
        return nearActor;
    }

}
