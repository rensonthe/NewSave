using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

    public SpriteRenderer[] Sprites;
    public bool hasPlayer;

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

        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);
        }

        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    public IEnumerator FadeCheckIn()
    {
        Fade(true, 1.25f);
        StopCoroutine("FadeCheckIn");
        yield return null;
    }

    public IEnumerator FadeCheckOut()
    {
        Fade(false, 1.25f);
        StopCoroutine("FadeCheckOut");
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            hasPlayer = true;
            foreach(SpriteRenderer s in Sprites)
            {
                //StartCoroutine("FadeCheckIn");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            hasPlayer = false;
            foreach (SpriteRenderer s in Sprites)
            {
                //StartCoroutine("FadeCheckOut");
            }
        }
    }
}
