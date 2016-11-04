using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Ucss;
using Assets.ResponseModels;
using LitJson;
using Assets;
using System;
using System.Linq;
using Assets.ViewModels;

public class EventDetailScript : MonoBehaviour
{
    public GameObject ButtonTemplate;
    public GameObject CouponTemplate;
    public GameObject GameTemplate;
    public GameObject StoreButtonTemplate;
    public GameObject SurveyTemplate;
    public GameObject VotingTemplate;
    public GameObject CheckinTemplate;
    public GameObject RegisterEventButton;

    public static int EventId;
    public static string EventName;
    public static Image EventImage;
    public int OrganizerId;

    public GameObject loadingPanel;

    public Text TeekAmountText;
    public Text RubyAmountText;
    public Text SapphireAmountText;

    //Buu
    public Text PrizeName;
    public Text PrizeTeek;
    public Image PrizeImage;
    public Text PrizeRuby;
    public Text PrizeCitrine;
    public Text PrizeSapphire;
    public Text OrLabel;


    public Text EventFullName;
    public Text EventMultiplier;
    public Text EventCalendar;
    public Text CitrineAmountText;
    public Text EventNameText;
    public Image EventImageObject;
    public GameObject PrizePanel;
    public GameObject BagPanel;
    public GameObject InformationPanel;
    public GameObject ActivityPanel;
    public GameObject RedeemCodePanel;
    public InputField MasterCodeText;
    public GameObject RedeemPrizePanel;
    public GameObject RedeemPrizeCodeSuccessPanel;
    public Text CountTimeText;
    public Text RedeemPrizeCodeSuccessMessage;
    public Button ClaimByTeekButton;
    public Button ClaimByGemButton;

    private int PrizeCodeId;
    private int PrizeId;
    private bool StartCountTime = false; //Use to count down time when redeem prize code success

    private float TimeOutRedeemPrizeCodeSuccessfully = 5f * 60;

    private List<string> truncateLongerStringList = new List<string>();
    private float truncateLongerStringTime = 0;

    // Use this for initialization
    void Start()
    {
        truncateLongerStringList = Utils.TruncateLongerString(EventName, 17);

        //Get Organizer Id from PlayerPrefs
        OrganizerId = PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId);

        //Set Eventid To PlayerPrefs
        PlayerPrefs.SetInt(ConstantClass.PP_EventIDKey, EventId);
        PlayerPrefs.Save();

