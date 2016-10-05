using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerInformationScript : MonoBehaviour {

    //Use these 2 Key string to store login session in PlayerPrefs
    private readonly string usernameKeyValue = "Username";
    private readonly string passwordKeyValue = "Pass";

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Logout()
    {
        PlayerPrefs.DeleteKey(usernameKeyValue);
        PlayerPrefs.DeleteKey(passwordKeyValue);
        SceneManager.LoadSceneAsync("LoginScene");
    }
}
