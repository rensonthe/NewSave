using UnityEngine;
using System.Collections;

public class SkillPoints {

    public string parent = string.Empty;
    public int required;
    public bool unlocked = false;

    public SkillPoints(int required, string parent)
    {
        this.required = required;
        this.parent = parent;
    }

    public SkillPoints(int required)
    {
        this.required = required;
    }

}
