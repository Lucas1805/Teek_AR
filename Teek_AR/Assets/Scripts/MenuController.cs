using UnityEngine;
using System.Collections;
using Assets;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    private bool isOpen = false;
    private bool isClose = false;
    private Vector2 origin;
    private Vector2 target;

    // Use this for initialization
    void Start () {
        origin = transform.position;
        target.y = transform.position.y;
        target.x = transform.position.x + 860;
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpen)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, 3000 * Time.deltaTime);

        }
        if (isClose)
        {
            transform.position = Vector2.MoveTowards(transform.position, origin, 3000 * Time.deltaTime);
        }
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

    public void Open()
    {
        isOpen = true;
        isClose = false;
    }
    public void Close()
    {
        isOpen = false;
        isClose = true;
    }
}
