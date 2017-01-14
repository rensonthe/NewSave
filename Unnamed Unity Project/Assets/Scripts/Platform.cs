using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    [SerializeField]
    private Transform childTransform;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.layer = 10;
            other.transform.SetParent(childTransform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
