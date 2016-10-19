using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour {
    public void NavigateStoreEventScene(Text StoreId)
    {
        StoreEventController.StoreId = int.Parse(StoreId.text);
        MySceneManager.setLastScene(ConstantClass.BrandDetailSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.StoreEventSceneName);
    }

    public void NavigateBrandDetailScene(GameObject Brand)
    {
        BrandDetailController.OrganizerId = int.Parse(Brand.transform.GetChild(0).GetComponent<Text>().text);
        BrandDetailController.OrganizerName = Brand.transform.GetChild(1).GetComponent<Text>().text;
        MySceneManager.setLastScene(ConstantClass.HomeSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.BrandDetailSceneName);
    }
}
