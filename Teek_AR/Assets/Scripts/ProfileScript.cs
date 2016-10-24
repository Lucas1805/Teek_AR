using UnityEngine;
using System.Collections;
using Assets;
using Ucss;
using Assets.ResponseModels;
using LitJson;
using UnityEngine.UI;

public class ProfileScript : MonoBehaviour {

    public Text CoinText;
    public Text BrandCountText;
    public Text EventCountText;
    public Text UsernameText;
    public Text FullnameText;
    public Text EmailText;
    public Text PhoneText;
    public Image ProfileImage;

    // Use this for initialization
    void Start () {
        LoadUserInformation();
	}

    public void Refresh()
    {
        LoadUserInformation();
    }

    public void LoadUserInformation()
    {
        MessageHelper.LoadingDialog("Loading data....");
        string UserID = Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey));

        if(UserID !=null)
        {
            //CREATE GET REQUEST AND SEND
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadUserProfile + "?userId=" + UserID;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadUserInformationRequest);
            request.onError = new EventHandlerServiceError(MessageHelper.OnError);
            request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
            UCSS.HTTP.GetString(request);
        }
    }

    #region PROCESS LOAD USER PROFILE REQUEST
    public void OnDoneCallLoadUserInformationRequest(string result, string transactionId)
    {
        ResponseModel<UserInfoModel> jsonResponse = new ResponseModel<UserInfoModel>();
        jsonResponse.Data = new UserInfoModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<UserInfoModel>>(result);

        if (jsonResponse.Succeed)
        {
            CoinText.text = jsonResponse.Data.Coin.ToString();
            BrandCountText.text = jsonResponse.Data.BrandCount.ToString();
            EventCountText.text = jsonResponse.Data.EventCount.ToString();
            UsernameText.text = jsonResponse.Data.Username;
            FullnameText.text = jsonResponse.Data.Fullname;
            EmailText.text = jsonResponse.Data.Email;
            PhoneText.text = jsonResponse.Data.Phone;

            WWW www_loadImage = new WWW(jsonResponse.Data.ImageURL);
            //StartCoroutine(loadProfileImage(www_loadImage));

            MessageHelper.CloseDialog();
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
     }
    #endregion

    IEnumerator loadProfileImage(WWW www)
    {
        yield return www;

        MessageHelper.LoadingDialog("Loading data....");
        if (www.isDone)
        {
            if (www.error == null)
            {
                ProfileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                MessageHelper.CloseDialog();
            }
        }
    }
}
