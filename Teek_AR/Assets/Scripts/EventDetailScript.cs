﻿using UnityEngine;
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
    public Sprite icon;
}

public class EventDetailScript : MonoBehaviour
{
    public GameObject ButtonTemplate;
    public List<ItemTemplate> listItem;

    public Transform contentPanel;

   
    // Use this for initialization
    void Start()
    {
        PopulateDataList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopulateDataList()
    {
        foreach (var item in listItem)
        {
            GameObject newButton = Instantiate(ButtonTemplate) as GameObject;
            RewardButtonTemplate sampleButton = newButton.GetComponent<RewardButtonTemplate>();
            sampleButton.PrizeName.text = item.name;
            sampleButton.CoinImage.sprite = item.icon;
            sampleButton.CoinAmount.text = item.coin.ToString();
            newButton.transform.SetParent (contentPanel,false);
        }
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(Assets.ConstantClass.GameSceneName);
       
    }
}
