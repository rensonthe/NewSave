using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Flowchart flowchart;
    public InputField mainInputField;
    public GameObject EnterNameGUI;
    public Text enterName;
    static string playerName;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(enterName.text);
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
