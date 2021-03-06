﻿using UnityEngine;
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
    public Text UserCoinText;
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
            if (jsonResponse.Data.Count > 0)
            {
                //Sort by name
                jsonResponse.Data = jsonResponse.Data.OrderBy(q => q.Name).ThenByDescending(q => q.StoreCount).ToList();
                
                foreach (var item in jsonResponse.Data)
                {
                    GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                    BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                    sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = item.Name;
                    sampleBrandButton.Brand.transform.GetChild(2).GetComponent<Text>().text = item.Name; // Original name
                    sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                    sampleBrandButton.BrandAmount.text = item.StoreCount.ToString() + " store";

                    sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                    sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                    sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);

                    //Load Image of brand
                    if(item.ImageUrl != null)
                    {
                        string url = ConstantClass.ImageHost + item.ImageUrl;
                        WWW www_loadImage = new WWW(url);
                        StartCoroutine(LoadImage(www_loadImage, sampleBrandButton.BrandLogo));
                    }

                    newBrandButton.transform.SetParent(AllBrandsPanel.transform, false);
                }
            }
            else
            {
                //Show record that say on data yet
                GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = "No data yet";

                newBrandButton.transform.SetParent(AllBrandsPanel.transform, false);
            }
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
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
            if(jsonResponse.Data.Count > 0)
            {
                //Sort by name
                jsonResponse.Data = jsonResponse.Data.OrderBy(q => q.Name).ThenByDescending(q => q.StoreCount).ToList();

                foreach (var item in jsonResponse.Data)
                {
                    {
                        GameObject newBrandButton = Instantiate(BrandButtonTemplateGO) as GameObject;
                        BrandButtonTemplate sampleBrandButton = newBrandButton.GetComponent<BrandButtonTemplate>();
                        sampleBrandButton.Brand.transform.GetChild(1).GetComponent<Text>().text = item.Name;
                        sampleBrandButton.Brand.transform.GetChild(2).GetComponent<Text>().text = item.Name; // Original name
                        sampleBrandButton.Brand.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                        sampleBrandButton.BrandAmount.text = item.StoreCount.ToString()  + " store";
                        sampleBrandButton.Activities.transform.GetChild(0).gameObject.SetActive(item.HasMultiplier);
                        sampleBrandButton.Activities.transform.GetChild(1).gameObject.SetActive(item.HasARGAME);
                        sampleBrandButton.Activities.transform.GetChild(2).gameObject.SetActive(item.HasVoting);

                        //Load Image of brand
                        if (item.ImageUrl != null)
                        {
                            string url = ConstantClass.ImageHost + item.ImageUrl;
                            WWW www_loadImage = new WWW(url);
                            StartCoroutine(LoadImage(www_loadImage, sampleBrandButton.BrandLogo));
                        }

                        newBrandButton.transform.SetParent(MyBrandsPanel.transform, false);
                    }
                }
            }
            
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
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
            if (jsonResponse.Data.Coin > 2)
            {
                UserCoinText.text = jsonResponse.Data.Coin + " coins";
            }
            else
            {
                UserCoinText.text = jsonResponse.Data.Coin + " coin";
            }
            //Load profile image
            if (jsonResponse.Data.ImageURL != null)
            {
                string url = ConstantClass.ImageHost + jsonResponse.Data.ImageURL;
                WWW www_loadImage = new WWW(url);
                StartCoroutine(loadProfileImage(www_loadImage));
            }
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
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
                try
                {
                    ProfileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                }
                catch
                {

                }
                finally
                {
                    LoadingManager.hideLoadingIndicator(loadingPanel);
                }
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

    IEnumerator LoadImage(WWW www, Image image)
    {
        yield return www;
        LoadingManager.showLoadingIndicator(loadingPanel);
        if (www.isDone)
        {
            if (www.error == null)
            {
                image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));               
            }
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
}
