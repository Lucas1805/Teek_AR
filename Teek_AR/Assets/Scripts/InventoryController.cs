using UnityEngine;
using System.Collections;
using Assets;

public class InventoryController : MonoBehaviour {

    public GameObject loadingPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadPreviousScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        MySceneManager.loadPreviousScene();
    }
}
