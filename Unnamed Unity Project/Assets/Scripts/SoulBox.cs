using UnityEngine;
using System.Collections;
using System;

public class SoulBox : Character
{
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
            Instantiate(XPOrb, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
            yield return null;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Sword")
        {
            StartCoroutine(TakeDamage());
        }
    }
}
