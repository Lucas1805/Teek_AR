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

    public GameObject EventButtonTemplate;
    public GameObject EventContentPanel;
    public GameObject LoadingPanel;
    public Text StoreNameText;

    // Use this for initialization
    void Start()
    {
        StoreNameText.text = Utils.TruncateLongString(StoreName,21);
        //TEST VALUE
        LoadEventListByStore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadEventListByStore()
    {
        LoadingManager.showLoadingIndicator(LoadingPanel);

        if (StoreId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadEventListByStore + "?storeId=" + StoreId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadEventListByStoreRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
            request.onError = new EventHandlerServiceError(this.OnLoadEventListError);


            UCSS.HTTP.GetString(request);
        }
        else
        {
            MessageHelper.MessageDialog("Error, Cannot get store ID");
        }

        LoadingManager.hideLoadingIndicator(LoadingPanel);

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
                    sampleButton.Time.text = item.StartDate;

                    //Load event image
                    WWW www_loadImage = new WWW("https://agenciatrampo.com.br/wp-content/uploads/2016/02/Solu%C3%A7%C3%B5es-em-Marketing-Digital-para-eventos-480x379.jpg");
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
                //SHOW NO RECORD MESSAGE BY CREATE A EMMPTY BUTTON AND MESSAGE
                MessageHelper.MessageDialog("This store has no event now");
                Debug.Log("No Event To Show");
            }
        }
        else
        {
            //Show error message
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadEventListError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);
        Debug.Log("WWW Error: " + error);
    }

    private void OnTimeOut(string transactionId)
    {
        MessageHelper.MessageDialog(ConstantClass.Msg_TimeOut);
        Debug.Log(ConstantClass.Msg_TimeOut);
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
        MySceneManager.loadPreviousScene();
    }

    IEnumerator LoadImage(WWW www, Image image)
    {
        LoadingManager.showLoadingIndicator(LoadingPanel);
        yield return www;
        image.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        LoadingManager.hideLoadingIndicator(LoadingPanel);

    }
}

