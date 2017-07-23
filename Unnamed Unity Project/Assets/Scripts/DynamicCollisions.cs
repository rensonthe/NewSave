using UnityEngine;
using System.Collections;

public class DynamicCollisions : MonoBehaviour {

    public Collider2D groundStair;
    public bool input;
    public bool deactivate;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            groundStair.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Player" && Input.GetKey(KeyCode.S) && input) || (other.tag == "Player" && Input.GetKey(KeyCode.DownArrow) && input))
        {
            groundStair.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && deactivate)
        {
            groundStair.gameObject.SetActive(false);
        }
    }
}
