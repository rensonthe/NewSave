using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipText : MonoBehaviour {

    [SerializeField]
    private bool revealed;

    public string abilityName;
    public string abilityDescription;
    public string abilityEffect;
    public string abilityEnergy;
    public string abilityExtra;
    public string abilityCost;
    public Image icon;

    public bool Revealed
    {
        get
        {
            return revealed;
        }

        set
        {
            revealed = value;
            icon.color = Color.white;
        }
    }

    public void SetText()
    {
        if (Revealed)
        {
            Tooltip.Instance.abilityName.text = abilityName.ToString();
            Tooltip.Instance.abilityDescription.text = abilityDescription.ToString();
            Tooltip.Instance.abilityEffect.text = abilityEffect.ToString();
            Tooltip.Instance.abilityEnergy.text = "Energy: " + abilityEnergy.ToString();
            Tooltip.Instance.abilityExtra.text = abilityExtra.ToString();
            Tooltip.Instance.abilityCost.text = abilityCost.ToString();
        }
    }
}
