using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Fungus;

public class CameraFollow : MonoBehaviour {

    public Flowchart flowchart;
    public GameObject player;
    public Transform target;
    public float speed;
    private bool panning = false;
    private bool _keyPressed = false;
    private bool fading = false;
    private bool displayed = false;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;
    public float smoothTimeY;
    public float smoothTimeX;

    public bool bounds;
    public Vector2 velocity;

    public Image fadeImage;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    private Vector3 startPos;
    private Vector3 endPosition;
    public bool isLerping = false;
    private float startTime;
    private float length;

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    private void Update()
    {
        if(Tutorial.Instance.peekDown == true)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += -Vector3.up * 24.5f * Time.deltaTime;
                Player.Instance.SetInactive();
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                Player.Instance.SetActive();
            }
        }

        if (!panning)
        {
            FollowPlayer();
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        if (_keyPressed == true)
        {
            panning = false;
            Player.Instance.SetActive();
        }

        if (isLerping)
        {
            LerpToPosition();
        }

        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    IEnumerator FadeCheck()
    {
        fading = true;
        Fade(true, 1.25f);
        yield return new WaitForSeconds(5f);
        Fade(false, 2f);
        yield return new WaitForSeconds(2f);
        fading = false;
        StopCoroutine("FadeCheck");
    }

    void LerpToPosition()
    {
        float discover = (Time.time - startTime) * 5f;
        float journey = discover / length;
        transform.position = Vector3.Lerp(startPos, endPosition, journey);
        if (journey >= 1)
        {
            isLerping = false;
        }
    }

    public void StartLerp(Vector3 endPosition)
    {
        isLerping = true;
        startPos = transform.position;
        this.endPosition = endPosition;
        startTime = Time.time;
        length = Vector3.Distance(startPos, endPosition);
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {

    }

    private void FollowPlayer()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }

    public IEnumerator CameraPan()
    {
        panning = true;
        Player.Instance.SetInactive();
        yield return new WaitForSeconds(4.25f);
        if (!displayed)
        {
            displayed = true;
        }
    }

    public IEnumerator WaitForKeyPress()
    {
        while (!_keyPressed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _keyPressed = true;
                UIManager.Instance.isAllowed = true;
                break;
            }
            if (!fading)
            {
                StartCoroutine("FadeCheck");
            }
            yield return 0;
        }
    }

    public void StartPrompt()
    {
        StartCoroutine("WaitForKeyPress");
    }

    public void CameraPanTrigger()
    {
        StartCoroutine(CameraPan());
    }
}