using UnityEngine;
using System.Collections;

public class HitSoundScript : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip hitSound;
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);
    }
}
