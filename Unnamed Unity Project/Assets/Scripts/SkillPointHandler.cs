using UnityEngine;
using System.Collections;

public class SkillPointHandler : MonoBehaviour {

    public TooltipText[] tooltipText;

    public void Awake()
    {
        tooltipText[0].Revealed = true;
    }

    public void LevelUp(int level)
    {
        if (level == 1)
        {
            tooltipText[1].Revealed = true;
        }
        if (level == 2)
        {
            tooltipText[2].Revealed = true;
        }
    }

}
