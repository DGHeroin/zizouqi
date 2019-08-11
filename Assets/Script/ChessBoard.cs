using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour {
    [SerializeField]
    private Camera mainCamera = null;
    private GameObject selectObject = null;

    private Camera GetCamera() {
        if (mainCamera != null) {
            return mainCamera;
        }
        return Camera.main;
    }

    private Vector3 GetInputPosition() {
#if UNITY_EDITOR
        return Input.mousePosition;
#else
        //TODO Impl input detect
        实现平台相关代码
#endif
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CheckRaycast();
        }
        if (Input.GetMouseButtonUp(0)) { // 松开鼠标 取消选中
            ClearSelection();
        }
    }

    void CheckRaycast() {
        Ray ray = GetCamera().ScreenPointToRay(GetInputPosition());
        RaycastHit hitInfo;
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (Physics.Raycast(ray, out hitInfo)) {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject.tag == "ChessBlock") {
                ClearSelection(); // 清除前面的状态
                SelectObject(hitObject);
                return;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    void SelectObject(GameObject obj) {
        if (selectObject != null) {
            if (obj == selectObject) {
                return;
            }
            ClearSelection(); // clear prev select object
        }
        selectObject = obj;
        // Debug.Log("选中:" + selectObject.name);
        // 1.判断这个棋位是否有角色
        var actor = BattleField.Current.GetMap(selectObject.name);
        if (actor == null) { // 没人在这里, 取消选中效果
            ClearSelection();
            return;
        }

        // 选中棋块
        changeColor(Color.green);
    }

    private void changeColor(Color c) {
        Renderer[] rs = selectObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs) {
            Material m = r.material;
            m.color = c;
            r.material = m;
        }
    }

    void ClearSelection() {
        if (selectObject == null) {
            return;
        }
        // 1. 清除原有选中的状态
        changeColor(Color.white);

        // 2. 检查松开的位置
        do {
            if (Input.GetMouseButtonUp(0)) {
                Ray ray = GetCamera().ScreenPointToRay(GetInputPosition());
                RaycastHit hitInfo;
                RaycastHit[] hits = Physics.RaycastAll(ray);
                if (Physics.Raycast(ray, out hitInfo)) {
                    GameObject hitObject = hitInfo.transform.gameObject;
                    if (hitObject.tag == "ChessBlock") {
                        // 如果松开的位置, 也是 chess block
                        var srcActor = BattleField.Current.GetMap(selectObject.name);
                        var dstActor = BattleField.Current.GetMap(hitObject.name);
                        var dstPos = hitObject.name;
                        Debug.Log("松开:" + selectObject.name + " => " + hitObject.name + " " + srcActor.Id);
                        var bf = BattleField.Current;
                        if (bf.GameState() != BattleState.Prepare && dstPos.StartsWith("B")) { // 如果是在战斗中, 则不能再增加到棋盘中了
                            Debug.LogWarning("不在准备中, 无法操作到棋盘");
                            break;
                        }
                        if (dstActor != null) {
                            // 这个位置有人了
                            break;
                        }
                        // 可以移动过去                        
                        BattleField.Current.MoveCharacter(srcActor, hitObject.name);
                        break;
                    }
                }
            }
        } while (false);

        selectObject = null;
    }
}
