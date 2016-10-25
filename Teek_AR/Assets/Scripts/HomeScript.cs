using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Assets;
using System.Collections.Generic;
using Ucss;
using Assets.ResponseModels;
using LitJson;

public class HomeScript : MonoBehaviour {

    //public Text mac;
    public InputField searchInput;
    public GameObject AllBrandsPanel;
    public GameObject MyBrandsPanel;

    public GameObject loadingPanel;
    public GameObject BrandButtonTemplateGO;
    public Toggle AllToggle;
    public Toggle MultiplierToggle;
    public Toggle GameToggle;
    public Toggle VotingToggle;
    public Text UsernameText;
    public Image ProfileImage;

    private bool isFilterAll;

    // Use this for initialization
    void Start () {
        CallAPIGetOrganizers();
        CallAPIGetOrganizersByUserId();
        LoadUserInformation();
    }
	
	// Update is called once per frame
	void Update () {
        
	}


    /// <summary>
    /// This function is used to get MAC address of Wifi the phone is connected to. THIS FUNTION ONLY WORK ON ANDROID
    /// </summary>
    /// <returns>MAC Address String</returns>
    private string getBSSID()
    {
#if UNITY_ANDROID
        string bssid = null;

        AndroidJavaObject mWiFiManager = null;
        if (mWiFiManager == null)
        {
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                mWiFiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
            }
        }
        bssid = mWiFiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getBSSID");
        return bssid;
#endif

#if UNITY_IOS
        //NOT IMPLEMENT YET
#endif
    }


    public void showMac()
    {
        //mac.text = getBSSID();
    }
    
    
    
    public void SearchByName()
    {
        foreach (Transform item in AllBrandsPanel.transform)
        {
            GameObject parent = item.gameObject;
            if (!parent.GetComponentInChildren<Text>().text.ToLower().StartsWith(searchInput.text.ToLower()))
            {
                parent.SetActive(false);
            } else
            {
                parent.SetActive(true);
            }
        }
        foreach (Transform item in MyBrandsPanel.transform)
        {
            GameObject parent = item.gameObject;
            if (!parent.GetComponentInChildren<Text>().text.ToLower().StartsWith(searchInput.text.ToLower()))
            {
                parent.SetActive(false);
            }
            else
            {
                parent.SetActive(true);
            }
        }
    }

    public void CallAPIGetOrganizers()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        //request.url = ConstantClass.
        request.url = ConstantClass.API_LoadOrganizer;
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIGetOrganizers);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }

    public void OnDoneCallAPIGetOrganizers(string result, string transactionId)
    {
        ResponseModel<List<OrganizerModel>> jsonResponse = new ResponseModel<List<OrganizerModel>>();
        jsonResponse.Data = new List<OrganizerModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<OrganizerModel>>>(result);

        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = Utils.TruncateLongString(item.Name,18);
                sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                sampleBrandButton.BrandAmount.text = item.StoreCount.ToString() + " store(s)";
               
                sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);

                newBrandButton.transform.SetParent(AllBrandsPanel.transform, false);
            }
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    public void CallAPIGetOrganizersByUserId()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadMyBrand + "?userId="
            + Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey));
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIGetOrganizersByUserId);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }

    public void OnDoneCallAPIGetOrganizersByUserId(string result, string transactionId)
    {
        ResponseModel<List<OrganizerModel>> jsonResponse = new ResponseModel<List<OrganizerModel>>();
        jsonResponse.Data = new List<OrganizerModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<OrganizerModel>>>(result);

        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                {
                    GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                    BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                    sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = Utils.TruncateLongString(item.Name, 18);
                    sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                    sampleBrandButton.BrandAmount.text = item.StoreCount.ToString();
                    sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                    sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                    sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);
                    newBrandButton.transform.SetParent(MyBrandsPanel.transform, false);
                }
            }
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    public void FilterCheck()
    {
        foreach (Transform item in AllBrandsPanel.transform)
        {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in AllBrandsPanel.transform)
        {
            if (MultiplierToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(0).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }

            if (GameToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(1).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }

            if (VotingToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(2).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }

        foreach (Transform item in MyBrandsPanel.transform)
        {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in MyBrandsPanel.transform)
        {
            if (MultiplierToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(0).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }

            if (GameToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(1).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }

            if (VotingToggle.isOn)
            {
                if (item.GetComponent<BrandButtonTemplate>().Activities.transform.GetChild(2).gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }

        if ((!MultiplierToggle.isOn || !GameToggle.isOn || !VotingToggle.isOn) && !isFilterAll)
        {
            AllToggle.isOn = false;
        }
    }

    public void FilterAll()
    {
        isFilterAll = true;
        if (AllToggle.isOn)
        {
            AllToggle.isOn = true;
            MultiplierToggle.isOn = true;
            GameToggle.isOn = true;
            VotingToggle.isOn = true;

            foreach (Transform item in AllBrandsPanel.transform)
            {
                item.gameObject.SetActive(true);
            }

            foreach (Transform item in MyBrandsPanel.transform)
            {
                item.gameObject.SetActive(true);
            }
        }
        isFilterAll = false;
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
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion

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

    public void Refresh()
    {
        //Delete Brand List
        
        foreach (Transform child in AllBrandsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        CallAPIGetOrganizers();

        //Delete My Brand List
        foreach (Transform child in MyBrandsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        CallAPIGetOrganizersByUserId();

        LoadUserInformation();

    }
}
