using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drinks : MonoBehaviour {

    public float speed;

    void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
