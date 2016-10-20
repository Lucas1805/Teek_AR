using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour {
    public void NavigateStoreEventScene(GameObject ButtonObject)
    {
        StoreEventController.StoreId = int.Parse(ButtonObject.transform.GetChild(0).gameObject.GetComponent<Text>().text);
        StoreEventController.StoreName = ButtonObject.transform.GetChild(1).gameObject.GetComponent<Text>().text;

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

    public void NavigateBrandDetailToEventDetailScene(GameObject Event)
    {
        EventDetailScript.EventId = int.Parse(Event.transform.GetChild(0).GetComponent<Text>().text);
        EventDetailScript.EventName = Event.transform.GetChild(3).GetComponent<Text>().text;
        EventDetailScript.EventImage = Event.transform.GetChild(1).GetComponent<Image>();

        MySceneManager.setLastScene(ConstantClass.BrandDetailSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.EventDetailSceneName);
    }

    public void NavigateStoreEventSceneToEventDetailScene(GameObject Event)
    {
        EventDetailScript.EventId = int.Parse(Event.transform.GetChild(0).GetComponent<Text>().text);

        MySceneManager.setLastScene(ConstantClass.StoreEventSceneName);
        SceneManager.LoadSceneAsync(ConstantClass.EventDetailSceneName);
    }

    public void NavigateEventDetailToGameScene(GameObject Event)
    {
        EventDetailScript.EventId = int.Parse(Event.transform.GetChild(0).GetComponent<Text>().text);
        SceneManager.LoadSceneAsync(ConstantClass.GameSceneName);
    }
}
