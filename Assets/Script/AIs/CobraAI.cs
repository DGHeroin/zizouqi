using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraAI : MonoBehaviour, AIBase {
    HeroActor actor;
    float attackTimeDt = 0;
    private CommonAI cai;
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
    float lastAttackTime = 0;
    void DoFighing() {
        attackTimeDt += GameTime.DeltaTime;
        if (attackTimeDt >= actor.config.NormalAttackPeriod) {
            attackTimeDt = 0;
            // DEBUG
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

            if (nearActor != null) {
                actor.anim.AttackTransform = nearActor.gameObject.transform;
                actor.anim.DoNormalAttack(
                    actor.Id,               // 谁打的
                    actor.state.GetDamage() // 攻击量
                    );
            }
            lastAttackTime = GameTime.Time;
        }
    }

}
