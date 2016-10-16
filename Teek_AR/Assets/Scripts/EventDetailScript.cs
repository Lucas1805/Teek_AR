using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public List<ItemTemplate> listItem;
    public List<CouponTemplate> listCoupon;

    public GameObject contentPanel;
    public GameObject couponPanel;


    // Use this for initialization
    void Start()
    {
        PopulateDataList();
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
    public void PopulateDataList()
    {
        foreach (var item in listItem)
        {
            GameObject newButton = Instantiate(ButtonTemplate) as GameObject;
            RewardButtonTemplate sampleButton = newButton.GetComponent<RewardButtonTemplate>();
            sampleButton.PrizeName.text = item.name;
            if (item.ruby != 0)
            {
                sampleButton.Gem.transform.GetChild(0).gameObject.SetActive(true);
                sampleButton.Gem.GetComponentInChildren<Text>().text = item.ruby.ToString();
            }
            if (item.sapphire != 0)
            {
                sampleButton.Gem.transform.GetChild(2).gameObject.SetActive(true);
                sampleButton.Gem.GetComponentInChildren<Text>().text = item.sapphire.ToString();
            }
            if (item.emerald != 0)
            {
                sampleButton.Gem.transform.GetChild(1).gameObject.SetActive(true);
                sampleButton.Gem.GetComponentInChildren<Text>().text = item.emerald.ToString();
            }
            if (item.coin != 0)
            {
                sampleButton.Coin.SetActive(true);
                sampleButton.Coin.GetComponentInChildren<Text>().text = item.coin.ToString();
            }



            newButton.transform.SetParent(contentPanel.transform, false);
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
