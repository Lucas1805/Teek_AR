using UnityEngine;
using System.Collections;
using Assets;

public class TutorialScript : MonoBehaviour {

    public GameObject loadingPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void loadPreviousScene()
    {
        MessageHelper.LoadingDialog("Loading data....");
        MySceneManager.loadPreviousScene();
    }
}
