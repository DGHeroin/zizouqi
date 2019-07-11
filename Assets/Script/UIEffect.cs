using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIEffect : MonoBehaviour {
    public Transform target;

    public void PopWindow() {
        Vector3 max = Vector3.one;
        float time = .3f;
        target.localScale = Vector3.zero;
        target.DOScale(max, time);
    }

    public void HideWindow() {

    }
}
