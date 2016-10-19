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
    public static int eventId =101;

    public GameObject loadingPanel;
    public GameObject contentPanel;
    public GameObject couponPanel;


    // Use this for initialization
    void Start()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
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
            } else
            {
                sampleButton.No.SetActive(true);
            }



            newButton.transform.SetParent(couponPanel.transform, false);
        }
    }
    public void GetPrizeData()
    {

        HTTPRequest request = new HTTPRequest();
        request.url = "http://10.5.50.21/Teek/api/prize/GetPrizes?eventId=" + eventId;

        request.stringCallback = new EventHandlerHTTPString(this.PopulateDataList);

        UCSS.HTTP.GetString(request);
    }
    private void PopulateDataList(string result,string transactionId)
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
}
