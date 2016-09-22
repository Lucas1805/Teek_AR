using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class FollowObject : MonoBehaviour {
    public GameObject TrackObject;
    public Vector3 Offset;

    private float distance;
    private SphereCollider sphereCollider;
    private int ran;

    public Text displayText;

	// Use this for initialization
	void Start () {
        distance = Vector3.Distance(Camera.main.transform.position, TrackObject.transform.position);
        sphereCollider = TrackObject.GetComponent<SphereCollider>();
        ran = Random.Range(1,3);
        ran = 2;

        displayText.text = "";

        
    }
	
	// Update is called once per frame
	void Update () {

        if (sphereCollider.enabled)
        {
            switch (ran)
            {
                // neu 1 thi in ra con Raikou
                case 1:
                    TrackObject = GameObject.Find("Raikou");
                    //GameObject tempGameObject = GameObject.Find("/Character_Witch_Dragon/Models/ChaDragon");
                    GameObject tempGameObject = GameObject.Find("Dragon");
                    
                    if (tempGameObject!=null)
                    {
                        //displayText.text = tempGameObject.ToString();
                        tempGameObject.SetActive(false);
                    }
                    break;

                // con lai in ra con Dragon

                case 2:
                    TrackObject = GameObject.Find("Dragon");
                    //GameObject tempGameObject = GameObject.Find("/Character_Witch_Dragon/Models/ChaDragon");
                    GameObject tempGameObject2 = GameObject.Find("Raikou");

                    if (tempGameObject2 != null)
                    {
                        //displayText.text = tempGameObject2.ToString();
                        tempGameObject2.SetActive(false);
                    }
                    break;

                default:
                    break;
            }

            gameObject.transform.position = Camera.main.WorldToScreenPoint(TrackObject.transform.position) + Offset;
        }
        else
        {
            gameObject.transform.position = new Vector3(0, Screen.height * 2, 0);
        }
    }
}
