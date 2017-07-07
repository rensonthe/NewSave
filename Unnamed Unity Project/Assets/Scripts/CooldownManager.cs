﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownManager : MonoBehaviour {

    private static CooldownManager instance;

    public static CooldownManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CooldownManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image orbLaunchIconImage;
    [SerializeField]
    private Image corruptionIconImage;
    [SerializeField]
    private Image bulwarkIconImage;

    private float orbLaunchTimer;
    private float orbLaunchCooldown = 4f;
    private bool orbLaunchisAllowed;
    public bool OrbLaunchIsAllowed { get { return orbLaunchisAllowed; } }

    private float corruptionTimer;
    public float corruptionCharges;
    private bool corruptionActive = false;
    private bool corruptionIsAllowed = false;
    public bool CorruptionIsAllowed { get { return corruptionIsAllowed; } }

    private float bulwarkTimer;
    private float bulwarkDuration = 5;
    private float bulwarkCooldown = 10f;
    private bool bulwarkIsAllowed;
    public bool BulwarkIsAllowed { get { return bulwarkIsAllowed; } }

    // Use this for initialization
    void Start () {
        bulwarkTimer = bulwarkCooldown;
        orbLaunchTimer = orbLaunchCooldown;
	}

    // Update is called once per frame
    void Update()
    {
        OrbLaunchCooldown();

            CorruptionDuration();


        BulwarkCooldown();
        if(PlayerController.Instance.CurrentCorruption == 100 && corruptionCharges == 0 && !PlayerController.Instance.isInCorruption)
        {
            corruptionCharges++;
        }
        if(corruptionCharges == 1)
        {
            corruptionIsAllowed = true;
        }
    }

    private void HandleCooldown(Image content, float timeLeft, float maxTime)
    {
        float fillAmount = timeLeft / maxTime;
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
        }
    }

    public void LaunchOrb()
    {
        if (orbLaunchisAllowed)
        {
            orbLaunchIconImage.fillAmount = 0;
            orbLaunchIconImage.gameObject.SetActive(true);
            orbLaunchisAllowed = false;
        }
    }

    public void OrbLaunchCooldown()
    {
        if (!orbLaunchisAllowed)
        {
            orbLaunchTimer += Time.deltaTime;
            HandleCooldown(orbLaunchIconImage, orbLaunchTimer, orbLaunchCooldown);
            if (orbLaunchTimer >= orbLaunchCooldown)
            {
                orbLaunchisAllowed = true;
                orbLaunchTimer = 0;
                orbLaunchIconImage.gameObject.SetActive(false);
            }
        }
    }

    public void Bulwark()
    {
        if (bulwarkIsAllowed)
        {
            PlayerController.Instance.MyAnimator.SetTrigger("bulwark");
            PlayerController.Instance.soulsStat.CurrentVal -= PlayerController.Instance.bulwarkVal;
            PlayerController.Instance.bulwarkIsActive = true;
            bulwarkIconImage.fillAmount = 0;
            bulwarkIconImage.gameObject.SetActive(true);
            PlayerController.Instance.bulwarkPrefab.gameObject.SetActive(true);
            PlayerController.Instance.bulwarkIndicator.gameObject.SetActive(true);
            bulwarkIsAllowed = false;
        }
    }

    public void BulwarkCooldown()
    {
        if (!bulwarkIsAllowed)
        {
            bulwarkTimer += Time.deltaTime;
            HandleCooldown(bulwarkIconImage, bulwarkTimer, bulwarkCooldown);
            if (bulwarkTimer >= bulwarkDuration)
            {
                PlayerController.Instance.bulwarkIsActive = false;
                PlayerController.Instance.bulwarkPrefab.gameObject.SetActive(false);
                PlayerController.Instance.bulwarkIndicator.gameObject.SetActive(false);
            }
            if (bulwarkTimer >= bulwarkCooldown)
            {
                bulwarkIsAllowed = true;
                bulwarkTimer = 0;
                bulwarkIconImage.gameObject.SetActive(false);
            }
        }
    }

    public void Corruption()
    {
        if (corruptionIsAllowed)
        {
            corruptionActive = true;
            corruptionIsAllowed = false;
            corruptionCharges--;
        }
    }

    public void CorruptionDuration()
    {
        if (corruptionActive)
        {
            corruptionTimer += Time.deltaTime;
            HandleCooldown(corruptionIconImage, PlayerController.Instance.corruptionDuration - corruptionTimer, PlayerController.Instance.corruptionDuration);
            if (corruptionTimer >= PlayerController.Instance.corruptionDuration)
            {
                corruptionActive = false;
                corruptionIsAllowed = true;
                corruptionTimer = 30;
            }
        }
    }
}
