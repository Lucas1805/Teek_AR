using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
    public GameObject TrackObject;
    public Vector3 Offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (TrackObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = Camera.main.WorldToScreenPoint(TrackObject.transform.position) + Offset;
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}
