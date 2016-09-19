using UnityEngine;
using System.Collections;

public class DragonBackgroundSoundScript : MonoBehaviour {

    private AudioSource audioSource;

    public AudioClip dragonBackgroundSound;

    // Use this for initialization
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(dragonBackgroundSound);
    }
}

