using UnityEngine;
using System.Collections;

public class HeroActorDrag : MonoBehaviour {

    public LayerMask layerMask;
    public Camera cam;
    public Transform oldTrans;
    public float moveTime;
    public float height;

    private Ray ray;
    private RaycastHit hit;


    private void OnMouseDown() {
        AwakeMove();
        HightOpen();
    }

    private void OnMouseUp() {
        IsArrive(Input.mousePosition);
        HightClose();
    }

    private void OnMouseDrag() {
        StartMove();
    }

    public void AwakeMove() {
        StopAllCoroutines();
    }

    void HightOpen() {

    }
    void HightClose() {

    }
    void StartMove() {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, layerMask.value)) {
            if (hit.collider.tag == "xx") {
                Debug.Log("到了");
            } else {
                this.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }
    /// <summary>
    /// 判断是否到达目的地
    /// </summary>
    void IsArrive(Vector3 pos) {
        ray = cam.ScreenPointToRay(pos);
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
