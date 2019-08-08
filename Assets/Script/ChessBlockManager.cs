using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBlockManager : MonoBehaviour {
    public static ChessBlockManager Current;
    public List<GameObject> InHandZone = new List<GameObject>();
    Dictionary<string, GameObject> blocks = new Dictionary<string, GameObject>();
    void Awake() {
        Current = this;
        // 手牌
        foreach(var block in InHandZone) {
            blocks[block.name] = block;
        }
        // 棋盘
        foreach (Transform trans in transform) {
            blocks[trans.name] = trans.gameObject;
            //var block = trans.GetComponent<ChessBlock>();
            //if (block != null) {
            //    blocks[block.BlockTag] = block;
            //}
        }
    }

    public ChessBlock GetSelected() {
        //foreach (var item in blocks) {
        //    if (item.Value.IsSelected) {
        //        return item.Value;
        //    }
        //}
        return null;
    }

    public GameObject GetChessBlock(string key) {
        if (!blocks.ContainsKey(key)) { return null; }
        return blocks[key];
    }
}
