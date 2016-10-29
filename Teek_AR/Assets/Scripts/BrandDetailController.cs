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
    public static string OrganizerName = "";
    
    public GameObject StoreButtonTemplate;
    public GameObject StoreContentPanel;
    public GameObject EventButtonTemplate;
    public GameObject EventContentPanel;
    public GameObject loadingPanel;
    public Text OrganizerNameText;

    public Text TeekAmountText;
    public Text RubyAmountText;
    public Text SapphireAmountText;
    public Text CitrineAmountText;

    private List<string> truncateLongerStringList = new List<string>();
    private float truncateLongerStringTime = 0;


    // Use this for initialization
    void Start () {

        OrganizerNameText.text = OrganizerName;
        truncateLongerStringList = Utils.TruncateLongerString(OrganizerName, 17);

        //Set OrganizerId to PlayerPrefs
        PlayerPrefs.SetInt(ConstantClass.PP_OrganizerId, OrganizerId);
        PlayerPrefs.SetString(ConstantClass.PP_OrganizerName, OrganizerName);
        PlayerPrefs.Save();

        LoadUserInformation();
        LoadEventListByOrganizer();
    }

    void Update()
    {
        #region QuanHM - TruncateLongerString
        if (OrganizerName.Length > 17)  // if length of EventName over 20 char then call TruncateLongerString
        {
            truncateLongerStringTime += Time.deltaTime * 2;
            if (truncateLongerStringTime <= truncateLongerStringList.Count)
            {
                OrganizerNameText.text = truncateLongerStringList[(int)truncateLongerStringTime];
            }
            else
            {
                truncateLongerStringTime = 0;
            }
        }
        else // else just show the original EventName
        {
            OrganizerNameText.text = OrganizerName;
        }
        #endregion
    }

    public void LoadStoreList()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        if (OrganizerId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadStoreList + "?organizerId=" + OrganizerId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadStoreListRequest);
            //request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
            request.onError = new EventHandlerServiceError(MessageHelper.OnError);


            UCSS.HTTP.GetString(request);
        }        
    }

    public void LoadEventListByOrganizer()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        if (OrganizerId != 0)
        {
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadEventListByOrganizer + "?organizerId=" + OrganizerId;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadEventListByOrganizerRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
            request.onError = new EventHandlerServiceError(MessageHelper.OnError);


            UCSS.HTTP.GetString(request);
        }
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
                //SHOW NO RECORD MESSAGE
                GameObject newButton = Instantiate(StoreButtonTemplate) as GameObject;
                StoreButtonTemplate sampleButton = newButton.GetComponent<StoreButtonTemplate>();
                
                sampleButton.StoreName.text = "This brand has no store yet!!";
                sampleButton.Address.text = "";
                sampleButton.GetComponent<Button>().interactable = false;
                newButton.transform.SetParent(StoreContentPanel.transform, false);
                Debug.Log("No Store To Show On Organizer: " + OrganizerId);
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
                    sampleButton.EventName.text = Utils.TruncateLongString(item.Name, 23);
                    sampleButton.OriginalName.text = item.Name;
                    sampleButton.Time.text = Utils.JsonDateToDateTimeLongString(item.StartDate);

                    //Load event image
                    WWW www_loadImage = new WWW(item.ImageUrl);
                    StartCoroutine(LoadImage(www_loadImage, sampleButton.EventImgUrl));


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
                    
                    newButton.transform.SetParent(EventContentPanel.transform, false);
                }
                
            }
            else
            {
                //SHOW NO RECORD MESSAGE
                GameObject newButton = Instantiate(EventButtonTemplate) as GameObject;
                EventButtonTemplate sampleButton = newButton.GetComponent<EventButtonTemplate>();
                
                sampleButton.EventName.text = "No available event at this time yet";
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

    public void LoadUserInformation()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadCustomerInformation;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", OrganizerId);

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnLoadUserInformationRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    #region PROCESS LOAD USER INFORMATION REQUEST
    private void OnLoadUserInformationRequest(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            TeekAmountText.text = jsonResponse.Data.Teek.ToString() + " Teek";
            RubyAmountText.text = jsonResponse.Data.Ruby.ToString();
            SapphireAmountText.text = jsonResponse.Data.Sapphire.ToString();
            CitrineAmountText.text = jsonResponse.Data.Citrine.ToString();
        }
        else
        {
            //Show error message
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion
}
