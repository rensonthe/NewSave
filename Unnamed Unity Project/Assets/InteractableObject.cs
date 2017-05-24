using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

    private bool closed = true;
    public Collider2D collisionSpace;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact()
    {
        GetComponent<Animator>().SetTrigger("use");
        closed = !closed;
        if(closed == false)
        {
            collisionSpace.enabled = false;
        }
        else
        {
            collisionSpace.enabled = true;
        }
    }
}
