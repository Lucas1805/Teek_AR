using UnityEngine;
using System.Collections;
using Assets;
using Ucss;
using Assets.ResponseModels;
using System.Collections.Generic;
using LitJson;
using Assets.ViewModels;
using UnityEngine.UI;

public class CouponController : MonoBehaviour {

    public GameObject loadingPanel;
    public GameObject couponPanel;
    public GameObject CouponTemplate;
    public InputField CouponCodeInputField;
    public Text UsernameText;
    public Image ProfileImage;

	// Use this for initialization
	void Start () {
        LoadCouponList();
        LoadUserInformation();
        UsernameText.text = Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UsernameKey));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadCouponList()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadCouponList;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneLoadCouponListRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    public void RedeemCoupon()
    {
        if(CouponCodeInputField.text.Length > 0)
        {
            LoadingManager.showLoadingIndicator(loadingPanel);
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_RedeemCoupon;

            WWWForm form = new WWWForm();
            form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
            form.AddField("couponCode", CouponCodeInputField.text);

            request.formData = form;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneRedeemCouponRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
            request.onError = new EventHandlerServiceError(MessageHelper.OnError);

            UCSS.HTTP.PostForm(request);
        }
        else
        {
            LoadingManager.hideLoadingIndicator(loadingPanel);
            MessageHelper.ErrorDialog("Please enter coupon code");
        }
        
    }

    public void LoadUserInformation()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        string UserID = Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey));

        if (UserID != null)
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
            UsernameText.text = jsonResponse.Data.Username;

            WWW www_loadImage = new WWW(jsonResponse.Data.ImageURL);
            StartCoroutine(loadProfileImage(www_loadImage));
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion

    #region PROCESS LOAD COUPON LIST REQUEST
    private void OnDoneLoadCouponListRequest(string result, string transactionId)
    {
        ResponseModel<List<CouponModel>> jsonResponse = new ResponseModel<List<CouponModel>>();
        jsonResponse.Data = new List<CouponModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<CouponModel>>>(result);

        if (jsonResponse.Succeed)
        {
            if (jsonResponse.Data != null && jsonResponse.Data.Count > 0)
            {
                foreach (var item in jsonResponse.Data)
                {
                    if (item.Active == false && item.RedeemDate != null) //Only record with active = false and have redeem date is valid redeem. Record with active = false but no redeem date is expired (do not show)
                    {
                        GameObject newButton = Instantiate(CouponTemplate) as GameObject;
                        NotificationTemplate sampleButton = newButton.GetComponent<NotificationTemplate>();
                        sampleButton.Name.text = "Get " + item.Teek + " teek from code: " + item.Code;
                        sampleButton.RedeemDate.text = "Redeem at: " + Utils.JsonDateToDateTimeLongString(item.RedeemDate);
                        newButton.transform.SetParent(couponPanel.transform, false);
                    }
                }
            }
            else
            {
                //Show no record yet
                GameObject newButton = Instantiate(CouponTemplate) as GameObject;
                NotificationTemplate sampleButton = newButton.GetComponent<NotificationTemplate>();
                sampleButton.Name.text = "You has not redeem any coupon yet";
                sampleButton.RedeemDate.text = "";
                newButton.transform.SetParent(couponPanel.transform, false);
            }
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion

    #region PROCESS REDEEM COUPON REQUEST
    private void OnDoneRedeemCouponRequest(string result, string transactionId)
    {
        ResponseModel<CouponModel> jsonResponse = new ResponseModel<CouponModel>();
        jsonResponse.Data = new CouponModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CouponModel>>(result);

        if (jsonResponse.Succeed)
        {
            MessageHelper.MessageDialog("Success","Redeem coupon successfully. You received " + jsonResponse.Data.Teek + " Teek. Your new teek amount is: " + jsonResponse.Data.NewTeek);

            //Clear Coupon List
            foreach (Transform child in couponPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            LoadCouponList();
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    IEnumerator loadProfileImage(WWW www)
    {
        yield return www;

        LoadingManager.showLoadingIndicator(loadingPanel);
        if (www.isDone)
        {
            if (www.error == null)
            {
                ProfileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                //LoadingManager.hideLoadingIndicator(loadingPanel);
            }
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion
}
