using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHandChessManager : MonoBehaviour {
    [Header("手牌位置")]
    public GameObject[] Slots;

    void Awake() {
        Slots.ClearAllChild();
    }

    void Start() {

    }

    void Update() {

    }

    public void AddChess(GameObject obj) {
        foreach (var slot in Slots) {
            if (slot.transform.childCount == 0) {
                obj.transform.SetParent(slot.transform, false);
                var actor = obj.GetComponent<HeroActor>();
                break;
            }
        }
    }

    public int Count() {
        var n = 0;
        foreach (var slot in Slots) {
            if (slot.transform.childCount > 0) {
                n++;
            }
        }
        return n;
    }
}
