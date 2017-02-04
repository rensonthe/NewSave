using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Image fadeImage;
    public bool isAllowed = true;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    public Flowchart flowchart;
    public InputField mainInputField;
    public GameObject EnterNameGUI;
    public GameObject UpgradeUI;
    public Text currentSkillPoints;
    public Text enterName;
    static string playerName;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab) && isAllowed)
        {
            ShowUpgradeUI();
            fadeImage.enabled = true;
            PlayerController.Instance.SetInactive();
        }
        if (!isInTransition)
            return;

        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.black, transition);

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

    IEnumerator FadeCheck()
    {
        Fade(true, 1.25f);
        yield return new WaitForSeconds(1.25f);
        UpgradeUI.SetActive(!UpgradeUI.activeSelf);
        fadeImage.enabled = false;
        Fade(false, 1f);
        if (UpgradeUI.activeSelf == false)
        {
            PlayerController.Instance.SetActive();
        }
        StopCoroutine("FadeCheck");
    }

    public void ShowUpgradeUI()
    {
        StartCoroutine("FadeCheck");
        if (UpgradeUI.activeSelf == false)
        {
            PlayerController.Instance.SetActive();
        }
    }

    public void TABUpgrades() { 
}

    public void ShowGUI()
    {
        EnterNameGUI.SetActive(true);
    }

    public void EnterName()
    {
        if(enterName.text == "Loriella" || enterName.text == "loriella")
        {
            flowchart.ExecuteBlock("Loriella");
        }
        if(enterName.text == string.Empty)
        {
            flowchart.ExecuteBlock("Error");
        }
        if(enterName.text != string.Empty)
        {
            if(enterName.text == "Loriella" || enterName.text == "loriella")
            {
                return;
            }
            mainInputField.text = enterName.text;
            PlayerPrefs.SetString("Player Name", enterName.text);
            playerName = PlayerPrefs.GetString("Player Name");
            SetName();
            PlayerController.Instance.SetActive();
            EnterNameGUI.SetActive(false);
            flowchart.SetStringVariable("MyName", playerName);
            flowchart.ExecuteBlock("Name");
        }

    }

    public void SetName()
    {
        string name = PlayerPrefs.GetString("Player Name");
    }
}
