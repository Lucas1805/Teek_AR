using UnityEngine;
using System.Collections;
using System;

public class DragonScript : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

	}

    public void dead()
    {
        anim.SetTrigger("Dead");
    }
}
