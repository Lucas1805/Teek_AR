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

    //private const int FIREBALL_DEFAULT_NUMBER = 20;
    private int coinNumber = 666;
    private int fireballNumber = 13;
    private int iceballNumber = 13;

    private bool error = false; //Check if loading data from server before playing is error or not
    private ProductInfo justBoughtProduct;

    // Use this for initialization
    void Start () {

        //CALL AIP TO GET NUMBER OF ITEM OF USER TO LOAD
        //loadCoin();
        //loadFireball();

        //SET VALUE TO SHOP
        setValueToShop();
        Shop.OnProductBought += this.OnProductBought;
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


    [System.Serializable]
    public class JSONResponseObject
    {

        public bool Succeed { get; set; }

        public string Message { get; set; }

        public string Errors { get; set; }
        public Data Data { get; set; }

    }

    public class Data
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
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
        form.AddField("userParticipationID",PlayerPrefs.GetInt(ConstantClass.PP_UserParticipationID));
        form.AddField("productClass", product.ProductClass);
        form.AddField("amount", product.IncrementOnBuy);
        form.AddField("price", product.price);

        request.url = ConstantClass.API_BuyItem;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallBuyItemRequest);
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

    void loadTeek()
    {
        //Create object to sen Http Request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));
        form.AddField("OrganizerId", ""); 
        request.url = ConstantClass.API_LoadTeek;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadTeekRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnLoadTeekError);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }

    void loadCoin()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //Create object to sen Http Request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));
        request.url = ConstantClass.API_LoadCoins;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadCoinRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnLoadCoinError);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }

    void loadFireball()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //Create object to sen Http Request
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));
        request.url = ConstantClass.API_LoadCoins;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallLoadFireballRequest);
        request.onTimeOut = new EventHandlerServiceTimeOut(this.OnTimeOut);
        request.onError = new EventHandlerServiceError(this.OnLoadFireballError);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }
    
    /// <summary>
    /// Used everytimeplayer buy an item 
    /// </summary>
    void useFireball()
    {
        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));
        form.AddField("EventId", PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));

        //SEND POST REQUEST
    }

    #region PROCESS BUY ITEM REQUEST
    private void OnDoneCallBuyItemRequest(string result, string transactionId)
    {
        ResponseModel<LoadCoinModel> jsonResponse = new ResponseModel<LoadCoinModel>();
        jsonResponse.Data = new LoadCoinModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<LoadCoinModel>>(result);

        if (!jsonResponse.Succeed)
        {
            restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);
            //showLoginMessage(www.error);
            Debug.Log(error);
        }
        
    }

    private void OnBuyItemError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);

        //RESTORE PURCHASE
        restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);

        Debug.Log(error);
        
    }
    #endregion
    
    #region PROCESS LOAD COIN REQUEST
    private void OnDoneCallLoadCoinRequest(string result, string transactionId)
    {
        ResponseModel<LoadCoinModel> jsonResponse = new ResponseModel<LoadCoinModel>();
        jsonResponse.Data = new LoadCoinModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<LoadCoinModel>>(result);

        if (jsonResponse.Succeed)
        {
            coinNumber = jsonResponse.Data.Coin;
        }
        else
        {
            MessageHelper.MessageDialog("Cannot Load Coin");
        }
        
    }

    private void OnLoadCoinError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);
        Debug.Log(error);
        
    }
    #endregion

    #region PROCESS LOAD FIREBALL REQUEST
    private void OnDoneCallLoadFireballRequest(string result, string transactionId)
    {
        ResponseModel<LoadFireballModel> jsonResponse = new ResponseModel<LoadFireballModel>();
        jsonResponse.Data = new LoadFireballModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<LoadFireballModel>>(result);

        if (jsonResponse.Succeed)
        {
            fireballNumber = jsonResponse.Data.Fireball;
        }
        else
        {
            MessageHelper.MessageDialog("Cannot Load Fireball");
        }
        
    }

    private void OnLoadFireballError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);
        Debug.Log(error);
        
    }
    #endregion

    #region PROCESS LOAD TEEK REQUEST
    private void OnDoneCallLoadTeekRequest(string result, string transactionId)
    {
        ResponseModel<LoadTeekModel> jsonResponse = new ResponseModel<LoadTeekModel>();
        jsonResponse.Data = new LoadTeekModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<LoadTeekModel>>(result);

        if (jsonResponse.Succeed)
        {
            coinNumber = jsonResponse.Data.Teek;
        }
        else
        {
            MessageHelper.MessageDialog("Cannot Load Teek");
        }
        
    }

    private void OnLoadTeekError(string error, string transactionId)
    {
        MessageHelper.MessageDialog(error);
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
}
