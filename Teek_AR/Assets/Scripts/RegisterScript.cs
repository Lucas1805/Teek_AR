using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RegisterScript : MonoBehaviour {
    public UnityEngine.UI.InputField fullnameField;
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField emailField;
    public UnityEngine.UI.InputField passwordField;
    public UnityEngine.UI.InputField passwordAgainField;
    public Text message;

    string fullname;
    string username;
    string email;
    string password;
    string passwordAgain;

    // Use this for initialization
    void Start () {
	
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
            message.text = "Please enter full name";
        }
        else if (email.Length <= 0)
        {
            result = false;
            emailField.Select();
            message.text = "Please enter email";
        }
        else if (email.Length > 0 && !validateEmail(email))
        {
            result = false;
            emailField.Select();
            message.text = "Email format is not valid";

        }
        else if (username.Length <= 0)
        {
            result = false;
            usernameField.Select();
            message.text = "Please enter your username";
        }
        else if (password.Length <= 0)
        {
            result = false;
            passwordField.Select();
            message.text = "Please enter your password";
        }
        else if(!password.Equals(passwordAgain))
        {
            result = false;
            passwordAgainField.Select();
            message.text = "Re-enter Password is not match";
        }
        return result;
    }


}
