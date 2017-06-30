using UnityEngine;
using System.Collections;

public class RoomCamera : MonoBehaviour {

    public CameraFollow camerafollow;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;
    public Vector3 lerpPosition;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            camerafollow.StartLerp(lerpPosition, minCameraPos, maxCameraPos);
        }
    }
}
