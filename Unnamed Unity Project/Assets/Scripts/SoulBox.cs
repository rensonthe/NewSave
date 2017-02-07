using UnityEngine;
using System.Collections;
using System;

public class SoulBox : Character
{
    public ParticleSystem deathEffect;
    public GameObject XPOrb;
    public GameObject healthOrb;
    public GameObject energyOrb;

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    public override IEnumerator TakeDamage()
    {
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            Destroy(gameObject.GetComponent<Collider2D>());
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            MyAnimator.SetTrigger("death");
            yield return new WaitForSeconds(1f);
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
            Instantiate(XPOrb, transform.position, Quaternion.identity);
            Instantiate(healthOrb, transform.position, Quaternion.identity);
            Instantiate(energyOrb, transform.position, Quaternion.identity);
            yield return null;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Sword")
        {
            health -= 10;
            StartCoroutine(TakeDamage());
        }
    }
}
