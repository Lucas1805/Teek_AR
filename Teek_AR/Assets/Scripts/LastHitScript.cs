using UnityEngine;
using System.Collections;

public class LastHitScript : MonoBehaviour {

    public static bool IsLastHit = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragon"))
        {
            //GameObject.Find("DisplayText").GetComponent<Text>().text = "trúng chắc luôn xD";
            //GameObject.Find("RaikouClone").SetActive(true);
            //GameObject.Find("RaikouClone").transform.position = new Vector3(0, 0, 0);

            IsLastHit = true;
        }
    }
}
