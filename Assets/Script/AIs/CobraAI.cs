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
            case BattleState.Fighting:
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
            actor.anim.AttackTransform = this.transform;// 打自己
            actor.anim.DoNormalAttack();
            lastAttackTime = GameTime.Time;
        }
    }

}
