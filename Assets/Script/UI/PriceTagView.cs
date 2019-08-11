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
    public CharacterConfig purchaseConfig;
    [HideInInspector]
    public int purchaseListIndex;
    [HideInInspector]
    public BattleFiledGame battleFieldGame;

    private void Awake() {
        PurchaseButton.onClick.AddListener(OnClick);
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnClick() {
        if (heroObject != null) {
            if (BattleCharacterPurchaseManager.Instance.Purchase(purchaseListIndex, purchaseConfig)) {
                // 购买成功
                Destroy(heroObject);
                heroObject = null;
                // 创建一个英雄, 并放入手牌中
                var actor = HeroActor.CreateView(purchaseConfig.Tag);
                if (actor != null) {
                    actor.IsMyActor = true;
                    actor.IsInStage = false;
                    // 1. 放到自己手牌中
                    actor.gameObject.AddComponent<CapsuleCollider>();
                    // var pos = battleFieldGame.onHandChessManager.AddChess(actor);
                    var pos = BattleField.Current.GetEmptyHandPosition();
                    Debug.Log("放到位置:" + pos);
                    // 1. 放到棋盘地图中
                    BattleField.Current.MoveCharacter(actor, pos);
                }
            } else {
                // 购买失败
            }
        }
    }
}
