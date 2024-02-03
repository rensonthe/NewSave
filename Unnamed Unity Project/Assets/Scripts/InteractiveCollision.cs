using UnityEngine;
using System.Collections;
public class InteractiveCollision : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //if(other.tag == "Player")
        //{
        //    PlayerController.Instance.transform.position = PlayerController.Instance.startPos;
        //    flowchart.ExecuteBlock("Fall");
        //}
    }
}
