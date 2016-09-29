using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSceneHomeButton : MonoBehaviour {

    public int HomeSceneID;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadHomeScene()
    {
        SceneManager.LoadSceneAsync(HomeSceneID);
    }
}
