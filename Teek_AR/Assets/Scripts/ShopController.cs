using UnityEngine;
using System.Collections;
using MobyShop;
using MobyShop.UI;
using UnityEngine.SceneManagement;
using Assets;
using LitJson;
using Assets.ResponseModels;

public class ShopController : MonoBehaviour {

    public ShopUIBase mobyShop;
    public GameObject loadingPanel;
    public GameObject menuGroupButtonPanel;

    //private const int FIREBALL_DEFAULT_NUMBER = 20;
    private int coinNumber = 5000;
    private int fireballNumber = 0;

    private bool error = false; //Check if loading data from server before playing is error or not

    // Use this for initialization
    void Start () {

        //CALL AIP TO GET NUMBER OF ITEM OF USER TO LOAD
        //loadCoins();
        //loadItem();

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

        //SET VALUE
        fireball.Value = fireballNumber;
        coin.Value = coinNumber;

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
        

        //CALL API TO UPDATE VALUE OF PRODUCT IN SERVER DATABASE
        //Create object to send Http Request
        WWWForm form = new WWWForm();
        form.AddField("userParticipationID",PlayerPrefs.GetInt(ConstantClass.PP_UserParticipationID));
        form.AddField("productClass", product.ProductClass);
        form.AddField("amount", product.IncrementOnBuy);
        form.AddField("price", product.price);

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_BuyItem, form);

        StartCoroutine(BuyItemRequest(www, product));
    }

    IEnumerator BuyItemRequest(WWW www, MobyShop.ProductInfo product)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<UpdateShopItemModel> jsonResponse = new ResponseModel<UpdateShopItemModel>();
                jsonResponse.Data = new UpdateShopItemModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<UpdateShopItemModel>>(www.text);

                if (jsonResponse.Succeed)
                {
                    LoadingManager.hideLoadingIndicator(loadingPanel);
                }
                else
                {
                    //RESTORE PURCHASE
                    restorePurchase(product, product.IncrementOnBuy);
                    LoadingManager.hideLoadingIndicator(loadingPanel);
                    //Show error message
                    //showLoginMessage(jsonResponse.Message);
                }
            }
            else {
                //RESTORE PURCHASE
                restorePurchase(product, product.IncrementOnBuy);
                LoadingManager.hideLoadingIndicator(loadingPanel);
                //showLoginMessage(www.error);
                Debug.Log("WWW Error: " + www.error);
            }
        }

    }

    private void restorePurchase(ProductInfo product, int deviation)
    {
        //Restore product value
        product.Value = product.Value - deviation;

        //Restore coin value
        Shop.GetProduct(ConstantClass.CoinItemID).Value = Shop.GetProduct(ConstantClass.CoinItemID).Value + product.price;
    }

    public void showHideMenuGroup()
    {
        if (menuGroupButtonPanel.active)
            menuGroupButtonPanel.SetActive(false);
        else
            menuGroupButtonPanel.SetActive(true);        
    }

    void loadCoins()
    {
        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_LoadCoins, form);

        StartCoroutine(LoadCoinRequest(www));
    }

    void loadFireball()
    {
        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("UserId", Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey))));
        form.AddField("EventId",PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_LoadFireball, form);

        StartCoroutine(LoadFireballRequest(www));
    }

    IEnumerator LoadCoinRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<LoadCoinModel> jsonResponse = new ResponseModel<LoadCoinModel>();
                jsonResponse.Data = new LoadCoinModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<LoadCoinModel>>(www.text);

                if (jsonResponse.Succeed)
                {
                    coinNumber = jsonResponse.Data.Coin;
                }
            }
            else {
                Debug.Log("WWW Error: " + www.error);
            }
        }
    }
    IEnumerator LoadFireballRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<LoadFireballModel> jsonResponse = new ResponseModel<LoadFireballModel>();
                jsonResponse.Data = new LoadFireballModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<LoadFireballModel>>(www.text);

                if (jsonResponse.Succeed)
                {
                    fireballNumber = jsonResponse.Data.Fireball;
                }
            }
            else {
                Debug.Log("WWW Error: " + www.error);
            }
        }
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

        WWW www = new WWW("", form);

        StartCoroutine(UpdateItemToDatabaseRequest(www));
    }

    IEnumerator UpdateItemToDatabaseRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<LoadCoinModel> jsonResponse = new ResponseModel<LoadCoinModel>();
                jsonResponse.Data = new LoadCoinModel();
                jsonResponse = JsonMapper.ToObject<ResponseModel<LoadCoinModel>>(www.text);

                if (!jsonResponse.Succeed)
                {
                    Debug.Log(jsonResponse.Message);
                }
            }
            else {
                //showLoginMessage(www.error);
                Debug.Log("WWW Error: " + www.error);
            }
        }
    }

    #region LOAD OTHER SCENE

    public void loadRedeemCodeScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.RedeemCodeSceneName);
    }

    public void loadHomeScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
    }

    public void loadInventoryScene()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //SET LAST SCENE VALUE BEFORE LOAD NEXT SCENE
        MySceneManager.setLastScene(ConstantClass.GameSceneName);

        SceneManager.LoadSceneAsync(ConstantClass.InventorySceneName);
    }
    #endregion
}
