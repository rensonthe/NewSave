using UnityEngine;
using System.Collections;
public class EInteraction : MonoBehaviour {

    public string Message;
    public PlayerController playerController;
    public SpriteRenderer bubble;
    public bool triggered = false;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    private void Update()
    {
        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        bubble.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;

        //if(Input.GetKeyDown(KeyCode.E) && triggered == true)
        //{
        //    if (flowchart.GetBooleanVariable("Triggered") == false)
        //    {
        //        playerController.moveSpeed = 0;
        //        playerController.trig = true;

        //        flowchart.SendFungusMessage(Message);
        //    }
        //}
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    void OnTriggerEnter2D()
    {
        triggered = true;
        StartCoroutine("FadeCheck");
    }

    void OnTriggerExit2D()
    {
        triggered = false;
    }

    IEnumerator FadeCheck()
    {
        Fade(true, 0.75f);
        yield return new WaitForSeconds(1.25f);
        Fade(false, 0.25f);
        StopCoroutine("FadeCheckIn");
    }
}
