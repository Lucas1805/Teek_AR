using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using LitJson;

public class RegisterScript : MonoBehaviour {
    public UnityEngine.UI.InputField fullnameField;
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField emailField;
    public UnityEngine.UI.InputField passwordField;
    public UnityEngine.UI.InputField passwordAgainField;
    public Text message;
    public GameObject messagePanel;
    public GameObject registerPanel;
    public GameObject loadingPanel;

    private string url = "http://localhost/Teek/api/account/register";
    private SpriteRenderer sp;

    string fullname;
    string username;
    string email;
    string password;
    string passwordAgain;

    // Use this for initialization
    void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void resetField()
    {
        fullnameField.text = "";
        usernameField.text = "";
        emailField.text = "";
        passwordField.text = "";
        passwordAgainField.text = "";
        message.text = "";
    }

    public void doRegister()
    {
        //Enable loading indicator
        showLoadingIndicator();

        //Reset message
        message.text = "";
        
        //Get values
        fullname = fullnameField.text;
        email = emailField.text;
        username = usernameField.text;
        password = passwordField.text;
        passwordAgain = passwordAgainField.text;

        if(checkRequireInfo())
        {
            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("Username", username);
            form.AddField("Fullname", fullname);
            form.AddField("Email", email);
            form.AddField("Password", password);

            //SEND POST REQUEST

            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
        }
    }

    private bool validateEmail(string email)
    {
        var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(email);
    }

    private bool checkRequireInfo()
    {
        bool result = true;
        if (fullname.Length <= 0)
        {
            result = false;
            fullnameField.Select();
            showMessage("Please enter fullname");
            resetPasswordFields();
        }
        else if (email.Length <= 0)
        {
            result = false;
            emailField.Select();
            showMessage("Please enter email");
            resetPasswordFields();
        }
        else if (email.Length > 0 && !validateEmail(email))
        {
            result = false;
            emailField.Select();
            showMessage("Email format is not valid");
            resetPasswordFields();

        }
        else if (username.Length <= 0)
        {
            result = false;
            usernameField.Select();
            showMessage("Please enter your username");
            resetPasswordFields();
        }
        else if (password.Length <= 0)
        {
            result = false;
            passwordField.Select();
            showMessage("Please enter your password");
            resetPasswordFields();
        }
        else if(!password.Equals(passwordAgain))
        {
            result = false;
            passwordAgainField.Select();
            showMessage("Re-enter Password is not match");
            resetPasswordFields();
        }
        return result;
    }

    public void showMessage(string messageString)
    {
        registerPanel.SetActive(false);
        messagePanel.SetActive(true);
        message.text = messageString;
        disableLoadinIndicator();
    }

    public void resetPasswordFields()
    {
        passwordField.text = "";
        passwordAgainField.text = "";
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

                disableLoadinIndicator();
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

    public void showLoadingIndicator()
    {
        registerPanel.SetActive(false);
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
        registerPanel.SetActive(true);
        if (sp != null && loadingPanel != null)
        {
            if (sp.enabled == true)
                sp.enabled = false;
            loadingPanel.SetActive(false);
        }
        else
            Debug.Log("Null Sprite Renderer of Loading Panel");
    }
}
