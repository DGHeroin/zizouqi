using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBlock : MonoBehaviour {
    public string BlockTag;
    public Material[] Materials;
    int index = 0;
    MeshRenderer mr = null;
    public bool IsSelected = false;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        StartCoroutine(checkHit());
    }

    Ray ray;
    RaycastHit hit;
    float lastHit = 0;
    IEnumerator checkHit() {
        while (true) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "ChessBlock" &&
                    hit.transform.gameObject == this.gameObject) {
                    lastHit = Time.time;
                    IsSelected = true;
                    if (index == 0) {
                        ChangeMat(1);
                        yield return new WaitForSeconds(0.1f);
                        continue;
                    }
                }
            }
            // 离开一段时间后在检测取消
            if (Time.time - lastHit > 0.1f) {
                if (index != 0) {
                    ChangeMat(0);
                    mr.material = Materials[index];
                }
                IsSelected = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void ChangeMat(int idx) {
        if (index == idx) {
            return;
        }
        index = idx;
        mr.material = Materials[idx];
    }

    private void OnMouseUp() {
        Debug.Log("松手在:" + BlockTag);
    }
}
