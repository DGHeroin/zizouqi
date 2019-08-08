using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {
    [SerializeField]
    private Camera mainCamera = null;
    private GameObject selectObject = null;
    private Camera GetCamera() {
        if (mainCamera != null) {
            return mainCamera;
        }
        return Camera.main;
    }

    void Update() {
#if UNITY_EDITOR
        CheckRaycast(Input.mousePosition);
#else
        //TODO Impl input detect
        实现平台相关代码
#endif
    }

    void CheckRaycast(Vector3 positon) {
        Ray ray = GetCamera().ScreenPointToRay(positon);
        RaycastHit hitInfo;
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (Physics.Raycast(ray, out hitInfo)) {
            GameObject hitObject = hitInfo.transform.root.gameObject;

            if (hitObject.tag != "character") {
                return;
            }
            SelectObject(hitObject);
        } else {
            ClearSelection();
        }
    }

    void SelectObject(GameObject obj) {
        if (selectObject != null) {
            if (obj == selectObject) {
                return;
            }
            ClearSelection(); // clear prev select object
        }
        selectObject = obj;

        Renderer[] rs = selectObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs) {
            Material m = r.material;
            m.color = Color.green;
            r.material = m;
        }
    }
    void ClearSelection() {
        if (selectObject == null) {
            return;
        }
        Renderer[] rs = selectObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs) {
            Material m = r.material;
            m.color = Color.white;
            r.material = m;
        }

        selectObject = null;
    }
}
