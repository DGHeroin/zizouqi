using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {

    public string Id { get; private set; }

    public string ChessBoardPosition = null;
    public AIBase ai = null;
    public CharacterConfig config;
    public bool IsMyActor = false;
    public bool IsInStage = false; // 是否在棋盘中

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static HeroActor CreateView(string tag) {
        var cfg = BattleField.Current.GetAnimationConfig(tag);
        // 创建模型
        var obj = Instantiate(cfg.Prefab);
        obj.transform.localScale = Vector3.one * 4f;
        var actor = obj.AddComponent<HeroActor>();
        // id
        actor.Id = System.Guid.NewGuid().ToString();
        actor.tag = "character";
        actor.config = cfg;
        // 动画控制
        var anim = obj.AddComponent<CharacterBase>();
        anim.config = cfg;
        actor.anim = anim;
        // AI
        var ai = Instantiate(cfg.AIPrefab);
        ai.transform.SetParent(obj.transform);
        return actor;
    }

    
    public void UpdateGame() {
        if (ai != null) {
            ai.UpdateGame();
        }
    }

    public CharacterBase anim;
    
}
