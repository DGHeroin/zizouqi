using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHeroView : MonoBehaviour {
    public string HeroTag = "";
    void Awake() {

    }

    void Start() {
        
    }

    void Update() {
        
    }

    public void OnBuyClicked() {
        Debug.Log(string.Format("购买英雄:{0}", HeroTag));
    }
}
