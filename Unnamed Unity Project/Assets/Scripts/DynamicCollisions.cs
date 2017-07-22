using UnityEngine;
using System.Collections;

public class DynamicCollisions : MonoBehaviour {

    public Collider2D groundStair;
    public bool input;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            groundStair.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Player" && Input.GetKey(KeyCode.W) && input) || (other.tag == "Player" && Input.GetKey(KeyCode.UpArrow) && input))
        {
            groundStair.gameObject.SetActive(true);
        }
    }
}
