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
    public BuyHeroView heroView;
    [HideInInspector]
    public int purchaseListIndex;
    [HideInInspector]
    public BattleFiledGame battleFieldGame;

    private void Awake() {
        PurchaseButton.onClick.AddListener(OnClick);
    }
    private void OnClick() {
        if (heroView != null) {
            if (BattleCharacterPurchaseManager.Instance.Purchase(purchaseListIndex, heroView)) {
                // 购买成功
                Destroy(heroView.gameObject);
                heroView = null;
                // 创建一个英雄, 并放入手牌中
                var prefab = Resources.Load<GameObject>("HeroActor/HeroActor");
                var actor = Instantiate(prefab);
                battleFieldGame.onHandChessManager.AddChess(actor);
                actor.GetComponent<HeroActor>().Create(heroTag);
            } else {
                // 购买失败
            }
        }
    }
}
