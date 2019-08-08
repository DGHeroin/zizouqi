using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 英雄购买管理
/// </summary>
public class BattleCharacterPurchaseManager : MonoBehaviour {
    private static BattleCharacterPurchaseManager _Instance;
    public static BattleCharacterPurchaseManager Instance {
        get {
            return _Instance;
        }
    }

    [Header("英雄列表")]
    public CharacterPurchaseConfig[] CharacterList;

    [Header("显示的UI Slots")]
    public GameObject[] purchaseViewPlaceHolder;

    private GameObject[] CurrentHeroList; // 当前可以购买的英雄

    [Header("价格标签")]
    public GameObject[] priceTags;

    public BattleFiledGame battleFieldGame;

    /// <summary>
    /// 锁定这次购买
    /// </summary>
    private bool IsLockCurrentList;

    void Awake() {
        _Instance = this;
    }

    private void Start() {
        BindEvents();
    }

    void BindEvents() {
        // 监听回合开始. 如果回合开始, 则自动刷新购买列表
        battleFieldGame.Game().AddEvent(BattleEvent.PrepareEnter, onRoundStart);
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    void onRoundStart() {
        if (IsLockCurrentList == true) {
            IsLockCurrentList = false;
            return; // 本次锁定, 不改变购买列表
        }
        DoCreatePurchaseList();
    }

    /// <summary>
    /// 创建新的购买列表
    /// </summary>
    public CharacterPurchaseConfig[] CreatePurchaseList() {
        int n = 5;
        CharacterPurchaseConfig[] result = new CharacterPurchaseConfig[5];
        for (int i = 0; i < n; i++) {
            var config = RandomList();
            result[i] = config;
        }
        return result;
    }
    /// <summary>
    /// 随机生成一个英雄
    /// </summary>
    /// <returns></returns>
    private CharacterPurchaseConfig RandomList() {
        return CharacterList[UnityEngine.Random.Range(0, CharacterList.Length - 1)];
    }

    /// <summary>
    /// 
    /// </summary>
    public void DoCreatePurchaseList() {
        // 创建
        var list = CreatePurchaseList();
        CurrentHeroList = new GameObject[list.Length];
        // 显示到列表中
        for (int i = 0; i < list.Length; i++) {
            while (purchaseViewPlaceHolder[i].transform.childCount > 0) {
                DestroyImmediate(purchaseViewPlaceHolder[i].transform.GetChild(0).gameObject);
            }
            var config = list[i];
            var obj = Instantiate<GameObject>(config.prefab);
            CurrentHeroList[i] = obj;

            obj.transform.position = Vector3.zero;
            obj.transform.SetParent(purchaseViewPlaceHolder[i].transform, false);

            obj.AddComponent<PurchaseView>().faceTo = characterFaceTo;

            var priceTagView = priceTags[i].GetComponent<PriceTagView>();
            priceTags[i].SetActive(true);
            priceTagView.NameText.text = config.Tag;
            priceTagView.PriceText.text = "" + config.Price;
            priceTagView.heroObject = obj;
            priceTagView.purchaseConfig = config;
            priceTagView.purchaseListIndex = i;
            priceTagView.battleFieldGame = battleFieldGame;
            priceTagView.heroTag = config.Tag;
        }
    }

    public Transform characterFaceTo;

    /// <summary>
    /// 执行购买
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool Purchase(int idx, CharacterPurchaseConfig config) {
        if (battleFieldGame.gameConfig.Me.Money < config.Price) {
            // 金币不够
            Debug.Log("金币不够");
            return false;
        }

        if (battleFieldGame.onHandChessManager.Count() == battleFieldGame.gameConfig.gamePlayMaxOnHandPlayer) {
            // 手牌已满
            Debug.Log("手牌满了");
            return false;
        }
        battleFieldGame.gameConfig.Me.Money -= config.Price; // 扣币
        priceTags[idx].SetActive(false); // 隐藏价格标签
        CurrentHeroList[idx].SetActive(false);
        return true;
    }
    public Transform baseObj;
    public void TestLookAt(Transform testLookAt) {
        testLookAt.DOLookAt(Camera.main.transform.position, 2, AxisConstraint.None, baseObj.up);
    }
}
