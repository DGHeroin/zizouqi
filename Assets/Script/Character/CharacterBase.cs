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
    private HeroActor actor;
    private void Awake() {
        anim = GetComponent<Animator>();
        actor = GetComponent<HeroActor>();
    }

    public void DoNormalAttack(string myId, int attackDamage, Action callback) {
        PerformNormalAttack(AttackTransform, (hit) => {
            var actor = hit.GetComponent<HeroActor>();
            if (actor != null) {
                actor.TakeDamage(myId, attackDamage);
            }
            callback.Invoke();
        });
    }

    public void DoUltimateAttack(string myId, int attackDamage, Action callback) {
        PerformUltimateAttack(AttackTransform, (hit) => {
            var actor = hit.GetComponent<HeroActor>();
            if (actor != null) {
                actor.TakeDamage(myId, attackDamage);
            }
            callback.Invoke();
        });
    }

    /// <summary>
    /// 大招
    /// </summary>
    /// <param name="target"></param>
    /// <param name="onHit"></param>
    public void PerformUltimateAttack(Transform target, Action<GameObject> onHit) {
        //Debug.LogError("开始放大招了");

        if (anim == null) {
            Debug.LogWarning("动画Animator空");
            return;
        }
        anim.SetBool(config.NormalAttack, true);
        if (config.AudioUltimateAttack != null) {
            AudioManager.Instance.PlaySFX(config.AudioUltimateAttack, config.AudioUltimateAttackDelay);
        }

        if (config.NormalAttackIsProject) { // 抛掷体
            var bullet = Instantiate(config.NormalAttackPrefab, this.transform);
            if (bullet == null) {
                Debug.LogWarning("普通攻击 抛掷体 空");
                return;
            }
            bullet.transform.SetParent(null, true);
            bullet.SetActive(false);
            if (config.NormalAttackYOffset > 0) {
                var pos = bullet.transform.position;
                pos.y += config.NormalAttackYOffset;
                bullet.transform.position = pos; // 子弹位置
            }
            var targetPosition = AttackTransform.position; // 打去哪个位置
            targetPosition.y += config.NormalAttackYOffset;

            //Debug.LogFormat("抛掷物:{0} 父节点:{1} 目标点:{2}", obj.transform.position, this.gameObject.transform.position, targetPosition);

            var seq = DOTween.Sequence();
            // 旋转攻击面向
            var r0 = this.transform.DOLookAt(targetPosition, 0.2f);
            seq.Append(r0);

            // 暂停一下, 再创建攻击效果
            seq.AppendInterval(config.NormatlAttackDelay);
            // 启用抛掷物
            seq.AppendCallback(() => {
                bullet.SetActive(true);
            });

            // 播放攻击动画
            float distance = Vector3.Distance(AttackTransform.position, this.transform.position);
            float duration = distance / config.NormatlAttackSpped;
            var att = bullet.transform.DOMove(AttackTransform.position, duration).OnComplete(() => {
                Destroy(bullet);
                onHit.Invoke(target.gameObject);
                if (config.AudioNormalAttackHit != null) {
                    AudioManager.Instance.PlaySFX(config.AudioNormalAttackHit, config.AudioNormalAttackHitDelay);
                }
                if (config.UltimateAttackHitEffect != null) {  // 有大招碰撞效果
                    var hitEff = Instantiate(config.UltimateAttackHitEffect, AttackTransform);
                    if (hitEff == null) {
                        return;
                    }
                    hitEff.transform.SetParent(null, true);
                    hitEff.transform.position = AttackTransform.position;
                    Debug.LogError("有大招碰撞效果" + AttackTransform.position + " => " + hitEff.transform.position);
                    Destroy(hitEff, config.UltimateAttackHitEffectDuration);
                }
            });
            seq.Append(att);
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    /// <param name="target"></param>
    /// <param name="onHit"></param>
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
            var bullet = Instantiate(config.NormalAttackPrefab, this.transform);
            if (bullet == null) {
                Debug.LogWarning("普通攻击 抛掷体 空");
                return;
            }
            bullet.transform.SetParent(null, true);
            bullet.SetActive(false);
            if (config.NormalAttackYOffset > 0) {
                var pos = bullet.transform.position;
                pos.y += config.NormalAttackYOffset;
                bullet.transform.position = pos; // 子弹位置
            }
            var targetPosition = AttackTransform.position; // 打去哪个位置
            targetPosition.y += config.NormalAttackYOffset;

            //Debug.LogFormat("抛掷物:{0} 父节点:{1} 目标点:{2}", obj.transform.position, this.gameObject.transform.position, targetPosition);

            var seq = DOTween.Sequence();
            // 旋转攻击面向
            var r0 = this.transform.DOLookAt(targetPosition, 0.2f);
            seq.Append(r0);

            // 暂停一下, 再创建攻击效果
            seq.AppendInterval(config.NormatlAttackDelay);
            // 启用抛掷物
            seq.AppendCallback(() => {
                bullet.SetActive(true);
            });

            // 播放攻击动画
            float distance = Vector3.Distance(AttackTransform.position, this.transform.position);
            float duration = distance / config.NormatlAttackSpped;
            var att = bullet.transform.DOMove(AttackTransform.position, duration).OnComplete(() => {
                Destroy(bullet);
                onHit.Invoke(target.gameObject);
                if (config.AudioNormalAttackHit != null) {
                    AudioManager.Instance.PlaySFX(config.AudioNormalAttackHit, config.AudioNormalAttackHitDelay);
                }
                if (config.NormatlAttackHitEffect != null) {  // 有碰撞效果
                    var hitEff = Instantiate(config.NormatlAttackHitEffect, AttackTransform);
                    if (hitEff == null) {
                        return;
                    }
                    hitEff.transform.SetParent(null, true);
                    hitEff.transform.position = AttackTransform.position;
                    Debug.Log("有碰撞效果" + AttackTransform.position + " => " + hitEff.transform.position);
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

    public void PerformDie(Action callback) {
        if (anim == null) { return; }
        anim.SetBool(config.DieAnim, true);

        StartCoroutine(EOnDie(callback, 2.5f));
    }

    public IEnumerator EOnDie(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action.Invoke(); // 回调
    }

}
