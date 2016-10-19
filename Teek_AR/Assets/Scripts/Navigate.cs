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
}
