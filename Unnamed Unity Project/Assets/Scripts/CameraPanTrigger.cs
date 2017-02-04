using UnityEngine;
using System.Collections;

public class CameraPanTrigger : MonoBehaviour {

    public CameraFollow cameraFollow;

    void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.Instance.isAllowed = false;
        cameraFollow.CameraPanTrigger();
        Destroy(gameObject);
    }
}
