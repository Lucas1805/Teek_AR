using UnityEngine;
using System.Collections;
using MobyShop;
using MobyShop.UI;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour {
    public ShopUIBase mobyShop;
    
    private const int FIREBALL_DEFAULT_NUMBER = 20;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void showShop()
    {
        mobyShop.Show(0,null);
    }

    public void setDefault()
    {
        //Set number o fireball of account to Default Value
        ProductInfo product = Shop.GetProduct("fireball");
        if (product != null)
        {
            product.Value = FIREBALL_DEFAULT_NUMBER;
        }
        else Debug.Log("Cannot get product object. Please check for product ID");

    }

    
}
