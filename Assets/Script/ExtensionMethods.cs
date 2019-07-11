using UnityEngine;
using System.Collections;

public static class ExtensionMethods {
    public static void ResetTransformation(this Transform trans) {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void ClearAllChild(this Transform trans) {
        foreach(Transform tr in trans) {
            GameObject.Destroy(tr.gameObject);
        }
    }
}