using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

    public SpriteRenderer[] Sprites;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            foreach(SpriteRenderer s in Sprites)
            {
                s.color = Color.white;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        foreach (SpriteRenderer s in Sprites)
        {
            s.color = Color.black;
        }
    }
}
