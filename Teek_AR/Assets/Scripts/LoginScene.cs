using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;
using Assets;
using Assets.ResponseModels;
using System.Text.RegularExpressions;

public class LoginScene : MonoBehaviour {

    //VARIABLE FOR LOGIN
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField passwordField;
    public Text loginMessage;
    public GameObject loginMessagePanel;
    public GameObject loginPanel;
    public GameObject loadingPanel;


    private string username = "";
    private string password = "";

    //VARIABLE FOR REGISTER
    public UnityEngine.UI.InputField rg_fullnameField;
    public UnityEngine.UI.InputField rg_usernameField;
    public UnityEngine.UI.InputField rg_emailField;
    public UnityEngine.UI.InputField rg_passwordField;
    public UnityEngine.UI.InputField rg_passwordAgainField;
    public GameObject registerPanel;
    public GameObject registerMessagePanel;
    public Text registerMessage;

    string rg_fullname;
    string rg_username;
    string rg_email;
    string rg_password;
    string rg_passwordAgain;

    // Use this for initialization
    void Start()
    {
        //Check if Player has Login Info (Login Session) in PlayerPrefs. If YES auto login
        if (PlayerPrefs.HasKey(ConstantClass.PP_UsernameKey) && PlayerPrefs.HasKey(ConstantClass.PP_PasswordKey))
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
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Reset message text
        loginMessage.text = "";

        //Get value from input fields
        username = usernameField.text;
        password = passwordField.text;

        //Check if player enter any iformation. If not do nothing
        if (username.Length > 0 && password.Length > 0)
        {
            //Check if Login Infomartion is valid or not

            //BACKDOOR CODE
            if (usernameField.text == "adminThongNe" && passwordField.text == "adminThong123")
            {
                //Start a coroutine that will load the desired scene.
                SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
            }

            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("Username", username);
            form.AddField("Password", password);

            //SEND POST REQUEST

            WWW www = new WWW(ConstantClass.API_Login, form);

            StartCoroutine(WaitForLoginRequest(www));
        }
        else
        {
            showLoginMessage("Please enter username and password");
        }
    }

    private void checkLoginWithSession()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Get username and pass from PlayerPrefs and decrypted it
        username = Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UsernameKey)));
        password = Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_PasswordKey)));

        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("Username", username);
        form.AddField("Password", password);

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_Login, form);

        StartCoroutine(WaitForLoginRequest(www));
    }

    public void doRegister()
    {
        //Enable loading indicator
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Reset message
        registerMessage.text = "";

        //Get values
        rg_fullname = rg_fullnameField.text;
        rg_email = rg_emailField.text;
        rg_username = rg_usernameField.text;
        rg_password = rg_passwordField.text;
        rg_passwordAgain = rg_passwordAgainField.text;

        if (checkRequireInfo())
        {
            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("Username", rg_username);
            form.AddField("Fullname", rg_fullname);
            form.AddField("Email", rg_email);
            form.AddField("Password", rg_password);

            //SEND POST REQUEST

            WWW www = new WWW(ConstantClass.API_Register, form);

            StartCoroutine(WaitForRegisterRequest(www));
        }
    }

    IEnumerator WaitForLoginRequest(WWW www)
    {
        yield return www;

        if(www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<LoginModel> jsonResponse = new ResponseModel<LoginModel>();
                jsonResponse.Data = new LoginModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<LoginModel>>(www.text);

                if (jsonResponse.Succeed)
                {
                    //Store login info for auto login and store player id to get player detail info later
                    PlayerPrefs.SetString(ConstantClass.PP_UsernameKey, Encrypt.EncryptString(username));
                    PlayerPrefs.SetString(ConstantClass.PP_PasswordKey, Encrypt.EncryptString(password));
                    PlayerPrefs.SetString(ConstantClass.PP_UserIDKey, Encrypt.EncryptString(jsonResponse.Data.Id));
                    PlayerPrefs.Save();

                    //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
                    MySceneManager.setLastScene(ConstantClass.LoginSceneName);

                    //Load home scene
                    SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
                }
                else
                {
                    //Delete autologin info when login failed
                    PlayerPrefs.DeleteKey(ConstantClass.PP_UsernameKey);
                    PlayerPrefs.DeleteKey(ConstantClass.PP_PasswordKey);
                    PlayerPrefs.DeleteKey(ConstantClass.PP_UserIDKey);

                    //Show error message
                    showLoginMessage(jsonResponse.Message);
                    resetLoginField();
                }
            }
            else {
                showLoginMessage(www.error);
                resetLoginField();
                Debug.Log("WWW Error: " + www.error);
            }
        }
        
    }

    IEnumerator WaitForRegisterRequest(WWW www)
    {
        yield return www;

        //Check for errors
        if (www.error == null)
        {
            ResponseModel<RegisterModel> jsonResponse = new ResponseModel<RegisterModel>();
            jsonResponse.Data = new RegisterModel();
            jsonResponse = JsonMapper.ToObject<ResponseModel<RegisterModel>>(www.text);

            if (jsonResponse.Succeed)
            {
                registerPanel.SetActive(false);
                loginPanel.SetActive(true);

                //Set variable and do login
                usernameField.text = rg_username;
                passwordField.text = rg_password;
                checkLogin();
            }
            else
            {
                //Show error message
                showRegisterMessage(jsonResponse.Message);
            }
        }
        else {
            showRegisterMessage(www.error);
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void resetLoginField()
    {
        usernameField.text = "";
        passwordField.text = "";
    }

    public void resetRegisterField()
    {
        rg_fullnameField.text = "";
        rg_usernameField.text = "";
        rg_emailField.text = "";
        rg_passwordField.text = "";
        rg_passwordAgainField.text = "";
    }

    public void showLoginMessage(string messageString)
    {
        loginPanel.SetActive(false);
        loginMessage.text = messageString;
        loginMessagePanel.SetActive(true);
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    public void showRegisterMessage(string messageString)
    {
        registerPanel.SetActive(false);
        registerMessagePanel.SetActive(true);
        registerMessage.text = messageString;
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    private bool validateEmail(string email)
    {
        var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(email);
    }

    private bool checkRequireInfo()
    {
        bool result = true;
        if (rg_fullname.Length <= 0)
        {
            result = false;
            showRegisterMessage("Please enter fullname");
            resetPasswordFields();
        }
        else if (rg_email.Length <= 0)
        {
            result = false;
            showRegisterMessage("Please enter email");
            resetPasswordFields();
        }
        else if (rg_email.Length > 0 && !validateEmail(rg_email))
        {
            result = false;
            showRegisterMessage("Email format is not valid");
            resetPasswordFields();

        }
        else if (rg_username.Length <= 0)
        {
            result = false;
            showRegisterMessage("Please enter your username");
            resetPasswordFields();
        }
        else if (rg_password.Length <= 0)
        {
            result = false;
            showRegisterMessage("Please enter your password");
            resetPasswordFields();
        }
        else if (!rg_password.Equals(rg_passwordAgain))
        {
            result = false;
            showRegisterMessage("Re-enter Password is not match");
            resetPasswordFields();
        }
        return result;
    }


    public void resetPasswordFields()
    {
        rg_passwordField.text = "";
        rg_passwordAgainField.text = "";
    }
}
