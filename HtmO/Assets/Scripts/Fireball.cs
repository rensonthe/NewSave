using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour {

    public float speed;
    private float fireballTimer = 2.5f;
    public ParticleSystem fireballEffect;

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
        fireballTimer -= Time.deltaTime;
        if (fireballTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platforms" || other.tag == "Ground" || other.tag == "Bounds" || other.tag == "noClimb" || other.tag == "TryAngle")
        {
            Destroy(gameObject);
            Destroy(Instantiate(fireballEffect.gameObject, transform.position, Quaternion.identity) as GameObject, fireballEffect.startLifetime);
        }
    }
}
