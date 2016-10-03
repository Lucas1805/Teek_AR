using UnityEngine;
using System.Collections;
using MobyShop;
using MobyShop.UI;
using UnityEngine.SceneManagement;
using Assets;
using LitJson;

public class ShopController : MonoBehaviour {
    public ShopUIBase mobyShop;

    //private const int FIREBALL_DEFAULT_NUMBER = 20;
    private const string url = "http://localhost/Teek/api/";
    private readonly string username;

	// Use this for initialization
	void Start () {
        //GET USERNAME FROM LOGINSCENE TO LOAD PLAYER INFORMATION
        //username = ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void showShop()
    {
        mobyShop.Show(0,null);
    }

    //public void setDefault()
    //{
    //    //Set number o fireball of account to Default Value
    //    ProductInfo product = Shop.GetProduct("fireball");
    //    if (product != null)
    //    {
    //        product.Value = FIREBALL_DEFAULT_NUMBER;
    //    }
    //    else Debug.Log("Cannot get product object. Please check for product ID");

    //}

    public void loadRedeemCodeScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.RedeemCodeSceneName);
    }

    public void loadHomeScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
    }

    public void loadInventoryScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.InventorySceneName);
    }

    /// <summary>
    /// Used this to load all information of Player in that Event andset to PlayerPref (Shop item, coins, etc...) before playing.
    /// </summary>
    private void loadAllInfo()
    {
        //Create object to sen Http Request
        WWWForm form = new WWWForm();
        form.AddField("Username", username);

        //SEND POST REQUEST

        WWW www = new WWW(url, form);

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
