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
        if (level == 2)
        {
            tooltipText[1].Revealed = true;
            tooltipText[2].Revealed = true;
            tooltipText[3].Revealed = true;
        }
        if (level == 3)
        {

        }
    }

}
