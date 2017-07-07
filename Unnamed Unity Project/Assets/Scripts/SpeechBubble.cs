using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    public Image speechBubble;
    public Text speechText;
    public string[] speechLines;
    public float speechSpeed;
    public float waitTime;
    private bool finishedSpeaking = false;
    private int speechIndex;
    private bool isSpeaking = false;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    // Use this for initialization
    void Start () {
        speechText = GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && !isSpeaking)
        {
            StartCoroutine("FadeCheck");
        }

        if(finishedSpeaking == true)
        {
            StartCoroutine(FadeOut());
            finishedSpeaking = false;
        }

        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        speechBubble.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);
        speechText.color = Color.Lerp(new Color(1, 1, 1, 0), Color.black, transition);

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

    IEnumerator Speak()
    {
        if (isSpeaking == false)
        {
            if (speechIndex < speechLines.Length)
            {
                isSpeaking = true;
                speechText.text = string.Empty;
                string currentLine = speechLines[speechIndex];
                int words = 0;
                foreach (char item in currentLine)
                {
                    if(item == ' ')
                    {
                        words++;
                    }
                    if(words == 6)
                    {
                        speechText.text += "\n ";
                        words = 0;
                    }
                    speechText.text += item;
                    yield return new WaitForSeconds(speechSpeed);
                }
                speechIndex++;
                isSpeaking = false;
                finishedSpeaking = true;
            }
        }
    }

    IEnumerator FadeCheck()
    {
        Fade(true, .5f);
        StartCoroutine(Speak());
        StopCoroutine("FadeCheck");
        yield return null;
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(waitTime);
        Fade(false, (1f));
    }
}
