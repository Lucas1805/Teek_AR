﻿using UnityEngine;
using System.Collections;
using Ucss;
using Assets;
using Assets.ResponseModels;
using LitJson;
using System.Collections.Generic;
using Assets.ViewModels;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BrandDetailController : MonoBehaviour {

    public static int OrganizerId;
    public static string OrganizerName = "";

    public bool OnStoreTab = true;
    public GameObject StoreButtonTemplate;
    public GameObject StoreContentPanel;
    public GameObject EventButtonTemplate;
    public GameObject EventContentPanel;
    public GameObject LoadingPanel;
    public Text OrganizerNameText;


	// Use this for initialization
	void Start () {
        //TEST VALUE FOR ORGANIZER ID
        OrganizerNameText.text = Utils.TruncateLongString(OrganizerName, 21);

        //Set OrganizerId to PlayerPrefs
        PlayerPrefs.SetInt(ConstantClass.PP_OrganizerId, OrganizerId);
        PlayerPrefs.Save();

        LoadStoreList();
        LoadEventListByOrganizer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadStoreList()
    {
        MessageHelper.LoadingDialog("Loading data....");

        if(OrganizerId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadStoreList + "?organizerId=" + OrganizerId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadStoreListRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
            request.onError = new EventHandlerServiceError(this.OnLoadStoreListError);
            

            UCSS.HTTP.GetString(request);
        }
        else
        {
            //Show error message
        }

        
    }

    public void LoadEventListByOrganizer()
    {
        MessageHelper.LoadingDialog("Loading data....");

        if (OrganizerId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadEventListByOrganizer + "?organizerId=" + OrganizerId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadEventListByOrganizerRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
            request.onError = new EventHandlerServiceError(this.OnLoadEventListError);


            UCSS.HTTP.GetString(request);
        }
        else
        {
            //Show error message
        }

        
    }


    #region PROCESS LOAD STORE REQUEST
    private void OnDoneCallLoadStoreListRequest(string result, string transactionId)
    {
        MessageHelper.CloseDialog();
        ResponseModel<List<StoreModel>> jsonResponse = new ResponseModel<List<StoreModel>>();
        jsonResponse.Data = new List<StoreModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<StoreModel>>>(result);

        if (jsonResponse.Succeed)
        {
            if(jsonResponse.Data.Count > 0)
            {
                foreach (var item in jsonResponse.Data)
                {
                    GameObject newButton = Instantiate(StoreButtonTemplate) as GameObject;
                    StoreButtonTemplate sampleButton = newButton.GetComponent<StoreButtonTemplate>();

                    sampleButton.StoreId.text = item.Id.ToString();
                    sampleButton.StoreName.text = item.Name;
                    sampleButton.Address.text = item.Address;


                    newButton.transform.SetParent(StoreContentPanel.transform, false);
                }
            }
            else
            {
                //SHOW NO RECORD MESSAGE BY CREATE A EMMPTY BUTTON AND MESSAGE
                MessageHelper.MessageDialog("This brand has no store yet");
                Debug.Log("No Store To Show");
            }

            MessageHelper.CloseDialog();
        }
        else
        {
            //Show error message
            MessageHelper.CloseDialog();
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadStoreListError(string error, string transactionId)
    {
        MessageHelper.CloseDialog();
        MessageHelper.MessageDialog(error);
        Debug.Log("WWW Error: " + error);
    }

    private void OnTimeOut(string transactionId)
    {
        MessageHelper.CloseDialog();
        MessageHelper.MessageDialog(ConstantClass.Msg_TimeOut);
        Debug.Log(ConstantClass.Msg_TimeOut);
    }
    #endregion

    #region PROCESS LOAD EVENT LIST BY ORGANIZER REQUEST
    private void OnDoneCallLoadEventListByOrganizerRequest(string result, string transactionId)
    {
        MessageHelper.CloseDialog();
        ResponseModel<List<EventModel>> jsonResponse = new ResponseModel<List<EventModel>>();
        jsonResponse.Data = new List<EventModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventModel>>>(result);

        if (jsonResponse.Succeed)
        {
            if (jsonResponse.Data.Count > 0)
            {
                foreach (var item in jsonResponse.Data)
                {
                    GameObject newButton = Instantiate(EventButtonTemplate) as GameObject;
                    EventButtonTemplate sampleButton = newButton.GetComponent<EventButtonTemplate>();

                    sampleButton.EventId.text = item.Id.ToString();
                    sampleButton.EventName.text = item.Name;
                    sampleButton.Time.text = item.StartDate;

                    //Load event image
                    WWW www_loadImage = new WWW("https://agenciatrampo.com.br/wp-content/uploads/2016/02/Solu%C3%A7%C3%B5es-em-Marketing-Digital-para-eventos-480x379.jpg");
                    StartCoroutine(LoadImage(www_loadImage, sampleButton.EventImgUrl));


                    if (item.Multiplier != 0)
                        sampleButton.Activities.transform.GetChild(1).gameObject.SetActive(true);

                    if (item.Activities != null)
                    {
                        foreach (var activity in item.Activities)
                        {
                            if (activity != null)
                            {
                                if (activity.GameId != null || activity.GameId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(0).gameObject.SetActive(true);
                                }
                                if (activity.SurveyId != null || activity.SurveyId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(2).gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    

                    newButton.transform.SetParent(EventContentPanel.transform, false);
                }
            }
            else
            {
                //SHOW NO RECORD MESSAGE BY CREATE A EMMPTY BUTTON AND MESSAGE
                MessageHelper.CloseDialog();
                MessageHelper.MessageDialog("No available event at this time");
                Debug.Log("No Event To Show");
            }

            MessageHelper.CloseDialog();
        }
        else
        {
            //Show error message
            MessageHelper.CloseDialog();
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadEventListError(string error, string transactionId)
    {
        //showRegisterMessage(error);
        MessageHelper.CloseDialog();
        Debug.Log("WWW Error: " + error);
    }
    #endregion

    public void Refresh()
    {
        //Clear Store List
        foreach (Transform child in StoreContentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadStoreList();

        //Clear Event List
        foreach (Transform child in EventContentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadEventListByOrganizer();
    }

    public void LoadPreviousScene()
    {
        MySceneManager.loadPreviousScene();
    }

    IEnumerator LoadImage(WWW www, Image image)
    {
        MessageHelper.LoadingDialog("Loading data....");
        yield return www;
        image.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        MessageHelper.CloseDialog();
        
    }
}
