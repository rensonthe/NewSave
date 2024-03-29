﻿using UnityEngine;
using System.Collections;

public class CameraPanTrigger : MonoBehaviour {

    public CameraFollow cameraFollow;
    public GameObject gameObjectSetActive;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            UIManager.Instance.isAllowed = false;
            cameraFollow.CameraPanTrigger();
            Destroy(gameObject);
            gameObjectSetActive.SetActive(true);
        }
    }
}
