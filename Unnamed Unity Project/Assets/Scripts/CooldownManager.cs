using UnityEngine;
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

    private float orbLaunchTimer;
    private float orbLaunchCooldown = 4f;
    private bool orbLaunchisAllowed;
    public bool OrbLaunchIsAllowed { get { return orbLaunchisAllowed; } }

    private float corruptionTimer;
    private float corruptionCharges;
    private bool corruptionIsAllowed = true;
    public bool CorruptionIsAllowed { get { return corruptionIsAllowed; } }

    // Use this for initialization
    void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        OrbLaunchCooldown();
        CorruptionDuration();
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

    public void Corruption()
    {
        if (corruptionIsAllowed)
        {
            corruptionIconImage.fillAmount = 1;
            corruptionIconImage.gameObject.SetActive(true);
            corruptionIsAllowed = false;
        }
    }

    public void CorruptionDuration()
    {
        if (!corruptionIsAllowed)
        {
            corruptionTimer += Time.deltaTime;
            HandleCooldown(corruptionIconImage, PlayerController.Instance.corruptionDuration - corruptionTimer, PlayerController.Instance.corruptionDuration);
            if (corruptionTimer >= PlayerController.Instance.corruptionDuration)
            {
                corruptionIsAllowed = true;
                corruptionTimer = 0;
                corruptionIconImage.gameObject.SetActive(false);
            }
        }
    }
}
