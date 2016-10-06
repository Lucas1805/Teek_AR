using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets;
using UnityEngine.UI;
using Assets.ResponseModels;
using LitJson;

public class PlayerInformationScript : MonoBehaviour {

    private string playerID;

    public Image profileImage;
    public Text usernameText;
    public Text fullnameText;
    public Text emailText;
    public Text phoneText;
    public GameObject loadingPanel;

    // Use this for initialization
    void Start () {
        //Get player id from PlayerPrefs and decrypted it
        playerID = Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        loadPlayerInfo();
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Logout()
    {
        PlayerPrefs.DeleteKey(ConstantClass.PP_UsernameKey);
        PlayerPrefs.DeleteKey(ConstantClass.PP_PasswordKey);
        PlayerPrefs.DeleteKey(ConstantClass.PP_UserIDKey);
        SceneManager.LoadSceneAsync(ConstantClass.LoginSceneName);
    }

    public void loadPreviousScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        MySceneManager.loadPreviousScene();
    }

    private void loadPlayerInfo()
    {
        //Show loading indicator
        LoadingManager.showLoadingIndicator(loadingPanel);

        if(playerID != null && playerID.Length >0)
        {
            //Create object to sen Http Request
            WWWForm form = new WWWForm();
            string url = ConstantClass.API_UserInfo + playerID;

            //SEND POST REQUEST
            WWW www = new WWW(url);

            StartCoroutine(WaitForRequest(www));
        }
        else
        {
            Debug.Log("Cannot player ID in PlayerPrefs");
        }
        
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<UserInfoModel> jsonResponse = new ResponseModel<UserInfoModel>();
                jsonResponse.Data = new UserInfoModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<UserInfoModel>>(www.text);

                if (jsonResponse.Succeed)
                {
                    //Load Username
                    usernameText.text = jsonResponse.Data.Username;

                    //Load Fullname
                    fullnameText.text = jsonResponse.Data.FullName;

                    //Load Email
                    emailText.text = jsonResponse.Data.Email;

                    //Load Birthdate
                    //birthdateText.text = jsonResponse.Data.;

                    //Load Phone
                    phoneText.text = jsonResponse.Data.PhoneNumber;

                    //Load Profile Image
                    WWW www_loadImage = new WWW(jsonResponse.Data.ImageUrl);
                    StartCoroutine(loadProfileImage(www_loadImage));

                    LoadingManager.hideLoadingIndicator(loadingPanel);
                }
            }
            else {
                //showMessage(www.error);
                LoadingManager.hideLoadingIndicator(loadingPanel);
                Debug.Log("WWW Error: " + www.error);
            }
        }
    }
    
    IEnumerator loadProfileImage(WWW www)
    {
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width * 130/100, www.texture.height * 130 / 100), new Vector2(0, 0));
    }

}
