using UnityEngine;
using System.Collections;

public class AmbientMovement : MonoBehaviour {

    private Vector3 posA;

    [SerializeField]
    private Transform childTransform;

    public GameObject chipsPrefab;
    public GameObject drinksPrefab;

    private float speed;
    public bool spawnable;

    // Use this for initialization
    void Start()
    {
        posA = childTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator SpawnChips()
    {
        if (spawnable)
        {
            while (true)
            {
                Vector3 position = new Vector3(Random.Range(-5.59f,5.59f), posA.y, 0);
                GameObject cloud = Instantiate(chipsPrefab, position, Quaternion.identity) as GameObject;
                cloud.GetComponent<Chips>().speed = Random.Range(1, 4);
                yield return new WaitForSeconds(Random.Range(0.25f, 0.65f));
            }
        }
    }

    public IEnumerator SpawnDrinks()
    {
        if (spawnable)
        {
            while (true)
            {
                Vector3 position = new Vector3(Random.Range(-5.59f, 5.59f), posA.y, 0);
                GameObject cloud = Instantiate(drinksPrefab, position, Quaternion.identity) as GameObject;
                cloud.GetComponent<Drinks>().speed = Random.Range(1, 4);
                yield return new WaitForSeconds(Random.Range(0.25f, 0.65f));
            }
        }
    }
}
