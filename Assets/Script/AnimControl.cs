using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{

    public Animator anim;
    public string playName;
    void Start()
    {
        anim.SetBool("Claw Attack", true);
    }
}
