using UnityEngine;
using System.Collections;

public class TooltipText : MonoBehaviour {

    private static TooltipText instance;

    public static TooltipText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TooltipText>();
            }
            return instance;
        }
    }

    public string abilityName;
    public string abilityDescription;
    public string abilityEffect;
    public string abilityEnergy;
    public string abilityExtra;
    public string abilityCost;

    public void SetText()
    {
        Tooltip.Instance.abilityName.text = abilityName.ToString();
        Tooltip.Instance.abilityDescription.text = abilityDescription.ToString();
        Tooltip.Instance.abilityEffect.text = abilityEffect.ToString();
        Tooltip.Instance.abilityEnergy.text = "Energy: " + abilityEnergy.ToString();
        Tooltip.Instance.abilityExtra.text = abilityExtra.ToString();
        Tooltip.Instance.abilityCost.text = abilityCost.ToString();
    }
}
