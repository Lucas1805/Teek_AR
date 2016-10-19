using UnityEngine;
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
    public static string OrganizerName;
    public bool OnStoreTab = true;
    public GameObject StoreButtonTemplate;
    public GameObject StoreContentPanel;
    public GameObject EventButtonTemplate;
    public GameObject EventContentPanel;
    public GameObject LoadingPanel;


	// Use this for initialization
	void Start () {
        //TEST VALUE FOR ORGANIZER ID
        OrganizerId = 39;
        LoadStoreList();
        LoadEventListByOrganizer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadStoreList()
    {
        LoadingManager.showLoadingIndicator(LoadingPanel);

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

        LoadingManager.hideLoadingIndicator(LoadingPanel);
    }

    public void LoadEventListByOrganizer()
    {
        LoadingManager.showLoadingIndicator(LoadingPanel);

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

        LoadingManager.hideLoadingIndicator(LoadingPanel);
    }


    #region PROCESS LOAD STORE REQUEST
    private void OnDoneCallLoadStoreListRequest(string result, string transactionId)
    {
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
                Debug.Log("No Store To Show");
            }
        }
        else
        {
            //Show error message
        }
    }

    private void OnLoadStoreListError(string error, string transactionId)
    {
        //showRegisterMessage(error);
        Debug.Log("WWW Error: " + error);
    }

    private void OnTimeOut(string transactionId)
    {
        //showLoginMessage(ConstantClass.Msg_TimeOut);
        Debug.Log(ConstantClass.Msg_TimeOut);
    }
    #endregion

    #region PROCESS LOAD EVENT LIST BY ORGANIZER REQUEST
    private void OnDoneCallLoadEventListByOrganizerRequest(string result, string transactionId)
    {
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
                Debug.Log("No Event To Show");
            }
        }
        else
        {
            //Show error message
        }
    }

    private void OnLoadEventListError(string error, string transactionId)
    {
        //showRegisterMessage(error);
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

    
}
