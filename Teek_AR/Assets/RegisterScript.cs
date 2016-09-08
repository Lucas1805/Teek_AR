using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour {
    public UnityEngine.UI.InputField fullnameField;
    public UnityEngine.UI.InputField usernameField;
    public UnityEngine.UI.InputField emailField;
    public UnityEngine.UI.InputField passwordField;
    public UnityEngine.UI.InputField passwordAgainField;
    public Text message;

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
}
