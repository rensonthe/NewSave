using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoor : MonoBehaviour {

    public CameraShake cameraShake;
    public GameObject Barrier;
    public SpriteRenderer[] barrier;
    public GameObject doorMask;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    private bool triggered = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && triggered && FreshBean.Instance.isBean)
        {
            StartCoroutine(DoorOpen());
        }

        if (!isInTransition)
            return;

        for(int i = 0; i < barrier.Length; i++)
        {
            transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
            barrier[i].color = Color.Lerp(new Color(0.102f, 0.102f, 0.102f, 0), Color.white, transition);
        }

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    IEnumerator DoorOpen()
    {
        FreshBean.Instance.beanUnlock = true;
        cameraShake.enabled = true;
        cameraShake.StartCoroutine("FadeCheckOut");
        StartCoroutine("FadeCheckOut");
        Player.Instance.moveSpeed = 0;
        Player.Instance.trig = true;
        yield return new WaitForSeconds(4);
        Barrier.SetActive(false);
        FreshBean.Instance.transform.SetParent(null);
        FreshBean.Instance.boxCollider2D.enabled = false;
        FreshBean.Instance.fadeImage.sortingOrder = -2;
        Player.Instance.moveSpeed = 6;
        Player.Instance.trig = false;
        cameraShake.enabled = false;
        cameraShake.StartCoroutine("FadeCheckIn");
        cameraShake.Reset();
        doorMask.SetActive(true);
    }

    public IEnumerator FadeCheckOut()
    {
        Fade(false, 10);
        StopCoroutine("FadeCheckOut");
        yield return null;
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            triggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = false;
        }
    }
}
