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

public class StoreEventController : MonoBehaviour
{
    public static int StoreId;
    public static string StoreName = "";

    public GameObject loadingPanel;
    public GameObject EventButtonTemplate;
    public GameObject EventContentPanel;
    public Text StoreNameText;

    // Use this for initialization
    void Start()
    {
        StoreNameText.text = Utils.TruncateLongString(StoreName,21);
        LoadEventListByStore();
    }

    public void LoadEventListByStore()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        if (StoreId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadEventListByStore + "?storeId=" + StoreId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadEventListByStoreRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
            request.onError = new EventHandlerServiceError(MessageHelper.OnError);


            UCSS.HTTP.GetString(request);
        }
        else
        {
            LoadingManager.hideLoadingIndicator(loadingPanel);
            MessageHelper.MessageDialog("Error, Cannot get store ID");
        }

    }

    #region PROCESS LOAD EVENT LIST BY ORGANIZER REQUEST
    private void OnDoneCallLoadEventListByStoreRequest(string result, string transactionId)
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
                    sampleButton.EventName.text = Utils.TruncateLongString(item.Name, 23);
                    sampleButton.OriginalName.text = item.Name;
                    sampleButton.Time.text = Utils.JsonDateToDateTimeLongString(item.StartDate);

                    //Load event image
                    WWW www_loadImage = new WWW(item.ImageUrl);
                    StartCoroutine(LoadImage(www_loadImage, sampleButton.EventImgUrl));

                    if (item.Multiplier != 0)
                        sampleButton.Activities.transform.GetChild(1).gameObject.SetActive(true);

                    if(item.Activities != null)
                    {
                        foreach (var activity in item.Activities)
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

                    newButton.transform.SetParent(EventContentPanel.transform, false);
                }
                
            }
            else
            {
                //SHOW NO RECORD MESSAGE
                GameObject newButton = Instantiate(EventButtonTemplate) as GameObject;
                EventButtonTemplate sampleButton = newButton.GetComponent<EventButtonTemplate>();

                sampleButton.EventName.text = "No available event at this time";
                sampleButton.Time.gameObject.SetActive(false);
                sampleButton.GetComponent<Button>().interactable = false;
                newButton.transform.SetParent(EventContentPanel.transform, false);
            }
        }
        else
        {
            //Show error message
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    
    #endregion

    public void Refresh()
    {
        //Clear Event List
        foreach (Transform child in EventContentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadEventListByStore();
    }

    public void LoadPreviousScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        MySceneManager.loadPreviousScene();
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
                LoadingManager.hideLoadingIndicator(loadingPanel);
            }
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
}

