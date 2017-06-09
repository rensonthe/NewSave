using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    Text speechText;
    public string[] speechLines;
    public float speechSpeed;
    private int speechIndex;
    private bool isSpeaking = false;

	// Use this for initialization
	void Start () {
        speechText = GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Speak());
        }	
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
                foreach (char item in currentLine)
                {
                    speechText.text += item;
                    yield return new WaitForSeconds(speechSpeed);
                }
                speechIndex++;
                isSpeaking = false;
            }
        }
    }
}
