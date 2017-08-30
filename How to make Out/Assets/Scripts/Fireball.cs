using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour {

    public float speed;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
