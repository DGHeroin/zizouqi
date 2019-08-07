using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PurchaseView : MonoBehaviour {

    public Transform faceTo;
    void Awake() {
		
    }

    void Start() {
        
    }

    void Update() {
        transform.DOLookAt(faceTo.position, 0f, AxisConstraint.None, transform.up);
    }
}
