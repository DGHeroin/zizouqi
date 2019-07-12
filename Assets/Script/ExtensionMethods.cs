using UnityEngine;
using System.Collections;

public static class ExtensionMethods {
    public static void ResetTransformation(this Transform trans) {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void ClearAllChild(this Transform trans) {
        foreach (Transform tr in trans) {
            Object.Destroy(tr.gameObject);
        }
    }

    public static void ClearAllChild(this GameObject[] objs) {
        foreach (var obj in objs) {
            if (obj != null) {
                obj.transform.ClearAllChild();
            }
        }
    }

    public static void ClearAllChild(this Transform[] trans) {
        foreach (var tr in trans) {
            if (tr != null) {
                tr.ClearAllChild();
            }
        }
    }
}