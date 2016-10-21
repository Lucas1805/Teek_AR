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

public class EventDetailScript : MonoBehaviour
{
    public GameObject ButtonTemplate;
    public GameObject CouponTemplate;
    
    public static int EventId;
    public static string EventName;
    public static Image EventImage;
    public int OrganizerId;

    public GameObject loadingPanel;
    public GameObject contentPanel;
    public GameObject couponPanel;
    public Text TeekAmountText;
    public Text RubyAmountText;
    public Text SapphireAmountText;
    public Text CitrineAmountText;
    public Text EventNameText;
    public Image EventImageObject;
    public GameObject PrizePanel;
    public GameObject PrizeCodePanel;
    public GameObject ActivityPanel;
    public GameObject RedeemCodePanel;
    public InputField MasterCodeText;
    public GameObject RedeemPrizePanel;
    public Toggle TeekToggleButton;
    public Toggle GemToggleButton;

    private int PrizeCodeId;

    // Use this for initialization
    void Start()
    {
        //Get Organizer Id from PlayerPrefs
        OrganizerId = PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId);

        //Set Eventid To PlayerPrefs
        PlayerPrefs.SetInt(ConstantClass.PP_EventIDKey, EventId);
        PlayerPrefs.Save();


        LoadUserInformation();
        LoadEventInfo();
        GetPrizeData();
        LoadPrizeCode();
    }

    public void GetPrizeData()
    {

        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadPrizeList + "?EventId=" + EventId;

        request.stringCallback = new EventHandlerHTTPString(this.PopulateDataList);

        UCSS.HTTP.GetString(request);
    }
    private void PopulateDataList(string result, string transactionId)
    {
        ResponseModel<List<PrizeResponseModel>> jsonResponse = new ResponseModel<List<PrizeResponseModel>>();
        jsonResponse.Data = new List<PrizeResponseModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<PrizeResponseModel>>>(result);
        MessageHelper.CloseDialog();
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
                    sampleButton.Gem.transform.GetChild(0).gameObject.SetActive(true);
                    sampleButton.Gem.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = item.Ruby.ToString();
                }
                if (item.Sapphire != 0)
                {
                    sampleButton.Gem.transform.GetChild(2).gameObject.SetActive(true);
                    sampleButton.Gem.transform.GetChild(2).gameObject.GetComponentInChildren<Text>().text = item.Sapphire.ToString();
                }
                if (item.Citrine != 0)
                {
                    sampleButton.Gem.transform.GetChild(1).gameObject.SetActive(true);
                    sampleButton.Gem.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text = item.Citrine.ToString();
                }
                if (item.Teek != 0)
                {
                    sampleButton.Coin.SetActive(true);
                    sampleButton.Coin.GetComponentInChildren<Text>().text = item.Teek.ToString();
                }



                newButton.transform.SetParent(contentPanel.transform, false);
            }
        }
        else
        {

        }

    }

    public void FilterCombo()
    {
        foreach (Transform childTransform in contentPanel.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            if (childTransform.GetChild(2).GetComponent<Text>().text.Equals("24"))
            {
                childGameObject.SetActive(false);
            }

        }

    }



    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(Assets.ConstantClass.GameSceneName);

    }

    public void LoadUserInformation()
    {
        MessageHelper.LoadingDialog("Loading data....");
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadUserInformation;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnLoadUserInformationRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnLoadUserInformationError);

        UCSS.HTTP.PostForm(request);
    }

    public void LoadPrizeCode()
    {
        MessageHelper.LoadingDialog("Loading data....");
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadPrizeCode;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("eventId", EventId);

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnLoadPrizeCodeRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnLoadPrizeCodeError);

        UCSS.HTTP.PostForm(request);
    }

    #region PROCESS LOAD USER INFORMATION REQUEST
    private void OnLoadUserInformationRequest(string result, string transactionId)
    {
        ResponseModel<List<CustomerResponseModel>> jsonResponse = new ResponseModel<List<CustomerResponseModel>>();
        jsonResponse.Data = new List<CustomerResponseModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<CustomerResponseModel>>>(result);

        if (jsonResponse.Succeed)
        {
            TeekAmountText.text = jsonResponse.Data[0].Teek.ToString() + " Teek";
            RubyAmountText.text = jsonResponse.Data[0].Ruby.ToString();
            SapphireAmountText.text = jsonResponse.Data[0].Sapphire.ToString();
            CitrineAmountText.text = jsonResponse.Data[0].Citrine.ToString();
        }
        else
        {
            //Show error message
            MessageHelper.CloseDialog();
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadUserInformationError(string error, string transactionId)
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

    void LoadEventInfo()
    {
        if(EventName != null && EventName.Length > 0)
        {
            EventNameText.text = Utils.TruncateLongString(EventName,23);
        }
        else
        {
            EventNameText.text = "";
        }
        
        if(EventImage != null)
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
        MessageHelper.CloseDialog();
        if (jsonResponse.Succeed)
        {
            if(jsonResponse.Data != null)
            {
                foreach (var item in jsonResponse.Data)
                {
                    GameObject newButton = Instantiate(CouponTemplate) as GameObject;
                    CouponButtonTemplate sampleButton = newButton.GetComponent<CouponButtonTemplate>();
                    sampleButton.PrizeName.text = item.PrizeName;
                    sampleButton.CouponId.text = item.Id.ToString();
                    sampleButton.Yes.GetComponent<Button>().onClick.AddListener(() => ShowRedeemPrizeCodePanel(int.Parse(sampleButton.CouponId.text)));

                    if (item.Status)
                    {
                        sampleButton.Yes.SetActive(true);
                    }
                    else
                    {
                        sampleButton.No.SetActive(true);
                    }

                    newButton.transform.SetParent(couponPanel.transform, false);
                }
            }
        }
        else
        {
            //Show error message
            
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadPrizeCodeError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);
        Debug.Log("WWW Error: " + error);
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
        foreach (Transform child in PrizeCodePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        LoadPrizeCode();

        ////Clear Activity List
        //foreach (Transform child in ActivityPanel.transform)
        //{
        //    GameObject.Destroy(child.gameObject);
        //}
    }

    public void LoadPreviouseScene()
    {
        
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
        MessageHelper.LoadingDialog("Loading...");

        if(MasterCodeText.text.Length > 0)
        {
            if(PrizeCodeId != 0)
            {
                //SEND REDEEM PRIZE CODE REQUEST
                HTTPRequest request = new HTTPRequest();
                WWWForm form = new WWWForm();
                form.AddField("prizeCodeId", PrizeCodeId);
                form.AddField("masterCode", MasterCodeText.text);
                request.url = ConstantClass.API_RedeemPrizeCode;
                request.formData = form;

                request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallRedeemPrizeCodeRequest);
                request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
                request.onError = new EventHandlerServiceError(this.OnRedeemPrizeCodeError);

                UCSS.HTTP.PostForm(request);
            }
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
            //Hide RedeemCode Panel
            RedeemCodePanel.SetActive(false);

            //Clear PrizeCodeList
            foreach (Transform child in PrizeCodePanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            //Load Prize Code Again
            LoadPrizeCode();
        }
        else
        {
            //Show message
            MessageHelper.CloseDialog();
            //Close redeem prize code panel
            RedeemCodePanel.SetActive(false);
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnRedeemPrizeCodeError(string error, string transactionId)
    {
        MessageHelper.CloseDialog();
        MessageHelper.MessageDialog(ConstantClass.Msg_ErrorTitle, error);
        Debug.Log("Login WWW error: " + error);
    }
    #endregion

    #region PROCESS REDEEM PRIZE REQUEST
    public void ShowRedeemPrizePanel(RewardButtonTemplate RewardObject)
    {
        Debug.Log(RewardObject.transform.GetChild(0).GetComponent<Text>().text);
        try
        {
            int PrizeId = int.Parse(RewardObject.transform.GetChild(0).GetComponent<Text>().text);
            int Teek = int.Parse(RewardObject.Coin.GetComponentInChildren<Text>().text);
            int Ruby = int.Parse(RewardObject.Gem.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text);
            int Sapphire = int.Parse(RewardObject.Gem.transform.GetChild(2).gameObject.GetComponentInChildren<Text>().text);
            int Citrine = int.Parse(RewardObject.Gem.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text);
            Debug.Log(Ruby);
            Debug.Log(Sapphire);
            Debug.Log(Citrine);
            Debug.Log(Teek);
        }
        catch(Exception e)
        {
            Debug.Log("Parse error in EventDetailScript with message: " + e.Message);
        }
        //If prize can redeem by both Teek and Gem, default select is Teek

        RedeemPrizePanel.SetActive(true);
    }
    #endregion
}
