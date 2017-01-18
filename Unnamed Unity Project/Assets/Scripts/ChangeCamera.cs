using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

    public CameraFollow cameraFollow;
    public Vector3 minCameraPos;

    private bool activated = false;

    void Update()
    {
        if(activated == true)
        {
            SmoothChange();
            if(minCameraPos.y >= 2)
            {
                activated = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            activated = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            activated = false;
            minCameraPos.y -= cameraFollow.minCameraPos.y;
            cameraFollow.minCameraPos = minCameraPos;
        }
    }

    void SmoothChange()
    {
        cameraFollow.minCameraPos = minCameraPos;
        if (minCameraPos.y <= 2)
        {
            minCameraPos.y += Time.deltaTime;
        }
    }
}
