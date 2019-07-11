using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceTagView : MonoBehaviour {
    public Text NameText;
    public Text PriceText;
    public Button PurchaseButton;
    [HideInInspector]
    public BuyHeroView heroView;
    [HideInInspector]
    public int purchaseListIndex;

    private void Awake() {
        PurchaseButton.onClick.AddListener(OnClick);
    }
    private void OnClick() {
        if (heroView != null) {
            if (BattleCharacterPurchaseManager.Instance.Purchase(purchaseListIndex, heroView)) {
                // 购买成功
                Destroy(heroView.gameObject);
                heroView = null;
            } else {
                // 购买失败
            }
        }
    }
}
