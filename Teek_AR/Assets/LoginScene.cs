using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LoginScene : MonoBehaviour {
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField passwordField;
    public int nextSceneID; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void checkLogin()
    {
        //Get value from input fields
        string username = usernameField.text;
        string password = passwordField.text;

        //Check if Login Infomartion is valid or not
        //REPLACE BELOW CODE WITH LOGIN API 
        //if (usernameField.text == "admin" && passwordField.text == "admin")
        //{
        //    Start a coroutine that will load the desired scene.
        //    StartCoroutine(LoadNextScene());
        //    Debug.Log("Success");
        //    Application.LoadLevel("pokemon");
        //}


        string url = "10.255.250.160:19291/api/account/login/";

        Dictionary<string, string> a = new Dictionary<string, string>();
        POST(url,a);    
        

    }

    public WWW POST(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<String, String> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    IEnumerator LoadNextScene()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = Application.LoadLevelAsync(nextSceneID);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
