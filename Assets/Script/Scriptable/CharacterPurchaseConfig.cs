using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "AnimationConfig", menuName = "ScriptableObjects/角色购买配置", order = 1)]
#endif
public class CharacterPurchaseConfig : ScriptableObject {
    public string Tag;
    public int Price;
    public GameObject prefab;
}
