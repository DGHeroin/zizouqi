using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "AnimationConfig", menuName = "ScriptableObjects/关卡配置", order = 1)]
#endif
public class LevelConfig : ScriptableObject {
    public string Id;
    public LevelHeroConfig[] Heroes;
}
[System.Serializable]
public class LevelHeroConfig {
    public string Position;
    public string CharacterTag;
}
