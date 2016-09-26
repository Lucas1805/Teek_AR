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
    public GameObject loadingPanel;
    public int nextSceneID;

    //Use these 2 Key string to store login session in PlayerPrefs
    private readonly string usernameKeyValue = "Username";
    private readonly string passwordKeyValue = "Pass";

    private string username = "";
    private string password = "";


    private string url = "http://localhost/Teek/api/account/login";
    private SpriteRenderer sp;

    // Use this for initialization
    void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp != null)
        {
            sp.enabled = false;
        }
        else Debug.Log("Cannot get Sprite Renderer Object in Start function");

        //Check if Player has Login Info (Login Session) in PlayerPrefs. If YES auto login
        if (PlayerPrefs.HasKey(usernameKeyValue) && PlayerPrefs.HasKey(usernameKeyValue))
        {
            this.checkLoginWithSession();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void checkLogin()
    {
        //Enable loading indicator
        showLoadingIndicator();

        //Reset message text
        message.text = "";

        //Get value from input fields
        username = usernameField.text;
        password = passwordField.text;

        //Check if player enter any iformation. If not do nothing
        if (username.Length > 0 && password.Length > 0)
        {
            //Check if Login Infomartion is valid or not

            //BACKDOOR CODE
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

    private void checkLoginWithSession()
    {
        //Get username and pass from PlayerPrefs and decrypted it
        username = Decrypt.DecryptString((PlayerPrefs.GetString(usernameKeyValue)));
        password = Decrypt.DecryptString((PlayerPrefs.GetString(passwordKeyValue)));

        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("Username", username);
        form.AddField("Password", password);

        //SEND POST REQUEST

        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
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
                //Store login info for auto login
                PlayerPrefs.SetString(usernameKeyValue, Encrypt.EncryptString(username));
                PlayerPrefs.SetString(passwordKeyValue, Encrypt.EncryptString(password));
                PlayerPrefs.Save();

                //Load next scene
                disableLoadinIndicator();
                SceneManager.LoadSceneAsync(nextSceneID);
            }
            else
            {
                //Show error message
                showMessage(jsonResponse.Message);
                disableLoadinIndicator();
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
        disableLoadinIndicator();
    }
    
    public void showLoadingIndicator()
    {
        if (sp != null && loadingPanel != null)
        {
            sp.enabled = true;
            loadingPanel.SetActive(true);
        }
        else
            Debug.Log("Null Sprite Renderer of Loading Panel");
    }

    public void disableLoadinIndicator()
    {
        if (sp != null && loadingPanel != null)
        {
            if (sp.enabled == true)
                sp.enabled = false;
            loadingPanel.SetActive(false);
        }
        else
            Debug.Log("Null Sprite Renderer of Loading Panel");
    }

    public void testAutoLogin()
    {
        Debug.Log(Encrypt.EncryptString(usernameField.text));
        PlayerPrefs.SetString("TESTAUTO", Encrypt.EncryptString(usernameField.text));
        PlayerPrefs.Save();

        showMessage(Decrypt.DecryptString(PlayerPrefs.GetString("TESTAUTO")));
    }
}
