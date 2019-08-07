using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceTagView : MonoBehaviour {
    public Text NameText;
    public Text PriceText;
    public string heroTag;// 英雄tag
    public Button PurchaseButton;
    [HideInInspector]
    public GameObject heroObject;
    [HideInInspector]
    public CharacterPurchaseConfig purchaseConfig;
    [HideInInspector]
    public int purchaseListIndex;
    [HideInInspector]
    public BattleFiledGame battleFieldGame;

    private void Awake() {
        PurchaseButton.onClick.AddListener(OnClick);
    }
    private void OnClick() {
        if (heroObject != null) {
            if (BattleCharacterPurchaseManager.Instance.Purchase(purchaseListIndex, purchaseConfig)) {
                // 购买成功
                Destroy(heroObject);
                heroObject = null;
                // 创建一个英雄, 并放入手牌中
                var actor = HeroActor.CreateView(purchaseConfig);
                if (actor != null) {
                    actor.AddComponent<HeroActorDrag>();
                    actor.AddComponent<CapsuleCollider>();
                    battleFieldGame.onHandChessManager.AddChess(actor);
                }
            } else {
                // 购买失败
            }
        }
    }
}
