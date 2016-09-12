using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameController.ARCamera = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
