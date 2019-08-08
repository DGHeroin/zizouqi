using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {

    public string Id { get; private set; }

    public string ChessBoardPosition = null;

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
    public static HeroActor CreateView(CharacterPurchaseConfig config) {
        // 创建模型
        var obj = Instantiate(config.prefab);
        obj.transform.localScale = Vector3.one * 4f;
        var actor = obj.AddComponent<HeroActor>();
        // id
        actor.Id = System.Guid.NewGuid().ToString();
        Debug.Log("=================>" + actor.Id);
        return actor;
    }
}
