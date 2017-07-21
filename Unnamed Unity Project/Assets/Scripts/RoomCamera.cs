using UnityEngine;
using System.Collections;

public class RoomCamera : MonoBehaviour {

    public Transform[] yPositions;
    public CameraFollow camerafollow;
    public Vector3 minCameraPosDown;
    public Vector3 minCameraPosUp;
    public Vector3[] lerpPosition;
    private bool activated = false;
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

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
            lerpPosition[0].x = other.transform.position.x;
            if (activated && player.transform.position.y > yPositions[0].position.y)
            {
                activated = !activated;
                camerafollow.minCameraPos.y = minCameraPosUp.y;
                camerafollow.StartLerp(lerpPosition[0]);
            }
            lerpPosition[1].x = other.transform.position.x;
            if (activated && player.transform.position.y < yPositions[0].position.y && player.transform.position.y > yPositions[1].position.y)
            {
                Debug.Log("run");
                activated = !activated;
                camerafollow.minCameraPos.y = minCameraPosDown.y;
                camerafollow.StartLerp(lerpPosition[1]);
            }
        }
    }
}
