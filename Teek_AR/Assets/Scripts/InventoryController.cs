using UnityEngine;
using System.Collections;
using Assets;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public GameObject loadingPanel;
    public InputField quantityInputField;

    private int quantity;

	// Use this for initialization
	void Start () {
        quantityInputField.text = "1";
        quantity = int.Parse(quantityInputField.text);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadPreviousScene()
    {
        MySceneManager.loadPreviousScene();
    }

    public void IncreaseQuantity()
    {
        quantity++;
        quantityInputField.text = quantity.ToString();
    }

    public void DecreaseQuantity()
    {
        quantity--;
        quantityInputField.text = quantity.ToString();
    }
}
