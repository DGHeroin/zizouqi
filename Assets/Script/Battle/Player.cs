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
    public Dictionary<string, HeroActor> Stage = new Dictionary<string, HeroActor>(); // 战场
    public Dictionary<string, HeroActor> Bench = new Dictionary<string, HeroActor>(); // 冷板凳
}