        LoadUserInformation();
        LoadEventInfo();
        GetPrizeData();
        LoadInformtion();
        LoadStores();
        LoadActivities();
        LoadPrizeCode();

    }

    private void LoadInformtion()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_GetEventDetails + "?EventId=" + EventId;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallGetDetails);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }

    private void OnDoneCallGetDetails(string result, string transactionId)
    {
        ResponseModel<EventModel> jsonResponse = new ResponseModel<EventModel>();
        jsonResponse.Data = new EventModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<EventModel>>(result);

        if (jsonResponse.Succeed)
        {
            var item = jsonResponse.Data;
                EventFullName.text = item.Name.ToString();
                EventMultiplier.text = item.Multiplier.ToString();
                EventCalendar.text = Utils.JsonDateToDateTimeLongString(item.StartDate.ToString()) + " - " + Utils.JsonDateToDateTimeLongString(item.EndDate.ToString());

            //Load event image
            if (item.ImageUrl != null)
            {
                string url = ConstantClass.ImageHost + item.ImageUrl;
                WWW www_loadImage = new WWW(url);
                StartCoroutine(LoadImage(www_loadImage, EventImageObject));
            }
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    private void LoadStores()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_GetStoreByEventId + "?EventId=" + EventId;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallGetStores);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }

    private void OnDoneCallGetStores(string result, string transactionId)
    {
        ResponseModel<List<StoreModel>> jsonResponse = new ResponseModel<List<StoreModel>>();
        jsonResponse.Data = new List<StoreModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<StoreModel>>>(result);

        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                GameObject newButton = Instantiate(StoreButtonTemplate) as GameObject;
                StoreButtonTemplate sampleButton = newButton.GetComponent<StoreButtonTemplate>();
                sampleButton.StoreId.text = item.Id.ToString();
                sampleButton.StoreName.text = item.Name.ToString();
                sampleButton.Address.text = item.Address.ToString();
                newButton.transform.SetParent(InformationPanel.transform, false);
            }
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    private void LoadActivities()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_GetPublishedActivityByEventId + "?EventId=" + EventId;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallGetActivities);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }

    private void OnDoneCallGetActivities(string result, string transactionId)
    {
        ResponseModel<List<ActivityModel>> jsonResponse = new ResponseModel<List<ActivityModel>>();
        jsonResponse.Data = new List<ActivityModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<ActivityModel>>>(result);

        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                if (item.GameId != null)
                {
                    GameObject newButton = Instantiate(GameTemplate) as GameObject;
                    GameButtonTemplate sampleButton = newButton.GetComponent<GameButtonTemplate>();
                    sampleButton.GameId.text = item.GameId.ToString();
                    newButton.transform.SetParent(ActivityPanel.transform, false);
                }
            }
        }
        else
        {
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    void Update()
    {
        if (StartCountTime)
        {
            TimeOutRedeemPrizeCodeSuccessfully -= Time.deltaTime;
            CountTimeText.text = "Auto close in: " + ((int)Math.Truncate(TimeOutRedeemPrizeCodeSuccessfully) / 60).ToString() + ":" + (Math.Truncate(TimeOutRedeemPrizeCodeSuccessfully) % 60).ToString() + " minute(s)";
        }
        if (TimeOutRedeemPrizeCodeSuccessfully <= 0)
        {
            RedeemPrizeCodeSuccessPanel.SetActive(false);
            StartCountTime = false;
            TimeOutRedeemPrizeCodeSuccessfully = 5f * 60;
        }

        #region QuanHM - TruncateLongerString
        if (EventName.Length > 17)  // if length of EventName over 20 char then call TruncateLongerString
        {
            truncateLongerStringTime += Time.deltaTime * 2;
            if (truncateLongerStringTime <= truncateLongerStringList.Count)
            {
                EventNameText.text = truncateLongerStringList[(int)truncateLongerStringTime];
            }
            else
            {
                truncateLongerStringTime = 0;
            }
        }
        else // else just show the original EventName
        {
            EventNameText.text = EventName;
        }
        #endregion
    }

    public void GetPrizeData()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadPrizeList + "?EventId=" + EventId;

        request.stringCallback = new EventHandlerHTTPString(this.PopulateDataList);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        UCSS.HTTP.GetString(request);
    }
    private void PopulateDataList(string result, string transactionId)
    {
        ResponseModel<List<PrizeResponseModel>> jsonResponse = new ResponseModel<List<PrizeResponseModel>>();
        jsonResponse.Data = new List<PrizeResponseModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<PrizeResponseModel>>>(result);

        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                GameObject newButton = Instantiate(ButtonTemplate) as GameObject;
                RewardButtonTemplate sampleButton = newButton.GetComponent<RewardButtonTemplate>();
                sampleButton.PrizeName.text = item.Name;
                sampleButton.PrizeId.text = item.Id.ToString();
                sampleButton.GetComponent<Button>().onClick.AddListener(() => ShowRedeemPrizePanel(sampleButton));

                if (item.Ruby != 0)
                {
                    sampleButton.Gem.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = item.Ruby.ToString();
                }
                if (item.Sapphire != 0)
                {
                    sampleButton.Gem.transform.GetChild(2).gameObject.GetComponentInChildren<Text>().text = item.Sapphire.ToString();
                }
                if (item.Citrine != 0)
                {
                    sampleButton.Gem.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text = item.Citrine.ToString();
                }
                if (item.Teek != 0)
                {
                    sampleButton.Coin.GetComponentInChildren<Text>().text = item.Teek.ToString();
                }
                if (item.ImageURL != null)
                {
                    string url = ConstantClass.API_HOST_IP + item.ImageURL;
                    WWW www_loadImage = new WWW(url);
                    GameObject child = sampleButton.PrizeImage.transform.GetChild(0).gameObject;
                    StartCoroutine(LoadImage(www_loadImage, child.transform.GetChild(0).gameObject.GetComponent<Image>()));
                }

                newButton.transform.SetParent(PrizePanel.transform, false);
            }
        }
        else
        {
            //MessageHelper.MessageDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);

    }





    public void PlayGame()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        SceneManager.LoadSceneAsync(Assets.ConstantClass.GameSceneName);
    }

    public void LoadUserInformation()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadCustomerInformation;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnLoadUserInformationRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    public void LoadPrizeCode()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadPrizeCode;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("eventId", EventId);

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnLoadPrizeCodeRequest);
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

    void LoadEventInfo()
    {
        if (EventName != null && EventName.Length > 0)
        {
            EventNameText.text = Utils.TruncateLongString(EventName, 23);
        }
        else
        {
            EventNameText.text = "";
        }

        if (EventImage != null)
        {
            EventImageObject.sprite = EventImage.sprite;
        }
    }

    #region PROCESS LOAD PRIZE CODE REQUEST
    private void OnLoadPrizeCodeRequest(string result, string transactionId)
    {
        ResponseModel<List<PrizeCodeModel>> jsonResponse = new ResponseModel<List<PrizeCodeModel>>();
        jsonResponse.Data = new List<PrizeCodeModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<PrizeCodeModel>>>(result);

        if (jsonResponse.Succeed)
        {
            if (jsonResponse.Data != null && jsonResponse.Data.Count > 0)
            {
                //Sort before show
                jsonResponse.Data = jsonResponse.Data.OrderByDescending(t => t.Status).ThenBy(t => t.PrizeName).ToList();

                foreach (var item in jsonResponse.Data)
                {
                    GameObject newButton = Instantiate(CouponTemplate) as GameObject;
                    CouponButtonTemplate sampleButton = newButton.GetComponent<CouponButtonTemplate>();
                    sampleButton.PrizeName.text = item.PrizeName;
                    sampleButton.CouponId.text = item.Id.ToString();
                    if (item.Status == false && item.Date != null)
                    {
                        sampleButton.RedeemDate.text = "Redeem at: " + Utils.JsonDateToDateTimeLongString(item.Date);
                    }
                    sampleButton.Yes.GetComponent<Button>().onClick.AddListener(() => ShowRedeemPrizeCodePanel(int.Parse(sampleButton.CouponId.text)));

                    if (item.Status)
                    {
                        sampleButton.Yes.SetActive(true);
                    }
                    else
                    {
                        sampleButton.No.SetActive(true);
                    }

                    newButton.transform.SetParent(BagPanel.transform, false);
                }
            }
        }
        else
        {
            RegisterEventButton.SetActive(true);
            foreach (Transform child in ActivityPanel.transform)
            {
                if (child.childCount >= 4)
                {
                    GameObject childObject = child.GetChild(4).gameObject;
                    if (childObject != null)
                    {
                        childObject.GetComponent<Button>().interactable = false;
                    }
                }
               
                
            }
            foreach (Transform child in PrizePanel.transform)
            {
               child.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    #endregion

    public void Refresh()
    {
        LoadUserInformation();

        //Clear Prize List
        foreach (Transform child in PrizePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GetPrizeData();

        //Clear PrizeCode List
        foreach (Transform child in BagPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadPrizeCode();

        //Clear Activity List
        foreach (Transform child in ActivityPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadActivities();
        foreach (Transform child in InformationPanel.transform)
        {
            if (child.GetSiblingIndex() > 4) {
                GameObject.Destroy(child.gameObject);
            }
            
        }
        LoadStores();
    }

    public void LoadPreviouseScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        MySceneManager.loadPreviousScene();
    }

    void ShowRedeemPrizeCodePanel(int PrizeCodeId)
    {
        MasterCodeText.text = "";
        this.PrizeCodeId = PrizeCodeId;
        RedeemCodePanel.SetActive(true);
    }

    public void RedeemPrizeCode()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        if (MasterCodeText.text.Length > 0)
        {
            if (PrizeCodeId != 0)
            {
                //SEND REDEEM PRIZE CODE REQUEST
                HTTPRequest request = new HTTPRequest();
                WWWForm form = new WWWForm();
                form.AddField("prizeCodeId", PrizeCodeId);
                form.AddField("masterCode", MasterCodeText.text);
                request.url = ConstantClass.API_RedeemPrizeCode;
                request.formData = form;

                request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallRedeemPrizeCodeRequest);
                request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
                request.onError = new EventHandlerServiceError(MessageHelper.OnError);

                UCSS.HTTP.PostForm(request);
            }
            else
            {
                LoadingManager.hideLoadingIndicator(loadingPanel);
                MessageHelper.ErrorDialog("ERROR", "Cannot get prize ID");
            }
        }
        else
        {
            LoadingManager.hideLoadingIndicator(loadingPanel);
            MessageHelper.ErrorDialog("Please enter master code");
        }
    }

    #region PROCESS REDEEM PRIZE CODE REQUEST
    private void OnDoneCallRedeemPrizeCodeRequest(string result, string transactionId)
    {
        ResponseModel<PrizeCodeModel> jsonResponse = new ResponseModel<PrizeCodeModel>();
        jsonResponse.Data = new PrizeCodeModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<PrizeCodeModel>>(result);

        if (jsonResponse.Succeed)
        {

            //Clear PrizeCodeList
            foreach (Transform child in BagPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            //Load Prize Code Again
            LoadPrizeCode();

            //Show redeem prize code success panel
            RedeemPrizeCodeSuccessPanel.SetActive(true);
            RedeemPrizeCodeSuccessMessage.text = "You got " + jsonResponse.Data.PrizeName + " from " + PlayerPrefs.GetString(ConstantClass.PP_OrganizerName);
            StartCountTime = true;
        }
        else
        {
            //Show message
            MessageHelper.ErrorDialog(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    #endregion

    #region PROCESS REDEEM PRIZE REQUEST
    public void ShowRedeemPrizePanel(RewardButtonTemplate RewardObject)
    {
        Debug.Log(RewardObject.transform.GetChild(0).GetComponent<Text>().text);
        try
        {
            int PrizeIdTemp = int.Parse(RewardObject.transform.GetChild(0).GetComponent<Text>().text);
            int Teek;
            int Ruby;
            int Sapphire;
            int Citrine;

            string TeekText = RewardObject.Coin.GetComponentInChildren<Text>().text;
            string RubyText = RewardObject.Gem.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text;
            string SapphireText = RewardObject.Gem.transform.GetChild(2).gameObject.GetComponentInChildren<Text>().text;
            string CitrineText = RewardObject.Gem.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text;

            //Get teek value
            if (TeekText != null && TeekText.Length > 0)
            {
                PrizeTeek.text = TeekText;
                Teek = int.Parse(TeekText);
            }
            else
            {
                PrizeTeek.text = "0";
                Teek = 0;
            }

            //Get Ruby value
            if (RubyText != null && RubyText.Length > 0)
            {
                PrizeRuby.text = RubyText;
                Ruby = int.Parse(RubyText);
            }
            else
            {
                PrizeRuby.text = "0";
                Ruby = 0;
            }

            //Get Saphhire value
            if (SapphireText != null && SapphireText.Length > 0)
            {
                PrizeSapphire.text = SapphireText;
                Sapphire = int.Parse(SapphireText);
            }
            else
            {
                PrizeSapphire.text = "0";
                Sapphire = 0;
            }

            //Get Citrine value
            if (CitrineText != null && CitrineText.Length > 0)
            {
                PrizeCitrine.text = CitrineText;
                Citrine = int.Parse(CitrineText);
            }
            else
            {
                PrizeCitrine.text = "0";
                Citrine = 0;
            }

            this.PrizeId = PrizeIdTemp;
            PrizeName.text = RewardObject.PrizeName.text;
            PrizeImage.sprite = RewardObject.PrizeImage.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;

            ClaimByTeekButton.gameObject.SetActive(false);
            OrLabel.gameObject.SetActive(false);
            ClaimByGemButton.gameObject.SetActive(false);

            if (Teek > 0 && (Ruby  !=0 || Sapphire != 0 || Citrine != 0)) //If only can claim by Teek, disable Gem Select Button
            {
                ClaimByTeekButton.gameObject.SetActive(true);
                OrLabel.gameObject.SetActive(true);
                ClaimByGemButton.gameObject.SetActive(true);
            } else if (Teek <=0 && (Ruby != 0 || Sapphire != 0 || Citrine != 0))
            {
                ClaimByTeekButton.gameObject.SetActive(false);
                ClaimByGemButton.gameObject.SetActive(true);
            } else if (Teek > 0 && (Ruby == 0 && Sapphire == 0 && Citrine == 0))
            {
                ClaimByGemButton.gameObject.SetActive(false);
                ClaimByTeekButton.gameObject.SetActive(true);
            } 
            
        }
        catch (Exception e)
        {
            Debug.Log("Parse error in EventDetailScript with message: " + e.Message);
        }
        //If prize can redeem by both Teek and Gem, default select is Teek

        RedeemPrizePanel.SetActive(true);
    }

    public void RedeemPrizeUsingGem()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Prepare object to create request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("prizeId", PrizeId);
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));
        form.AddField("eventId", PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));

        request.formData = form;
        Debug.Log("Redeem by gem");
        request.url = ConstantClass.API_RedeemPrizeByGem;
        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallRedeemPrizeByGem);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    public void RedeemPrizeUsingTeek()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Prepare object to create request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("prizeId", PrizeId);
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));
        form.AddField("eventId", PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));

        request.formData = form;

        Debug.Log("Redeem by teek");
        request.url = ConstantClass.API_RedeemPrizeByTeek;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallRedeemPrizeByTeek);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);

    }

    private void OnDoneCallRedeemPrizeByTeek(string result, string transactionId)
    {
        ResponseModel<PrizeCodeModel> jsonResponse = new ResponseModel<PrizeCodeModel>();
        jsonResponse.Data = new PrizeCodeModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<PrizeCodeModel>>(result);

        if (jsonResponse.Succeed)
        {
            Refresh();
            MessageHelper.SuccessDialog("Claim prize successfully");
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
            Debug.Log(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    private void OnDoneCallRedeemPrizeByGem(string result, string transactionId)
    {
        ResponseModel<PrizeCodeModel> jsonResponse = new ResponseModel<PrizeCodeModel>();
        jsonResponse.Data = new PrizeCodeModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<PrizeCodeModel>>(result);

        if (jsonResponse.Succeed)
        {
            Refresh();
            MessageHelper.SuccessDialog("Claim prize successfully");
        }
        else
        {
            MessageHelper.ErrorDialog(jsonResponse.Message);
            Debug.Log(jsonResponse.Message);
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion

    public void ResetTimeCountForRedeemPrizeCode()
    {
        StartCountTime = false;
        TimeOutRedeemPrizeCodeSuccessfully = 5f * 60;
    }

    public void RegisterEvent()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_RegisterEvent;

        WWWForm form = new WWWForm();

        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("eventId", EventId);

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneRegisterEvent);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    private void OnDoneRegisterEvent(string result, string transactionId)
    {
        ResponseModel<String> jsonResponse = new ResponseModel<String>();
        jsonResponse.Data = "";
        jsonResponse = JsonMapper.ToObject<ResponseModel<String>>(result);
        if (jsonResponse.Succeed)
        {
            MessageHelper.SuccessDialog("Register successfully!!");
            RegisterEventButton.SetActive(false);
        }
        else
        {
            MessageHelper.ErrorDialog("Register failed!!");
        }
        Refresh();


        LoadingManager.hideLoadingIndicator(loadingPanel);
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
