using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {

    public Image image;
    bool triggered = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggered){
            StartCoroutine("FadeCheck");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.Instance.startPos = transform.position;

            triggered = true;
            image.enabled = true;
        }
    }
    
    void OnTriggerExit2D()
    {
        triggered = false;
        image.enabled = false;
    }

    IEnumerator FadeCheck()
    {
        PlayerController.Instance.SetInactive();
        FadeManager.Instance.Fade(true, 1.25f);
        yield return new WaitForSeconds(3f);
        FadeManager.Instance.Fade(false, 2f);
        PlayerController.Instance.SetActive();
        StopCoroutine("FadeCheck");
    }
}
