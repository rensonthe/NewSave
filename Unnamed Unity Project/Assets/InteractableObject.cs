using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

    private bool closed = true;
    public Collider2D collisionSpace;
    public FogOfWar roomA;
    public FogOfWar roomB;

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
            if (roomA != null && !roomA.hasPlayer)
            {
                StartCoroutine(roomA.FadeCheckIn());
            }
            if (roomB != null && !roomB.hasPlayer)
            {
                StartCoroutine(roomB.FadeCheckIn());
            }
            collisionSpace.enabled = false;
        }
        else
        {
            if (roomA != null && !roomA.hasPlayer)
            {
                StartCoroutine(roomA.FadeCheckOut());
            }
            if (roomB != null && !roomB.hasPlayer)
            {
                StartCoroutine(roomB.FadeCheckOut());
            }
            collisionSpace.enabled = true;
        }
    }
}
