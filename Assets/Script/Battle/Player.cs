using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public int HP;
    public int Money;
    public int Population;
    public Character[] Stage; // 战场
    public Character[] Bench; // 冷板凳
}

public class Character {
    bool IsOnStage;
    int x;
    int y;
}
