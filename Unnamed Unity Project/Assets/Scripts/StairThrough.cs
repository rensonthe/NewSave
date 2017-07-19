using UnityEngine;
using System.Collections;

public class StairThrough : MonoBehaviour
{
    private Vector2 boxSize = new Vector2(0.7f, 0.7f);
    private Vector2 boxOffSet = new Vector2(0.05f, -1.35f);

    private Vector2 size;
    private Vector2 offset;
    private bool activated = false;

    private BoxCollider2D boxCollider;
    public RaycastController raycastController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (boxCollider == null)
            {
                boxCollider = PlayerController.Instance.GetComponent<BoxCollider2D>();
                size = boxCollider.size;
                offset = boxCollider.offset;
            }
            if (!activated)
            {
                activated = !activated;
                boxCollider.size = boxSize;
                boxCollider.offset = boxOffSet;
                raycastController.CalculateRaySpacing();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (activated)
            {
                activated = !activated;
                boxCollider.size = size;
                boxCollider.offset = offset;
                raycastController.CalculateRaySpacing();
            }
        }
    }
}
