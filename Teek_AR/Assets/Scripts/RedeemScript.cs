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

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void redeemCode()
    {
        message.text = "";

        MessageHelper.LoadingDialog("Loading data....");

        //Get value
        code = inputCodeField.text;

        if(code.Length > 0)
        {
            //Show loading indicator
            MessageHelper.LoadingDialog("Loading data....");

            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            form.AddField("RedeemCode", code);

            //SEND POST REQUEST

            WWW www = new WWW(ConstantClass.API_RedeemCode, form);

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

                MessageHelper.CloseDialog();
            }
            else
            {
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
        MessageHelper.CloseDialog();
        resetField();

    }

    public void loadLastScene()
    {
        MessageHelper.LoadingDialog("Loading data....");
        MySceneManager.loadPreviousScene();
    }
}
