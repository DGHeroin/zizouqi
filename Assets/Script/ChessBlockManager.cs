using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBlockManager : MonoBehaviour {
    public static ChessBlockManager Current;
    Dictionary<string, ChessBlock> blocks = new Dictionary<string, ChessBlock>();
    void Awake() {
        Current = this;
        foreach (Transform trans in transform) {
            var block = trans.GetComponent<ChessBlock>();
            if (block != null) {
                blocks[block.BlockTag] = block;
            }
        }
    }

    public ChessBlock GetSelected() {
        foreach (var item in blocks) {
            if (item.Value.IsSelected) {
                return item.Value;
            }
        }
        return null;
    }
}
