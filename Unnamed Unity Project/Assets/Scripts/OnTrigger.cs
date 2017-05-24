using UnityEngine;
using System.Collections;
using Fungus;

public class OnTrigger : MonoBehaviour {

    public bool Clickable;
    public string staticMessage;
    private string Message;
    public Flowchart flowchart;
    public PlayerController playerController;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            if (flowchart.GetBooleanVariable("Triggered") == false)
            {
                playerController.moveSpeed = 0;
                playerController.trig = true;

                if (PlayerController.Instance.mentalityStat.CurrentVal > 0 && PlayerController.Instance.mentalityStat.CurrentVal <= 33)
                {
                    Message = "1";
                }
                if (PlayerController.Instance.mentalityStat.CurrentVal > 33 && PlayerController.Instance.mentalityStat.CurrentVal <= 66)
                {
                    Message = "2";
                }
                if (PlayerController.Instance.mentalityStat.CurrentVal > 66 && PlayerController.Instance.mentalityStat.CurrentVal <= 100)
                {
                    Message = "3";
                }

                flowchart.SendFungusMessage(Message);
            }
        } 
    }

    void OnMouseDown()
    {
        if (Clickable)
        {
            if (flowchart.GetBooleanVariable("Triggered") == false)
            {
                playerController.moveSpeed = 0;
                playerController.trig = true;

                flowchart.SendFungusMessage(staticMessage);
            }
        }
    }
}
