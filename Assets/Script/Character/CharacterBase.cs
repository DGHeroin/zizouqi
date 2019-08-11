using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// 角色动画控制
/// </summary>
public class CharacterBase : MonoBehaviour {

    public CharacterConfig config;
    private Animator anim;
    [Tooltip("攻击对象")]
    public Transform AttackTransform;
    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void DoNormalAttack() {
        PerformNormalAttack(AttackTransform, (hit) => {
            Debug.Log("打中扣血");
            var cb = hit.GetComponent<CharacterBase>();
            if (cb == null) { return; }
            cb.PerformTakeDamage();
        });
    }

    public void PerformNormalAttack(Transform target, Action<GameObject> onHit) {
        if (anim == null) {
            Debug.LogWarning("动画Animator空");
            return;
        }
        anim.SetBool(config.NormalAttack, true);
        if (config.AudioNormalAttack != null) {
            AudioManager.Instance.PlaySFX(config.AudioNormalAttack, config.AudioNormalAttackDelay);
        }

        if (config.NormalAttackIsProject) { // 抛掷体
            var obj = Instantiate(config.NormalAttackPrefab, this.transform);
            if (obj == null) {
                Debug.LogWarning("普通攻击 抛掷体 空");
                return;
            }
            obj.SetActive(false);
            if (config.NormalAttackYOffset > 0) {
                var pos = obj.transform.position;
                pos.y = config.NormalAttackYOffset;
                obj.transform.position = pos;
            }
            var seq = DOTween.Sequence();
            // 旋转攻击面向
            var r0 = this.transform.DOLookAt(AttackTransform.position, 0.2f);
            seq.Append(r0);

            // 暂停一下, 再创建攻击效果
            seq.AppendInterval(config.NormatlAttackDelay);
            // 启用抛掷物
            seq.AppendCallback(() => {
                obj.SetActive(true);
            });

            // 播放攻击动画
            float distance = Vector3.Distance(AttackTransform.position, this.transform.position);
            float duration = distance * config.NormatlAttackSpped;
            var att = obj.transform.DOMove(AttackTransform.position, duration).OnComplete(() => {
                Destroy(obj);
                onHit.Invoke(target.gameObject);
                if (config.AudioNormalAttackHit != null) {
                    AudioManager.Instance.PlaySFX(config.AudioNormalAttackHit, config.AudioNormalAttackHitDelay);
                }
                if (config.NormatlAttackHitEffect != null) {  // 有碰撞效果
                    var hitEff = Instantiate(config.NormatlAttackHitEffect, AttackTransform);
                    if (hitEff == null) {
                        return;
                    }
                    Destroy(hitEff, config.NormatlAttackHitEffectDuration);
                }
            });
            seq.Append(att);
        } else {
            // 直接攻击
            var seq = DOTween.Sequence();
            // 旋转攻击面向
            var r0 = this.transform.DOLookAt(AttackTransform.position, 0.2f);
            seq.Append(r0);

            // 暂停一下, 再创建攻击效果
            seq.AppendInterval(config.NormatlAttackDelay);
            seq.OnComplete(()=> {
                onHit.Invoke(target.gameObject);
                if (config.AudioNormalAttackHit != null) {
                    AudioManager.Instance.PlaySFX(config.AudioNormalAttackHit, config.AudioNormalAttackHitDelay);
                }
            });
            // 
            if (config.NormatlAttackHitEffect != null) {  // 有碰撞效果
                var hitEff = Instantiate(config.NormatlAttackHitEffect, AttackTransform);
                if (hitEff == null) {
                    return;
                }
                Destroy(hitEff, config.NormatlAttackHitEffectDuration);
            }
        }
    }
    public void PerformTakeDamage() {
        if (anim == null) { return; }
        anim.SetBool(config.TakeDamage, true);

        if (config.AudioTakeDamage != null) {
            AudioManager.Instance.PlaySFX(config.AudioTakeDamage);
        }
    }
}
