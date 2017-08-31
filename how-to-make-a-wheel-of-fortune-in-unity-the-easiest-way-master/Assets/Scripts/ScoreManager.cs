using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text totalHours;
    public Text balance;
    public Text chips;
    public Text drinks;
    public int hourvalue;
    public bool reset;

	// Use this for initialization
	void Start () {
        if(reset == true)
        {
            PlayerPrefs.SetInt("TotalHours", hourvalue);
            PlayerPrefs.SetFloat("Balance", 0);
            PlayerPrefs.SetFloat("Chips", 0);
            PlayerPrefs.SetFloat("Drinks", 0);
        }
        totalHours.text = "Total hours : " + PlayerPrefs.GetInt("TotalHours");
        balance.text = "Balance : " + PlayerPrefs.GetFloat("Balance");
        chips.text = "Chips : " + PlayerPrefs.GetFloat("Chips");
        drinks.text = "Drinks : " + PlayerPrefs.GetFloat("Drinks");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.SetInt("TotalHours", PlayerPrefs.GetInt("TotalHours")+1);
            totalHours.text = "Total hours : " + PlayerPrefs.GetInt("TotalHours");
        }
        balance.text = "Balance : " + PlayerPrefs.GetFloat("Balance");
        chips.text = "Chips : " + PlayerPrefs.GetFloat("Chips");
        drinks.text = "Drinks : " + PlayerPrefs.GetFloat("Drinks");
    }
}
