using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Linq;
using System;
using com.aeksaekhow.androidnativeplugin;

public class HomeScript : MonoBehaviour {
        
    public Text mac;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    /// <summary>
    /// This function is used to get MAC address of Wifi the phone is connected to
    /// </summary>
    /// <returns>MAC Address String</returns>
    public string getBSSID()
    {
        string mac = "test";
        //var card = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
        //if (card == null)
        //    return null;
        //else
        //{
        //    byte[] bytes = card.GetPhysicalAddress().GetAddressBytes();
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
        //        if (i != bytes.Length - 1)
        //        {
        //            mac = string.Concat(mac + ":");
        //        }
        //    }
        //    mac = card.GetPhysicalAddress().ToString();
        //    return mac;
        //}

        AndroidJavaObject jc = new AndroidJavaObject("android.net.wifi.WifiInfo");
        mac = jc.Call<string>("getBSSID");
        //AndroidJavaClass jc = new AndroidJavaClass("android.os.Build");
        //mac = jc.Get<string>("BRAND");
        return mac;
    }

    public string getWifiName()
    {
        string wifiName = "";
        return wifiName;


    }

    public string getMacAddress()
    {
        string mac = null;
        var card = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
        if (card == null)
            return null;
        else
        {
            byte[] bytes = card.GetPhysicalAddress().GetAddressBytes();
            for (int i = 0; i < bytes.Length; i++)
            {
                mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
                if (i != bytes.Length - 1)
                {
                    mac = string.Concat(mac + ":");
                }
            }
            mac = card.GetPhysicalAddress().ToString();
            return mac;
        }
    }

    public void showMac()
    {
        mac.text = getMacAddress();
    }
    
    
}
