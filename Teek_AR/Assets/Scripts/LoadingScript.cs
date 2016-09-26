using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour {

    private SpriteRenderer sp = new SpriteRenderer();

	// Use this for initialization
	void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
        sp.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void enableSprite()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp.enabled == false)
        {
            sp.enabled = true;
        }
    }

    public void disableSprite()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp.enabled == true)
        {
            sp.enabled = false;
        }
    }
}
