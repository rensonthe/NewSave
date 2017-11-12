using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClimb : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.Instance.noClimb = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player.Instance.noClimb = false;
        }
    }
}
