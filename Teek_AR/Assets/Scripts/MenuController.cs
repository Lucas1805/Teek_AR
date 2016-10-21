using UnityEngine;
using System.Collections;
using Assets;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Logout()
    {
        PlayerPrefs.DeleteKey(ConstantClass.PP_UsernameKey);
        PlayerPrefs.DeleteKey(ConstantClass.PP_PasswordKey);
        PlayerPrefs.DeleteKey(ConstantClass.PP_UserIDKey);
        SceneManager.LoadSceneAsync(ConstantClass.LoginSceneName);
    }

    public void NavigateProfileScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.ProfileSceneName);
    }
    public void NavigateHomeScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.HomeSceneName);
    }
    public void NavigateCouponScene()
    {
        SceneManager.LoadSceneAsync(ConstantClass.CouponSceneName);
    }
}
