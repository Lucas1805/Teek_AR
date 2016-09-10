using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;

public class LoginScene : MonoBehaviour {
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField passwordField;
    public Text message;
    public int nextSceneID;

    private string url = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    public void checkLogin()
    {
        message.text = "";
        //Get value from input fields
        string username = usernameField.text;
        string password = passwordField.text;

        //Check if player enter any iformation. If not do nothing
        if (username.Length > 0 && password.Length > 0)
        {
            //Check if Login Infomartion is valid or not
            //REPLACE BELOW CODE WITH LOGIN API 
            if (usernameField.text == "admin" && passwordField.text == "admin")
            {
                //Start a coroutine that will load the desired scene.
                SceneManager.LoadSceneAsync(nextSceneID);
            }

            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("Username", username);
            form.AddField("Password", password);

            //SEND POST REQUEST
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            www.Send();

            //Check result
            if (!www.isError) //If send request sucess
            {
                BuuCLass person = new BuuCLass();


                person = JsonMapper.ToObject<BuuCLass>(www.downloadHandler.text);
                Debug.Log(person.Succeed + " " + person.Message);

            }
        }
        else
        {
            message.text = "Please enter username and password";
        }
    }
    [System.Serializable]
    public class BuuCLass
    {

        public bool Succeed { get; set; }

        public string Message { get; set; }

        public string Errors { get; set; }
        public string Data { get; set; }
       
    }
    public void resetField()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
    
}
