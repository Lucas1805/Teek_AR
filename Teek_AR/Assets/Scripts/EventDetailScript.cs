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

[System.Serializable]
public class ItemTemplate
{
    public string name;
    public int coin;
    public int ruby;
    public int emerald;
    public int sapphire;
}

[System.Serializable]
public class CouponTemplate
{
    public string name;
    public bool isActive;
}

public class EventDetailScript : MonoBehaviour
{
    public GameObject ButtonTemplate;
    public GameObject CouponTemplate;

    public List<CouponTemplate> listCoupon;
    public static int eventId = 101;
    public int OrganizerId = 39;

    public GameObject loadingPanel;
    public GameObject contentPanel;
    public GameObject couponPanel;
    public Text TeekAmountText;
    public Text RubyAmountText;
    public Text SapphireAmountText;
    public Text CitrineAmountText;

    // Use this for initialization
    void Start()
    {
        //Get Organizer Id from PlayerPrefs
        OrganizerId = PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId);

        PlayerPrefs.SetString(ConstantClass.PP_UserIDKey, Encrypt.EncryptString("40efe638-04b6-42aa-81c3-a79b208d75e5"));
        PlayerPrefs.SetInt(ConstantClass.PP_OrganizerId, 39);
        PlayerPrefs.Save();

        LoadUserInformation();
        GetPrizeData();
        PopulateCoupon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopulateCoupon()
    {
        foreach (var item in listCoupon)
        {
            GameObject newButton = Instantiate(CouponTemplate) as GameObject;
            CouponButtonTemplate sampleButton = newButton.GetComponent<CouponButtonTemplate>();
            sampleButton.PrizeName.text = item.name;
            if (item.isActive)
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
    public void GetPrizeData()
    {

        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadPrizeList + "?eventId=" + eventId;

        request.stringCallback = new EventHandlerHTTPString(this.PopulateDataList);

        UCSS.HTTP.GetString(request);
    }
    private void PopulateDataList(string result, string transactionId)
    {
        ResponseModel<List<PrizeResponseModel>> jsonResponse = new ResponseModel<List<PrizeResponseModel>>();
        jsonResponse.Data = new List<PrizeResponseModel>();
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<PrizeResponseModel>>>(result);
        LoadingManager.hideLoadingIndicator(loadingPanel);
        if (jsonResponse.Succeed)
        {
            foreach (var item in jsonResponse.Data)
            {
                GameObject newButton = Instantiate(ButtonTemplate) as GameObject;
                RewardButtonTemplate sampleButton = newButton.GetComponent<RewardButtonTemplate>();
                sampleButton.PrizeName.text = item.Name;
                if (item.Ruby != 0)
                {
                    sampleButton.Gem.transform.GetChild(0).gameObject.SetActive(true);
                    sampleButton.Gem.GetComponentInChildren<Text>().text = item.Ruby.ToString();
                }
                if (item.Sapphire != 0)
                {
                    sampleButton.Gem.transform.GetChild(2).gameObject.SetActive(true);
                    sampleButton.Gem.GetComponentInChildren<Text>().text = item.Sapphire.ToString();
                }
                if (item.Citrine != 0)
                {
                    sampleButton.Gem.transform.GetChild(1).gameObject.SetActive(true);
                    sampleButton.Gem.GetComponentInChildren<Text>().text = item.Citrine.ToString();
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
        LoadingManager.showLoadingIndicator(loadingPanel);
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
            LoadingManager.hideLoadingIndicator(loadingPanel);
            MessageHelper.MessageDialog(jsonResponse.Message);
        }
    }

    private void OnLoadUserInformationError(string error, string transactionId)
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
}
