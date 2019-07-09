using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] CharacterList;

    [Header("显示的UI Slots")]
    public GameObject[] purchaseViewPlaceHolder;

    void Awake() {
        _Instance = this;
    }

    /// <summary>
    /// 创建新的购买列表
    /// </summary>
    public GameObject[] CreatePurchaseList() {
        int n = 5;
        GameObject[] result = new GameObject[5];
        for (int i = 0; i < n; i++) {
            var obj = RandomList();
            result[i] = obj;
        }
        return result;
    }
    /// <summary>
    /// 随机生成一个英雄
    /// </summary>
    /// <returns></returns>
    private GameObject RandomList() {
        return CharacterList[UnityEngine.Random.Range(0, CharacterList.Length - 1)];
    }

    public void DoCreatePurchaseList() {
        // 创建
        var list = CreatePurchaseList();
        // 显示到列表中
        for (int i = 0; i < list.Length; i++) {
            while (purchaseViewPlaceHolder[i].transform.childCount > 0) {
                DestroyImmediate(purchaseViewPlaceHolder[i].transform.GetChild(0).gameObject);
            }
            var obj = GameObject.Instantiate(list[i]);
            obj.transform.position = Vector3.zero;
            obj.transform.SetParent(purchaseViewPlaceHolder[i].transform, false);
        }
    }
}
