using UnityEngine;
using System.Collections;

public class RoomCamera : MonoBehaviour {

    public CameraFollow camerafollow;
    public Vector3 minCameraPosDown;
    public Vector3 minCameraPosUp;
    public Vector3 lerpPosition;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(camerafollow.minCameraPos.y > minCameraPosDown.y)
            {
                camerafollow.minCameraPos.y = minCameraPosDown.y;                
            }
            else if(camerafollow.minCameraPos.y < minCameraPosUp.y)
            {
                camerafollow.minCameraPos.y = minCameraPosUp.y;
                camerafollow.StartLerp(lerpPosition);
            }
        }
    }
}
