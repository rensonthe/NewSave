using UnityEngine;
using System.Collections;

public class AmbientMovement : MonoBehaviour {

    private Vector3 posA;

    private Vector3 posB;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

    public GameObject prefab;

    private float speed;

    // Use this for initialization
    void Start()
    {
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        StartCoroutine(SpawnCloud());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnCloud()
    {
        while (true)
        {
            Vector3 position = new Vector3(posA.x, Random.Range(3.75f, -1.5f), 0);
            GameObject cloud = Instantiate(prefab, position, Quaternion.identity) as GameObject;
            cloud.GetComponent<Cloud>().speed = Random.Range(1, 6);
            yield return new WaitForSeconds(Random.Range(3f,7f));
        }
    }
}
