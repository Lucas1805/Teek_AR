using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using Assets;

public class RedeemScript : MonoBehaviour {

    public InputField inputCodeField;
    public GameObject redeemCodePanel;
    public GameObject messagePanel;
    public GameObject loadingPanel;


    public Text message;

    string code;

    private string url = "http://localhost/Teek/api/account/login";
    private SpriteRenderer sp;

    // Use this for initialization
    void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
        if(sp == null)
        {
            Debug.Log("Cannot get loading sprite object");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void redeemCode()
    {
        message.text = "";

        //Get value
        code = inputCodeField.text;

        if(code.Length > 0)
        {
            showLoadingIndicator();

            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("RedeemCode", code);

            //SEND POST REQUEST

            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
        }
        else
        {
            showMessage("Please enter code");
        }
    }

    public void resetField()
    {
        inputCodeField.text = "";
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
                //If code is correct

                disableLoadinIndicator();
            }
            else
            {
                disableLoadinIndicator();
                showMessage(jsonResponse.Message);
            }
        }
        else {
            showMessage(www.error);
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

    public void showMessage(string messageString)
    {
        redeemCodePanel.SetActive(false);
        messagePanel.SetActive(true);
        message.text = messageString;
        disableLoadinIndicator();
        resetField();

    }

    public void showLoadingIndicator()
    {
        redeemCodePanel.SetActive(false);
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

    public void loadLastScene()
    {
        MySceneManager.loadPreviousScene();
    }
}
