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

    public GameObject BrandButtonTemplateGO;
    public Toggle AllToggle;
    public Toggle MultiplierToggle;
    public Toggle GameToggle;
    public Toggle VotingToggle;

    // Use this for initialization
    void Start () {
        CallAPIGetOrganizers();
        CallAPIGetOrganizersByUserId();
        //PlayerPrefs.SetString(ConstantClass.PP_UserIDKey, Encrypt.EncryptString("40efe638-04b6-42aa-81c3-a79b208d75e5"));
        //PlayerPrefs.Save();
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
        MessageHelper.LoadingDialog("Loading data....");
        HTTPRequest request = new HTTPRequest();
        //request.url = ConstantClass.
        request.url = ConstantClass.API_LoadOrganizer;
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
                sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = Utils.TruncateLongString(item.Name,18);
                sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                sampleBrandButton.BrandAmount.text = item.StoreCount.ToString() + " store(s)";
               
                sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);


                newBrandButton.transform.SetParent(AllBrandsPanel.transform, false);
            }
            MessageHelper.CloseDialog();
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    public void CallAPIGetOrganizersByUserId()
    {
        MessageHelper.LoadingDialog("Loading data....");
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadMyBrand + "?userId="
            + Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey));
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIGetOrganizersByUserId);
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
                //if (item.HasARGAME || item.HasMultiplier || item.HasVoting)
                {
                    GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                    BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                    sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = Utils.TruncateLongString(item.Name, 18);
                    sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                    sampleBrandButton.BrandAmount.text = item.StoreCount.ToString();
                    //sampleBrandButton.BrandCategory.text = item.
                    //sampleBrandButton.BrandLogo
                    sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                    sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                    sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);

                    newBrandButton.transform.SetParent(MyBrandsPanel.transform, false);
                }
            }
            MessageHelper.CloseDialog();
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }        
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

        if (!MultiplierToggle.isOn || !GameToggle.isOn || !VotingToggle.isOn)
        {
            AllToggle.isOn = false;
        }
    }

    public void FilterAll()
    {
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

    }
}
