using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChessBoardGenerator : MonoBehaviour {
    int width = 8;
    int height = 8;
    [Tooltip("图块")]
    public GameObject prefab;
    [Tooltip("块信息")]
    public Rect rect = Rect.zero;

    /// <summary>
    /// 
    /// </summary>
    public void GenerateMap() {
        this.ClearMap();
        // 生成地图
        int[,] map = ChessMapFuncs.GenerateArray(width, height);
        // 渲染地图
        ChessMapFuncs.RenderMap(map, prefab, this.transform, rect);
    }
    public void ClearMap() {
        // 清空节点
        for (int i = this.transform.childCount - 1; i >= 0; i--) {
            var tr = this.transform.GetChild(i);
            Object.DestroyImmediate(tr.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChessBoardGenerator))]
public class LevelGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ChessBoardGenerator gen = (ChessBoardGenerator)target;
        if (gen != null) {
            Editor mapSettingEditor = CreateEditor(gen);
            if (mapSettingEditor == null) {
                return;
            }

            if (GUILayout.Button("生成")) {
                gen.GenerateMap();
            }

            if (GUILayout.Button("清除")) {
                gen.ClearMap();
            }
        }
    }
}
#endif