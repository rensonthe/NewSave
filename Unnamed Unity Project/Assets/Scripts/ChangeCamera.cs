using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

    public CameraFollow cameraFollow;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;
    public float posXReset;
    public bool xReset;
    public bool smoothChange;
    public float posY;
    public bool smoothChangeX;
    public float posX;

    private bool activated = false;
    private bool activatedX = false;

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
                cameraFollow.maxCameraPos = maxCameraPos;
            }
        }
        if(activatedX == true)
        {
            if (smoothChangeX == true)
            {
                SmoothChangeX();
                if (minCameraPos.x >= posX)
                {
                    activatedX = true;
                }
            }
            else if (smoothChangeX == false)
            {
                cameraFollow.minCameraPos = minCameraPos;
                cameraFollow.maxCameraPos = maxCameraPos;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            activated = true;
            activatedX = false;
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
            activatedX = true;
            activated = false;
            if(smoothChange == true)
            {
                minCameraPos.y -= cameraFollow.minCameraPos.y;
                cameraFollow.minCameraPos = minCameraPos;
            }
            if (smoothChangeX == true)
            {
                minCameraPos.x -= cameraFollow.minCameraPos.x;
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

    void SmoothChangeX()
    {
        cameraFollow.minCameraPos = minCameraPos;
        if (minCameraPos.x <= posX)
        {
            minCameraPos.x += Time.deltaTime;
        }
    }
}
