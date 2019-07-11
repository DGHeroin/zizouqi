using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public string NickName;
    public int HP;
    public int Money;
    public int Population;
    public int Exp;           // 经验数
    public Character[] Stage; // 战场
    public Character[] Bench; // 冷板凳
}

public class Character {
    bool IsOnStage;
    int x;
    int y;
}
