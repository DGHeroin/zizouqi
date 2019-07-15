using UnityEngine;
using System.Collections;

public class HeroActorDrag : MonoBehaviour {

    public LayerMask layerMask;
    public Transform oldTrans;
    public float moveTime;
    public float height;

    private Ray ray;
    private RaycastHit hit;


    private void OnMouseDown() {
        Debug.Log("OnMouseDown");
        AwakeMove();
        HightOpen();
    }

    private void OnMouseUp() {
        Debug.Log("OnMouseUp");
        IsArrive(Input.mousePosition);
        HightClose();

        var sel = ChessBlockManager.Current.GetSelected();
        if (sel != null) {
            Debug.Log("=========选中了:" + sel.BlockTag);
        } else {
            Debug.Log("没有选中释放位置");
        }
    }

    private void OnMouseDrag() {
        StartMove();
    }

    public void AwakeMove() {
        StopAllCoroutines();
        Debug.Log("开始拖拽");
    }

    void HightOpen() {

    }
    void HightClose() {

    }
    void StartMove() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == this.gameObject) {
                //var origin = this.transform.position;
                //this.transform.position = new Vector3(hit.point.x, origin.y, hit.point.z);
            }
        }
    }
    /// <summary>
    /// 判断是否到达目的地
    /// </summary>
    void IsArrive(Vector3 pos) {
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, 100, layerMask.value)) {
            if (hit.collider.tag != "xxx") {
                ReturnHomePosition();
            }
        } else {
            ReturnHomePosition();
        }
    }

    void ReturnHomePosition() {
        Debug.Log("返回原来位置");

    }
}
