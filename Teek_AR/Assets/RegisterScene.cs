using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class RegisterScene : MonoBehaviour {
    public UnityEngine.UI.InputField fullnameField;
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField passwordField;
    public UnityEngine.UI.InputField passwordAgainField;
    public UnityEngine.UI.InputField emailField;
    public UnityEngine.UI.Text messageField;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void resetFields()
    {
        fullnameField.text = "";
        usernameField.text = "";
        emailField.text = "";
        passwordField.text = "";
        passwordAgainField.text = "";
        messageField.text = "";
    }

    public void doSignup()
    {
        //Get value
        string fullname = fullnameField.text;
        string username = usernameField.text;
        string email = emailField.text;
        string password = passwordField.text;
        string passwordAgain = passwordAgainField.text;

        //Check valid for retype password
        if(password.Equals(passwordAgain))
        {
            //if(InputField.CharacterValidation.EmailAddress)
            //ENTER REGISTER API HERE
            Debug.Log(fullname);
            Debug.Log(username);
            Debug.Log(email);
            Debug.Log(password);
            Debug.Log(passwordAgain);

            //IF SUCCESS

            //IF FAIL, SHOW MESSAGE
        }
        else
        {
            //Show Message
            //EditorUtility.DisplayDialog("Error", "Retype password is not match with password", "OK", "");
            messageField.text = "Error";
        }
    }
}
