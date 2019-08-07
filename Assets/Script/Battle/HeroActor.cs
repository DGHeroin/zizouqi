using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {


    public bool Create(CharacterPurchaseConfig config) {
        //Debug.Log("创建英雄" + heroTag);
        do {
            // 创建英雄模型
            if (!CreateView(config)) {
                return false;
            }
        } while (false);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static GameObject CreateView(CharacterPurchaseConfig config) {
        // 创建模型
        var view = Instantiate(config.prefab);
        view.transform.localScale = Vector3.one * 4f;
        view.AddComponent<HeroActor>();
        return view;
    }
}
