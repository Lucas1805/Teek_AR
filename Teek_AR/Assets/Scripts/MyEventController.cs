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

public class MyEventController : MonoBehaviour {

    public Text UsernameText;
    public Image ProfileImage;
    public GameObject EventButtonTemplate;
    public GameObject AllEventPanel;
    public GameObject MyEventPanel;
    public GameObject loadingPanel;
    public InputField searchField;

    // Use this for initialization
    void Start () {

        LoadAllEvent();
        LoadMyEvent();
        LoadUserInformation();
	}

    private void LoadAllEvent()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_GetHotEvents;
        WWWForm form = new WWWForm();
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadAllEventRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.GetString(request);
    }

    private void OnDoneCallLoadAllEventRequest(string result, string transactionId)
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
                    sampleButton.EventName.text = Utils.TruncateLongString(item.Name, 35);
                    sampleButton.Time.text = Utils.JsonDateToDateTimeLongString(item.StartDate);

                    //Load event image
                    if (item.ImageUrl != null)
                    {
                        string url = ConstantClass.ImageHost + item.ImageUrl;
                        WWW www_loadImage = new WWW(url);
                        StartCoroutine(LoadImage(www_loadImage, sampleButton.EventImgUrl));
                    }


                    if (item.Multiplier != 0)
                        sampleButton.Activities.transform.GetChild(1).gameObject.SetActive(true);

                    if (item.Activities != null)
                    {
                        foreach (var activity in item.Activities)
                        {
                            if (activity != null)
                            {
                                if (activity.GameId != null && activity.GameId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(0).gameObject.SetActive(true);
                                }
                                if (activity.SurveyId != null && activity.SurveyId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(2).gameObject.SetActive(true);
                                }
                            }
                        }
                    }

                    if (item.BrandImageUrl != null)
                    {
                        string url = ConstantClass.ImageHost + item.BrandImageUrl;
                        WWW www_loadImage = new WWW(url);
                        sampleButton.Brand.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Filled;
                        StartCoroutine(LoadImage(www_loadImage, sampleButton.Brand.transform.GetChild(0).GetComponent<Image>()));
                    }

                    newButton.transform.SetParent(AllEventPanel.transform, false);
                }

            }
            else
            {
                //SHOW NO RECORD MESSAGE
                GameObject newButton = Instantiate(EventButtonTemplate) as GameObject;
                EventButtonTemplate sampleButton = newButton.GetComponent<EventButtonTemplate>();

                sampleButton.EventName.text = "No available event yet";
                sampleButton.Time.gameObject.SetActive(false);
                sampleButton.GetComponent<Button>().interactable = false;
                newButton.transform.SetParent(AllEventPanel.transform, false);
            }
        }
        else
        {
            //Show error message
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void LoadMyEvent()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadMyEvent;
        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadMyEventRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    #region PROCESS LOAD EVENT LIST BY ORGANIZER REQUEST
    private void OnDoneCallLoadMyEventRequest(string result, string transactionId)
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
                    sampleButton.EventName.text = Utils.TruncateLongString(item.Name, 35);
                    sampleButton.Time.text = Utils.JsonDateToDateTimeLongString(item.StartDate);

                    //Load event image
                    if (item.ImageUrl != null)
                    {
                        string url = ConstantClass.ImageHost + item.ImageUrl;
                        WWW www_loadImage = new WWW(url);
                        StartCoroutine(LoadImage(www_loadImage, sampleButton.EventImgUrl));
                    }


                    if (item.Multiplier != 0)
                        sampleButton.Activities.transform.GetChild(1).gameObject.SetActive(true);

                    if (item.Activities != null)
                    {
                        foreach (var activity in item.Activities)
                        {
                            if (activity != null)
                            {
                                if (activity.GameId != null && activity.GameId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(0).gameObject.SetActive(true);
                                }
                                if (activity.SurveyId != null && activity.SurveyId != 0)
                                {
                                    sampleButton.Activities.transform.GetChild(2).gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    if (item.BrandImageUrl != null)
                    {
                        string url = ConstantClass.ImageHost + item.BrandImageUrl;
                        WWW www_loadImage = new WWW(url);
                        sampleButton.Brand.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Filled;
                        StartCoroutine(LoadImage(www_loadImage, sampleButton.Brand.transform.GetChild(0).GetComponent<Image>()));
                    }
                    newButton.transform.SetParent(MyEventPanel.transform, false);
                }

            }
            else
            {
                //SHOW NO RECORD MESSAGE
                GameObject newButton = Instantiate(EventButtonTemplate) as GameObject;
                EventButtonTemplate sampleButton = newButton.GetComponent<EventButtonTemplate>();

                sampleButton.EventName.text = "You has not join any event yet";
                sampleButton.Time.gameObject.SetActive(false);
                sampleButton.GetComponent<Button>().interactable = false;
                newButton.transform.SetParent(MyEventPanel.transform, false);
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

    public void Refresh()
    {

        //Clear Event List
        foreach (Transform child in MyEventPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadMyEvent();
        LoadUserInformation();
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
    public void SearchByName()
    {
        foreach (Transform item in MyEventPanel.transform)
        {
            GameObject parent = item.gameObject;
            if (!parent.transform.GetChild(3).GetComponent<Text>().text.ToLower().Contains(searchField.text.ToLower()))
            {
                parent.SetActive(false);
            }
            else
            {
                parent.SetActive(true);
            }
        }
        foreach (Transform item in AllEventPanel.transform)
        {
            GameObject parent = item.gameObject;
            if (!parent.transform.GetChild(3).GetComponent<Text>().text.ToLower().Contains(searchField.text.ToLower()))
            {
                parent.SetActive(false);
            }
            else
            {
                parent.SetActive(true);
            }
        }
    }
}
