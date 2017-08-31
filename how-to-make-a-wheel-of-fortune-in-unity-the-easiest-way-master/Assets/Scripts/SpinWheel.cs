using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinWheel : MonoBehaviour
{
    public AmbientMovement ambientMovement;
	public List<float> prize;
	public List<AnimationCurve> animationCurves;
	
	private bool spinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
	
	void Start(){
		spinning = false;
		anglePerItem = 360/prize.Count;		
	}
	
	void  Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !spinning) {
		
			randomTime = Random.Range (1, 4);
			itemNumber = Random.Range (0, prize.Count);
			float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
			
			StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
		}
	}
	
	IEnumerator SpinTheWheel (float time, float maxAngle)
	{
		spinning = true;
		
		float timer = 0.0f;		
		float startAngle = transform.eulerAngles.z;		
		maxAngle = maxAngle - startAngle;
		
		int animationCurveNumber = Random.Range (0, animationCurves.Count);
		Debug.Log ("Animation Curve No. : " + animationCurveNumber);
		
		while (timer < time) {
		//to calculate rotation
			float angle = maxAngle * animationCurves [animationCurveNumber].Evaluate (timer / time) ;
			transform.eulerAngles = new Vector3 (0.0f, 0.0f, angle + startAngle);
			timer += Time.deltaTime;
			yield return 0;
		}
		
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, maxAngle + startAngle);
		spinning = false;
			
		Debug.Log ("Prize: " + prize [itemNumber]);//use prize[itemNumnber] as per requirement
        if(prize[itemNumber] >= 1)
        {
            if (prize[itemNumber] == 1)
            {
                PlayerPrefs.SetFloat("Drinks", PlayerPrefs.GetFloat("Drinks") + 1);
                StartCoroutine(RainingDrinks());
            }
            else if (prize[itemNumber] == 2)
            {
                PlayerPrefs.SetFloat("Chips", PlayerPrefs.GetFloat("Chips") + 1);
                StartCoroutine(RainingChips());
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance") + prize[itemNumber]);
        }
    }

    IEnumerator RainingChips()
    {
        ambientMovement.spawnable = true;
        ambientMovement.StartCoroutine("SpawnChips");
        yield return new WaitForSeconds(10);
        ambientMovement.spawnable = false;
        ambientMovement.StopCoroutine("SpawnChips");
    }

    IEnumerator RainingDrinks()
    {
        ambientMovement.spawnable = true;
        ambientMovement.StartCoroutine("SpawnDrinks");
        yield return new WaitForSeconds(10);
        ambientMovement.spawnable = false;
        ambientMovement.StopCoroutine("SpawnDrinks");
    }
}
