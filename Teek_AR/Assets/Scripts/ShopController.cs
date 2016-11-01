using UnityEngine;
using System.Collections;
using MobyShop;
using MobyShop.UI;
using UnityEngine.SceneManagement;
using Assets;
using LitJson;
using Assets.ResponseModels;
using Ucss;

public class ShopController : MonoBehaviour {

    public ShopUIBase mobyShop;
    public GameObject loadingPanel;
    public GameObject menuGroupButtonPanel;
    public static int GameId;
    
    private int coinNumber = 666;
    private int fireballNumber = 13;
    private int iceballNumber = 13;
    
    private ProductInfo justBoughtProduct;

    // Use this for initialization
    void Start () {
        
        //CALL AIP TO GET NUMBER OF ITEM OF USER TO LOAD
        LoadCustomerInformation();
        LoadDropRate();

        /*
        --------WARNING---------
        The code below add a listener to shop. Whenever a product is bought it will run ALL functions that has been add to it
        To prevent the potention bug when use play game -> buy a product -> back to home -> play another game and buy product -> user must pay twice the price
        because the function has been add 2 times

        So we must check if it's been add into it or not. If NOT (== null) then we add
        */
        if(Shop.OnProductBought == null)
        {
            Shop.OnProductBought += this.OnProductBought;
        }

    }
	
	// Update is called once per frame
	void Update () {

	}

    public void showShop()
    {
        mobyShop.Show(0,null);
    }

    public void setValueToShop()
    {
        Shop.ClearAllPurchaseData();

        ProductInfo fireball = Shop.GetProduct(ConstantClass.FireBallItemID);
        ProductInfo coin = Shop.GetProduct(ConstantClass.CoinItemID);
        ProductInfo iceball = Shop.GetProduct(ConstantClass.IceBallItemID);

        //SET VALUE
        fireball.Value = fireballNumber;
        coin.Value = coinNumber;
        iceball.Value = iceballNumber;

    }
        
