using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {


    public bool Create(string heroTag) {
        Debug.Log("创建英雄" + heroTag);
        do {
            // 创建英雄模型
            if (!CreateView(heroTag)) {
                return false;
            }
        } while (false);
        return true;
    }

    public static GameObject CreateView(string heroTag) {
        // 加载配置it
        var obj = Resources.Load<CharacterAnimationConfig>("HeroView/" + heroTag);
        if (obj == null) {
            Debug.Log("模型丢失:" + heroTag);
            return null;
        }

        // 创建模型
        var view = Instantiate(obj.Prefab);
        //view.transform.SetParent(transform, false);
        view.transform.localScale = Vector3.one * 0.7f;
        view.AddComponent<HeroActor>();
        return view;
    }
}
