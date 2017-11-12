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

    public GameObject UIParent;
    public Image currentAbility;
    public Animator redGoop;
    [HideInInspector]
    public bool triggered;
    [HideInInspector]
    public bool fireball;

    public bool unlocked;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(Interact());

        if (!unlocked)
        {
            UIParent.SetActive(false);
        }
        else
        {
            UIParent.SetActive(true);
        }
    }

    IEnumerator Interact()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !triggered && unlocked)
        {
            ShowUI();
            yield return new WaitForSeconds(1f);
            triggered = true;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && triggered && unlocked)
        {
            HideUI();
            yield return new WaitForSeconds(1f);
            triggered = false;
            Cursor.visible = false;
        }
    }

    public void SelectAbility(GameObject ability)
    {
        currentAbility.transform.GetComponent<Image>().sprite = ability.transform.GetChild(1).GetComponent<Image>().sprite;
        currentAbility.transform.GetComponent<RectTransform>().offsetMax = ability.transform.GetChild(1).GetComponent<RectTransform>().offsetMax;
        currentAbility.transform.GetComponent<RectTransform>().offsetMin = ability.transform.GetChild(1).GetComponent<RectTransform>().offsetMin;
    }

    public void SetAbilityBool(string abilityName)
    {
        if(abilityName == "fireball")
        {
            fireball = true;
        }
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
