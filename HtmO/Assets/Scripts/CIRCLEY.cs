using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIRCLEY : MonoBehaviour {

    public GameObject particleObject;
    public GameObject activateParticle;
    public GameObject EPrompt;
    public CameraShake cameraShake;

    public ParticleSystem[] streams;
    public float hSliderValue = 1.0f;
    private bool canFade = false;
    private bool triggered = false;

    void Start()
    {
        for (int i = 0; i < streams.Length; i++)
        {
            var colorOverLifetime = streams[i].colorOverLifetime;
            colorOverLifetime.enabled = true;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );

            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        }
    }

    void Update()
    {
        if (canFade)
        {
            hSliderValue -= Time.deltaTime;
            if (hSliderValue <= 0)
            {
                canFade = false;
            }
        }

        for (int i = 0; i < streams.Length; i++)
        {
            var main = streams[i].main;
            main.startLifetime = hSliderValue;
        }
        if (Input.GetKeyDown(KeyCode.E) && triggered && Player.Instance.trig == false)
        {
            StartCoroutine(CIRCLEYSave());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Player.Instance.canCircley || Input.GetKeyDown(KeyCode.RightShift) && Player.Instance.canCircley == true)
        {
            CIRCLEYDeploy();
        }
    }


    public void CIRCLEYDeploy()
    {
        Player.Instance.circleyCharges--;
        Instantiate(this.gameObject, Player.Instance.transform.localPosition + new Vector3(0,1.85f,0),Quaternion.identity);
        //Play animation
        //Disable player while animation is active
    }

    IEnumerator CIRCLEYSave()
    {
        Player.Instance.healthStat.CurrentVal = Player.Instance.healthStat.MaxVal;
        Player.Instance.startPos = transform.position;
        Player.Instance.moveSpeed = 0;
        Player.Instance.trig = true;
        cameraShake.enabled = true;
        activateParticle.SetActive(true);
        cameraShake.StartCoroutine("FadeCheckOut");
        yield return new WaitForSeconds(1.9f);
        activateParticle.SetActive(false);
        Player.Instance.moveSpeed = 6;
        Player.Instance.trig = false;
        cameraShake.enabled = false;
        cameraShake.StartCoroutine("FadeCheckIn");
        cameraShake.Reset();
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(hSliderValue <= 1)
            {
                hSliderValue += Time.deltaTime;
            }
            else
            {
                return;
            }

            particleObject.SetActive(true);
            EPrompt.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = false;
            canFade = true;
            EPrompt.SetActive(false);
        }
    }
}
