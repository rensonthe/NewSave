using UnityEngine;
using System.Collections;
using Fungus;

public class InteractiveCollision : MonoBehaviour {

    public Flowchart flowchart;

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
            PlayerController.Instance.transform.position = PlayerController.Instance.startPos;
            flowchart.ExecuteBlock("Fall");
        }
    }
}
