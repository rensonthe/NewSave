using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshBean : MonoBehaviour {

    private static FreshBean instance;
    public static FreshBean Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<FreshBean>();
            }
            return instance;
        }
    }

    public SpriteRenderer fadeImage;
    private PlatformController platformController;
    [HideInInspector]
    public BoxCollider2D boxCollider2D;

    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    [HideInInspector]
    public bool isBean;

    [HideInInspector]
    public bool beanUnlock = true;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;
    private Vector3 startPos;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
        boxCollider2D = GetComponent<BoxCollider2D>();
        platformController = GetComponent<PlatformController>();

        Player.Instance.Dead += new DeadEventHandler(ResetBean);
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }
	
	// Update is called once per frame
	void Update () {

        if (beanUnlock)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        }

        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            platformController.enabled = false;
            StartCoroutine("FadeCheckIn");
            transform.position = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(0.8f, 0.8f, 0);
            transform.SetParent(Player.Instance.transform, false);
        }
    }

    public IEnumerator Damaged()
    {
        StartCoroutine("FadeCheckIn");
        transform.SetParent(null);
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(1);
        boxCollider2D.enabled = true;
    }

    public void ResetBean()
    {
        transform.SetParent(null);
        transform.position = startPos;
    }

    public IEnumerator FadeCheckIn()
    {
        Fade(true, 1.5f);
        StopCoroutine("FadeCheckIn");
        yield return null;
    }

    public IEnumerator FadeCheckOut()
    {
        Fade(false, 1.5f);
        StopCoroutine("FadeCheckOut");
        yield return null;
    }
}
