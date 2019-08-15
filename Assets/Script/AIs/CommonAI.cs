using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonAI : MonoBehaviour {
    public HeroActor actor;

    public int UpdateAI() {
        // 
        var bf = BattleField.Current;
        switch (bf.GameState()) {
            case BattleState.Fighting: // 战斗中
                break;
            case BattleState.Prepare:
                return 1;
        }

        return 0; // continue run ai
    }
}
