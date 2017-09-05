using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownManager : MonoBehaviour
{
    [SerializeField]
    private Image fireballIconImage;

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

    private float fireballTimer;
    private float fireballCooldown = 4f;
    private bool fireballIsAllowed;
    public bool FireballIsAllowed { get { return fireballIsAllowed; } }

    // Use this for initialization
    void Start()
    {
        fireballTimer = fireballCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        FireballCooldown();
    }

    private void HandleCooldown(Image content, float timeLeft, float maxTime)
    {
        float fillAmount = timeLeft / maxTime;
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
        }
    }

    public void Fireball()
    {
        if (fireballIsAllowed)
        {
            fireballIconImage.fillAmount = 0;
            fireballIsAllowed = false;
        }
    }

    public void FireballCooldown()
    {
        if (!fireballIsAllowed)
        {
            fireballTimer += Time.deltaTime;
            HandleCooldown(fireballIconImage, fireballTimer, fireballCooldown);
            if (fireballTimer >= fireballCooldown)
            {
                fireballIsAllowed = true;
                fireballTimer = 0;
            }
        }
    }
}