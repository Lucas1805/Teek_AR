using UnityEngine;
using System.Collections;

public class ShootSoundScript : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip shootSound;
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
        audioSource.PlayOneShot(shootSound);
    }
}
