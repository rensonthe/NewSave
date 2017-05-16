using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

    public CameraFollow cameraFollow;
    public Vector3 minCameraPos;
    public float posXReset;
    public bool xReset;
    public bool smoothChange;
    public float posY;

    private bool activated = false;

    void Update()
    {
        if(activated == true)
        {
            if (smoothChange == true)
            {
                SmoothChange();
                if (minCameraPos.y >= posY)
                {
                    activated = false;
                }
            }
            else if (smoothChange == false)
            {
                cameraFollow.minCameraPos = minCameraPos;
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
            if(xReset == true)
        {
            cameraFollow.minCameraPos.x = posXReset;
        }
            activated = false;
            if(smoothChange == true)
            {
                minCameraPos.y -= cameraFollow.minCameraPos.y;
                cameraFollow.minCameraPos = minCameraPos;
            }
        }
    }

    void SmoothChange()
    {
        cameraFollow.minCameraPos = minCameraPos;
        if (minCameraPos.y <= posY)
        {
            minCameraPos.y += Time.deltaTime;
        }
    }
}
