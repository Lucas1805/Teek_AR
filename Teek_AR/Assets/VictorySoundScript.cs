using UnityEngine;
using System.Collections;

public class VictorySoundScript : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip victorySound;
	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(victorySound);
    }
}
