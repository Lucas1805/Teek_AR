using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Linq;
using System;


public class HomeScript : MonoBehaviour {
        
    public Text mac;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    /// <summary>
    /// This function is used to get MAC address of Wifi the phone is connected to. THIS FUNTION ONLY WORK ON ANDROID
    /// </summary>
    /// <returns>MAC Address String</returns>
    private string getBSSID()
    {
#if UNITY_ANDROID
        string bssid = null;

        AndroidJavaObject mWiFiManager = null;
        if (mWiFiManager == null)
        {
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                mWiFiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
            }
        }
        bssid = mWiFiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getBSSID");
        return bssid;
#endif

#if UNITY_IOS
        //NOT IMPLEMENT YET
#endif
    }


    public void showMac()
    {
        mac.text = getBSSID();
    }


    
    
}
