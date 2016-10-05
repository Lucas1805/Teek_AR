using UnityEngine;
using System.Collections;
using Assets;

public class TutorialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void loadPreviousScene()
    {
        MySceneManager.loadPreviousScene();
    }
}
