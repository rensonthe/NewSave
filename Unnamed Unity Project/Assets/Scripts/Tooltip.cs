using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour {

    private static Tooltip instance;

    public static Tooltip Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Tooltip>();
            }
            return instance;
        }
    }

    public Image fadeImage;
    public Text abilityName;
    public Text abilityDescription;
    public Text abilityEffect;
    public Text abilityEnergy;
    public Text abilityExtra;
    public Text abilityCost;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);
        abilityName.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, transition);
        abilityDescription.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0.9f,0.9f,0.9f,1), transition);
        abilityEffect.color = Color.Lerp(new Color(0, 0, 0, 0), Color.red, transition);
        abilityEnergy.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0.25f, 0.10f, 1, 1), transition);
        abilityExtra.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, transition);
        abilityCost.color = Color.Lerp(new Color(0, 0, 0, 0), Color.yellow, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    public IEnumerator FadeIn()
    {
        Fade(true, 1.25f);
        yield return null;
    }

    public IEnumerator FadeOut()
    {
        Fade(false, 1.22f);
        yield return null;
    }

    public void FadeInn()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutt()
    {
        StartCoroutine(FadeOut());
    }
}
