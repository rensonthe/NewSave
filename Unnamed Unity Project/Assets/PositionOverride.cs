using UnityEngine;
using System.Collections;

public class PositionOverride : MonoBehaviour {

    public CameraFollow cameraFollow;
    public Vector3 minPos;
    public Vector3 maxPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            cameraFollow.minCameraPos.y = minPos.y;
            cameraFollow.maxCameraPos.y = maxPos.y;
        }
    }
}
