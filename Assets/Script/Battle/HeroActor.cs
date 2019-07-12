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

    private bool CreateView(string heroTag) {
        // 加载配置
        var obj = Resources.Load<CharacterAnimationConfig>("HeroView/" + heroTag);
        if (obj == null) {
            Debug.Log("模型丢失:" + heroTag);
            return false;
        }
        // 创建模型
        var view = Instantiate(obj.Prefab);
        Debug.Log("===>new :" + view);
        view.transform.SetParent(this.transform, false);
        view.transform.localScale = Vector3.one * 0.7f;

        return true;
    }
}
