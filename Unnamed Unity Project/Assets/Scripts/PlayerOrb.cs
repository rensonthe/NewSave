﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerOrb : MonoBehaviour
{
    public ParticleSystem orbEffect;
    public CircleCollider2D damageCollider;
    public float speed = 5f;
    private float orbTimer = 2.5f;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        OrbMechanic();
        orbTimer -= Time.deltaTime;
        if(orbTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }

    private void OrbMechanic()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerController.Instance.OrbJaunt == true)
        {
            StartCoroutine(OrbWait());
            PlayerController.Instance.soulsStat.CurrentVal -= PlayerController.Instance.orbJauntVal;
        }
    }

    IEnumerator OrbWait()
    {
        while (true)
        {
            damageCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            Destroy(Instantiate(orbEffect.gameObject, transform.position, Quaternion.identity) as GameObject, orbEffect.startLifetime);
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Wall")
        {
            Destroy(gameObject);
            Destroy(Instantiate(orbEffect.gameObject, transform.position, Quaternion.identity) as GameObject, orbEffect.startLifetime);
        }
    }
}
