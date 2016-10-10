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

    //private const int FIREBALL_DEFAULT_NUMBER = 20;
    private readonly string username;
    private string productID;
    private int deviation; //Used to restore purchase when calling API to update data on server fail

    // Use this for initialization
    void Start () {
        setDefaultForShop();
        Shop.OnProductBought += this.OnProductBought;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void showShop()
    {
        mobyShop.Show(0,null);
    }

    public void setDefaultForShop()
    {
        Shop.ClearAllPurchaseData();

        ProductInfo fireball = Shop.GetProduct(ConstantClass.FireBallItemID);
        ProductInfo coin = Shop.GetProduct(ConstantClass.CoinItemID);

        //SET VALUE
        fireball.Value = 10;
        coin.Value = 150;

    }

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

    /// <summary>
    /// Used this to load all information of Player in that Event andset to PlayerPref (Shop item, coins, etc...) before playing.
    /// </summary>
    private void loadAllInfo()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);

        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("Username", username);

        //SEND POST REQUEST

        WWW www = new WWW("", form);

        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                JSONResponseObject jsonResponse = new JSONResponseObject();


                jsonResponse = JsonMapper.ToObject<JSONResponseObject>(www.text);

                if (jsonResponse.Succeed)
                {
                    //SET ALL INFO TO PLAYERPREFS
                }
                else
                {
                    //Show error message
                    //disableLoadinIndicator();
                    //showMessage(jsonResponse.Message);
                }
            }
            else {
                //showMessage(www.error);
                Debug.Log("WWW Error: " + www.error);
            }
        }

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

        productID = product.ProductId;
        deviation = product.IncrementOnBuy; //Used to restore purchase when calling API to update data on server fail
        

        //CALL API TO UPDATE VALUE OF PRODUCT IN SERVER DATABASE
        //Create object to send Http Request
        WWWForm form = new WWWForm();
        form.AddField("productID", productID);
        form.AddField("deviation", deviation);

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_UpdateShopItem, form);

        StartCoroutine(UpdateShopItem(www, product));
    }

    IEnumerator UpdateShopItem(WWW www, MobyShop.ProductInfo product)
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
                    restorePurchase(product, deviation);
                    LoadingManager.hideLoadingIndicator(loadingPanel);
                    //Show error message
                    //showLoginMessage(jsonResponse.Message);
                }
            }
            else {
                //RESTORE PURCHASE
                restorePurchase(product, deviation);
                LoadingManager.hideLoadingIndicator(loadingPanel);
                //showLoginMessage(www.error);
                Debug.Log("WWW Error: " + www.error);
            }
        }

    }

    private void restorePurchase(ProductInfo product, int deviation)
    {
        product.Value = product.Value - deviation;
    }
}
