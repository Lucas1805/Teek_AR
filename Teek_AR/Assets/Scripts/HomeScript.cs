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
    public GameObject loadingPanel;
    public InputField searchInput;
    public GameObject AllBrandsPanel;
    public GameObject MyBrandsPanel;

    public GameObject BrandButtonTemplateGO;

    // Use this for initialization
    void Start () {
        CallAPIGetOrganizers();
        CallAPIGetOrganizersByUserId();
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

    public void loadPlayerInfoScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.HomeSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.PlayerInfoSceneName);
    }

    public void loadRedeemCodeScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.HomeSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.RedeemCodeSceneName);
    }

    public void loadTutorialScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.HomeSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.TutorialSceneName);
    }

    public void loadEventDetailScene(int eventId)
    {
        PlayerPrefs.SetString(ConstantClass.PP_EventIDKey, eventId.ToString());

        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.HomeSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.EventDetailSceneName);
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
        HTTPRequest request = new HTTPRequest();
        //request.url = ConstantClass.
        request.url = "http://localhost:19291/api/organizer/getorganizers";
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIGetOrganizers);
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
                sampleBrandButton.BrandName.text = item.Name;
                sampleBrandButton.BrandAmount.text = item.StoreCount.ToString();
                //sampleBrandButton.BrandCategory.text = item.
                //sampleBrandButton.BrandLogo
                sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);


                newBrandButton.transform.SetParent(AllBrandsPanel.transform, false);
            }
        }
    }

    public void CallAPIGetOrganizersByUserId()
    {
        HTTPRequest request = new HTTPRequest();
        //request.url = ConstantClass.
        request.url = "http://localhost:19291/api/organizer/GetOrganizersByUserId?userid="
            + Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey));
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIGetOrganizers);
        UCSS.HTTP.GetString(request);
    }
}
