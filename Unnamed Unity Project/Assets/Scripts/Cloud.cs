using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    public float speed;

    void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();	
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