    void OnProductBought( MobyShop.BoughtOrRestored state, MobyShop.ProductInfo product, int amount)
    {
        //Show loading
        LoadingManager.showLoadingIndicator(loadingPanel);
        justBoughtProduct = product;

        //CALL API TO UPDATE VALUE OF PRODUCT IN SERVER DATABASE
        //Create object to send Http Request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        
        if (product.ProductClass.Equals(ConstantClass.CoinItemClassName))
        {
            form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
            form.AddField("coinAmount", product.IncrementOnBuy);

            request.url = ConstantClass.API_UpdateCoinItem;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallBuyCoinRequest);
        }
        else
        {
            form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
            form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));
            form.AddField("eventId", PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));
            form.AddField("price", product.price);

            request.url = ConstantClass.API_UpdateBallItem;

            if (product.ProductClass.Equals(ConstantClass.FireBallItemClassName)) //If player buy Fireball
            {
                form.AddField("fireballAmount", product.IncrementOnBuy);
                form.AddField("iceballAmount", 0);
            }

            if (product.ProductClass.Equals(ConstantClass.IceBallItemClassName)) //If player buy Iceball
            {
                form.AddField("fireballAmount", 0);
                form.AddField("iceballAmount", product.IncrementOnBuy);
            }

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallBuyItemRequest);
            
        }

        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnBuyItemError);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }
    
    private void restorePurchase(ProductInfo product, int deviation)
    {
        //Restore product value
        product.Value = product.Value - deviation;

        //Restore teek value
        Shop.GetProduct(ConstantClass.CoinItemID).Value = Shop.GetProduct(ConstantClass.CoinItemID).Value + product.price;
    }

    public void showHideMenuGroup()
    {
        if (menuGroupButtonPanel.active)
            menuGroupButtonPanel.SetActive(false);
        else
            menuGroupButtonPanel.SetActive(true);        
    }

    

    #region PROCESS BUY ITEM REQUEST
    private void OnDoneCallBuyItemRequest(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            coinNumber = (int) jsonResponse.Data.Coin;
            fireballNumber = (int)jsonResponse.Data.Fireball;
            iceballNumber = (int)jsonResponse.Data.Iceball;

            setValueToShop();

        }
        else
        {
            restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);
            MessageHelper.MessageDialog("Error", jsonResponse.Message);
            Debug.Log(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);        
    }

    private void OnDoneCallBuyCoinRequest(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            coinNumber = (int) jsonResponse.Data.Coin;
            setValueToShop();
        }
        else
        {
            restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);
            MessageHelper.MessageDialog("Error", jsonResponse.Message);
            Debug.Log(jsonResponse.Message);
        }
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    private void OnBuyItemError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);

        //RESTORE PURCHASE
        restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);

        Debug.Log(error);
        
    }
    #endregion
    
 

    private void OnTimeOut(string transactionId)
    {
        //showLoginMessage(ConstantClass.Msg_TimeOut);
        MessageHelper.MessageDialog(ConstantClass.Msg_TimeOut);
        Debug.Log(ConstantClass.Msg_TimeOut);
        
    }

    #region LOAD OTHER SCENE

    public void loadRedeemCodeScene()
    {
        
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.RedeemCodeSceneName);
    }

    public void loadHomeScene()
    {
        
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
    }

    public void loadInventoryScene()
    {
        
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.InventorySceneName);
    }
    #endregion

    public void LoadCustomerInformation()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        HTTPRequest request = new HTTPRequest();
        request.url = ConstantClass.API_LoadCustomerInformation;

        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));

        request.formData = form;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCustomerInformationRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);

        UCSS.HTTP.PostForm(request);
    }

    public void LoadDropRate()
    {
        if(GameId != 0)
        {
            LoadingManager.showLoadingIndicator(loadingPanel);
            HTTPRequest request = new HTTPRequest();
            request.url = ConstantClass.API_LoadDropRate;

            WWWForm form = new WWWForm();
            form.AddField("gameid", GameId);

            request.formData = form;

            request.stringCallback = new EventHandlerHTTPString(this.OnDoneLoadDropRateRequest);
            request.onTimeOut = new EventHandlerServiceTimeOut(this.SetDefaultDropRate);
            request.onError = new EventHandlerServiceError(this.SetDefaultDropRate);

            UCSS.HTTP.PostForm(request);
        }
    }

    #region PROCESS LOAD USER INFORMATION REQUEST
    private void OnDoneCustomerInformationRequest(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            if(jsonResponse.Data.Coin != null)
            {
                coinNumber = (int)jsonResponse.Data.Coin;
            }
            if (jsonResponse.Data.Fireball != null)
            {
                fireballNumber = (int) jsonResponse.Data.Fireball;
            }
            if (jsonResponse.Data.Iceball != null)
            {
                iceballNumber = (int)jsonResponse.Data.Iceball;
            }
        }
        else
        {
            //Show error message
            MessageHelper.MessageDialog(jsonResponse.Message);
        }

        //Call Set Value To Shop After Getting Data From Server
        setValueToShop();
        LoadingManager.hideLoadingIndicator(loadingPanel);
    }
    #endregion

    #region PROCESS LOAD DROP RATE REQUEST
    private void OnDoneLoadDropRateRequest(string result, string transactionId)
    {
        ResponseModel<DropRateModel> jsonResponse = new ResponseModel<DropRateModel>();
        jsonResponse.Data = new DropRateModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<DropRateModel>>(result);

        if (jsonResponse.Succeed)
        {
            if(jsonResponse.Data != null)
            {
                DragAndThrow.DropRateCombo1 = (float)jsonResponse.Data.DropRateCombo1;
                DragAndThrow.DropRateCombo2 = (float)jsonResponse.Data.DropRateCombo2;
                DragAndThrow.DropRateCombo3 = (float)jsonResponse.Data.DropRateCombo3;
            }
            else
            {
                DragAndThrow.DropRateCombo1 = 70f;
                DragAndThrow.DropRateCombo2 = 20f;
                DragAndThrow.DropRateCombo3 = 10f;
            }
        }
        else
        {
            DragAndThrow.DropRateCombo1 = 70f;
            DragAndThrow.DropRateCombo2 = 20f;
            DragAndThrow.DropRateCombo3 = 10f;
        }

        LoadingManager.hideLoadingIndicator(loadingPanel);
    }

    public void SetDefaultDropRate(string error, string transactionId)
    {
        LoadingManager.hideLoadingIndicator(loadingPanel);
        DragAndThrow.DropRateCombo1 = 70f;
        DragAndThrow.DropRateCombo2 = 20f;
        DragAndThrow.DropRateCombo3 = 10f;
        Debug.Log("WWW Error: " + error + ".Set drop rate to default");
    }
    public void SetDefaultDropRate(string transactionId)
    {
        LoadingManager.hideLoadingIndicator(loadingPanel);
        DragAndThrow.DropRateCombo1 = 70f;
        DragAndThrow.DropRateCombo2 = 20f;
        DragAndThrow.DropRateCombo3 = 10f;
        Debug.Log("WWW Error: Time out.Set drop rate to default");
    }
    #endregion
}
