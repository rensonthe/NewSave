using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    private static AbilityManager instance;
    public static AbilityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AbilityManager>();
            }
            return instance;
        }
    }

    public GameObject currentAbility;
    public Animator redGoop;
    private bool triggered;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !triggered)
        {
            ShowUI();
            yield return new WaitForSeconds(1f);
            triggered = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && triggered)
        {
            HideUI();
            yield return new WaitForSeconds(1f);
            triggered = false;
        }
    }

    public void SelectAbility(GameObject ability)
    {
        currentAbility.transform.GetChild(5).GetComponent<Image>().color = ability.transform.GetChild(0).GetComponent<Image>().color;
    }

    public void ShowUI()
    {
        redGoop.SetBool("Show", true);
    }

    public void HideUI()
    {
        redGoop.SetBool("Hide", true);
    }
}
