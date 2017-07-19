using UnityEngine;
using System.Collections;

public class RoomCamera : MonoBehaviour {

    public CameraFollow camerafollow;
    public Vector3 minCameraPosDown;
    public Vector3 minCameraPosUp;
    public Vector3 lerpPosition;
    private bool activated = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(!activated)
            {
                activated = !activated;
                camerafollow.minCameraPos.y = minCameraPosDown.y;                
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            lerpPosition.x = other.transform.position.x;
            if (activated)
            {
                activated = !activated;
                camerafollow.minCameraPos.y = minCameraPosUp.y;
                camerafollow.StartLerp(lerpPosition);
            }
        }
    }
}
