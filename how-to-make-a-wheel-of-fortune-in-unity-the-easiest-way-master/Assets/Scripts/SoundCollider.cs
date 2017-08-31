using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.instance.PlaySound("Colliders", transform.position);
    }


}
