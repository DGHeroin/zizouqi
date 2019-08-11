using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraAI : MonoBehaviour, AIBase {
    float lastAttackTime = 0;
    HeroActor actor;
    float attackTimeDt = 0;
    private void Start() {
        actor = GetComponentInParent<HeroActor>();
        actor.ai = this;
    }

    public void UpdateGame() {
        var bf = BattleField.Current;
        switch (bf.GameState()) {
            case BattleState.Fighting:
                DoFighing();
                break;
            case BattleState.Prepare:
                break;
        }

        
    }
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
