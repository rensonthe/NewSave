using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    private static Tutorial instance;
    public static Tutorial Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Tutorial>();
            }
            return instance;
        }
    }

    public bool peekDown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            peekDown = true;
        }
    }
}
