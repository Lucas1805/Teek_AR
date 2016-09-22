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
    public GameObject messagePanel;
    public GameObject loginPanel;
    public int nextSceneID;

    private string url = "http://localhost/Teek/api/account/login";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            //if (usernameField.text == "admin" && passwordField.text == "admin")
            //{
            //    //Start a coroutine that will load the desired scene.
            //    SceneManager.LoadSceneAsync(nextSceneID);
            //}

            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("Username", username);
            form.AddField("Password", password);

            //SEND POST REQUEST

            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
        }
        else
        {
            //message.text = "Please enter username and password";
            showMessage("Please enter username and password");
        }
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

         //Check for errors
        if (www.error == null)
        {
            JSONResponseObject jsonResponse = new JSONResponseObject();


            jsonResponse = JsonMapper.ToObject<JSONResponseObject>(www.text);

            if (jsonResponse.Succeed)
            {
                //Load next scene
                SceneManager.LoadSceneAsync(nextSceneID);
            }
            else
            {
                //Show error message
                showMessage(jsonResponse.Message);
            }
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    [System.Serializable]
    public class JSONResponseObject
    {
            
        public bool Succeed { get; set; }

        public string Message { get; set; }

        public string Errors { get; set; }
        public Data Data { get; set; }

    }

    public class Data
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
    }
    public void resetField()
    {
        usernameField.text = "";
        passwordField.text = "";
    }

    public void showMessage(string messageString)
    {
        loginPanel.SetActive(false);
        message.text = messageString;
        messagePanel.SetActive(true);
    }
}
