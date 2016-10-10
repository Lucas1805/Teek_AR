using UnityEngine;
using System.Collections;
using MobyShop;
using MobyShop.UI;
using UnityEngine.SceneManagement;
using Assets;
using LitJson;

public class ShopController : MonoBehaviour {

    public ShopUIBase mobyShop;
    public GameObject loadingPanel;

    //private const int FIREBALL_DEFAULT_NUMBER = 20;
    private readonly string username;

	// Use this for initialization
	void Start () {
        setDefaultForShop();
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
}
