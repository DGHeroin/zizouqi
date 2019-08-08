using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBlock : MonoBehaviour {
    public string BlockTag;
    private Material originMaterial;
    public Material selectedMaterial;
    int index = 0;
    MeshRenderer mr = null;
    public bool IsSelected = false;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        originMaterial = mr.materials[0]; // 记录原来的材质
        // StartCoroutine(checkHit());
    }

    Ray ray;
    RaycastHit hit;
    float lastHit = 0;
    IEnumerator checkHit() {
        while (true) {
            bool ok = false;
            var pos = GetInteractivePoint(ref ok);
            if (!ok) { // 是否有点击行为
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            // 是否被UI阻挡
            if (EventSystem.current.IsPointerOverGameObject()) {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            ray = Camera.main.ScreenPointToRay(pos);

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
                    mr.material = originMaterial;
                }
                IsSelected = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    Vector3 GetInteractivePoint(ref bool ok) {
        ok = false;
        switch (Application.platform) {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount == 0) {
                    ok = true;
                    return Input.GetTouch(0).position;
                }
                return Vector3.zero;
            default:
                break;
        }
        ok = true;
        return Input.mousePosition;
    }


    void ChangeMat(int idx) {
        if (index == idx) {
            return;
        }
        index = idx;
        
        if (index == 1) {
            mr.material = selectedMaterial;
        }
        if (index == 0) {
            mr.material = originMaterial;
        }
    }

    private void OnMouseUp() {
        // Debug.Log("松手在:" + BlockTag);
    }
}
