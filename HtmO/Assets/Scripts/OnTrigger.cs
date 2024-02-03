using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OnTrigger : MonoBehaviour {

    private static OnTrigger instance;
    public static OnTrigger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<OnTrigger>();
            }
            return instance;
        }
    }

    public bool reset;
    public bool Clickable;
    public string staticMessage;
    private string Message;
    private int randomValue;
    public Player playerController;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter()
    {
        if (Clickable)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    void OnMouseExit()
    {
        if (Clickable)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
    
    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if(collider.tag == "Player")
    //    {
    //        if (flowchart.GetBooleanVariable("Triggered") == false)
    //        {
    //            if(!Clickable)
    //            {
    //                playerController.moveSpeed = 0;
    //                playerController.trig = true;

    //                flowchart.SendFungusMessage(staticMessage);
    //            }
    //        }
    //    } 
    //}

    //void OnMouseDown()
    //{
    //    if (Clickable)
    //    {
    //        if (flowchart.GetBooleanVariable("Triggered") == false)
    //        {
    //            playerController.moveSpeed = 0;
    //            playerController.trig = true;

    //            flowchart.SendFungusMessage(staticMessage);
    //        }
    //    }
    //}

    //void Start()
    //{
    //    if (reset == true)
    //    {
    //        PlayerPrefs.SetInt("TotalDeaths", 0);
    //    }
    //    PlayerPrefs.GetInt("TotalDeaths");
    //}

    //void Update()
    //{
    //    if(flowchart.GetBooleanVariable("Triggered") == true)
    //    {
    //        Cursor.visible = true;
    //    }
    //    else if(flowchart.GetBooleanVariable("Triggered") == false && AbilityManager.Instance.triggered == false)
    //    {
    //        Cursor.visible = false;
    //    }
    //}

    //public void DeathTaunt()
    //{
    //    if (flowchart.GetBooleanVariable("Died") == true)
    //    {
    //        playerController.moveSpeed = 0;
    //        playerController.trig = true;

    //        if(PlayerPrefs.GetInt("TotalDeaths") == 1)
    //        {
    //            flowchart.ExecuteBlock("Circley Death1");
    //        }
    //        if (PlayerPrefs.GetInt("TotalDeaths") == 2)
    //        {
    //            flowchart.ExecuteBlock("Circley Death2");
    //        }
    //        if (PlayerPrefs.GetInt("TotalDeaths") == 3)
    //        {
    //            flowchart.ExecuteBlock("Circley Death3");
    //        }
    //        if (PlayerPrefs.GetInt("TotalDeaths") >= 4)
    //        {
    //            randomValue = Random.Range(1, 100);
    //            if (randomValue >= 1 && randomValue < 20)
    //            {
    //                flowchart.ExecuteBlock("Circley DeathR1");
    //            }
    //            if (randomValue >= 20 && randomValue < 40)
    //            {
    //                flowchart.ExecuteBlock("Circley DeathR2");
    //            }
    //            if (randomValue >= 40 && randomValue < 60)
    //            {
    //                flowchart.ExecuteBlock("Circley DeathR3");
    //            }
    //            if (randomValue >= 60 && randomValue < 80)
    //            {
    //                flowchart.ExecuteBlock("Circley DeathR4");
    //            }
    //            if (randomValue >= 80 && randomValue < 100)
    //            {
    //                flowchart.ExecuteBlock("Circley DeathR5");
    //            }
    //        }
    //    }
    //}

    //public void Died()
    //{
    //    flowchart.SetBooleanVariable("Died", true);
    //    PlayerPrefs.SetInt("TotalDeaths", PlayerPrefs.GetInt("TotalDeaths") + 1);
    //}
}
