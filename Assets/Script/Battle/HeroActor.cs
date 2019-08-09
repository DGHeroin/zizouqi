using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActor : MonoBehaviour {

    public string Id { get; private set; }

    public string ChessBoardPosition = null;

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
        // 动画控制
        var anim = obj.AddComponent<CharacterBase>();
        anim.config = cfg;
        actor.anim = anim;
        return actor;
    }

    float lastAttackTime = 0;
    public void UpdateGame() {
        var diff = GameTime.Time - lastAttackTime;
        if (diff > 2) {
            Debug.Log("攻击咯");

            // DEBUG
            anim.AttackTransform = this.transform;// 打自己

            anim.DoNormalAttack();
            lastAttackTime = GameTime.Time;
        }
    }

    private CharacterBase anim;
    
}
