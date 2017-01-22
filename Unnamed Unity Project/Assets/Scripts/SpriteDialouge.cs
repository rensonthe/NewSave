using UnityEngine;
using System.Collections;

public class SpriteDialouge : MonoBehaviour {

    public SpriteRenderer sprite;
    public bool activateLMB;

    private float Timer;
    private float Cooldown = 7;

    void Awake()
    {
        Timer = Cooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && Timer >= Cooldown)
        {
            StartCoroutine("FadeCheck");
            Timer = 0;
            if(PlayerController.Instance.LMB == false && activateLMB == true)
            {
                PlayerController.Instance.LMB = true;
            }
        }
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
    }

    IEnumerator FadeCheck()
    {
        Fade(true, 1.25f);
        yield return new WaitForSeconds(5f);
        Fade(false, 2f);
        StopCoroutine("FadeCheck");
    }

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    private void Update()
    {
        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        sprite.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }
}
