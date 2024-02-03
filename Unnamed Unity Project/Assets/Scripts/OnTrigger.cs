using UnityEngine;
using System.Collections;
public class OnTrigger : MonoBehaviour {

    public bool Clickable;
    public string staticMessage;
    private string Message;
    public PlayerController playerController;

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
    //    if (collider.tag == "Player")
    //    {
    //        if (flowchart.GetBooleanVariable("Triggered") == false)
    //        {
    //            if (!Clickable)
    //            {
    //                playerController.moveSpeed = 0;
    //                playerController.trig = true;

    //                if (PlayerController.Instance.mentalityStat.CurrentVal > 0 && PlayerController.Instance.mentalityStat.CurrentVal <= 33)
    //                {
    //                    Message = "1";
    //                }
    //                if (PlayerController.Instance.mentalityStat.CurrentVal > 33 && PlayerController.Instance.mentalityStat.CurrentVal <= 66)
    //                {
    //                    Message = "2";
    //                }
    //                if (PlayerController.Instance.mentalityStat.CurrentVal > 66 && PlayerController.Instance.mentalityStat.CurrentVal <= 100)
    //                {
    //                    Message = "3";
    //                }

    //                flowchart.SendFungusMessage(Message);
    //            }
    //        }
    //    }

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
    }