using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] Slider healthbar;
    [SerializeField] RuntimeAnimatorController diedAnimatorController;
    [HideInInspector] public bool takeDmg;
    [HideInInspector] public bool died;


    [Header("Hit Flash")]
    [SerializeField] float hitFlashDuration;
    [SerializeField] SkinnedMeshRenderer m_Renderer;
    [SerializeField] Material hitFlashMat;
    Material defaultMat;

    [Header("Hit Scale")]
    [SerializeField] float scaleAmount;
    [SerializeField] float scaleTime;
    Vector3 defaultScale;

    [Header("References")]
    [SerializeField] Animator anim;

    public static PlayerHealth Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        defaultMat = m_Renderer.material;
        defaultScale = transform.localScale;
        
        InitHealthBar();
    }

    void InitHealthBar()
    {
        currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            HitScale();
            StartCoroutine(HitFlash());
        }

        if (takeDmg)
        {
            takeDmg = false;
            HitScale();
            StartCoroutine(HitFlash());
        }

    }
    
    
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        healthbar.value = currentHealth;
        HitScale();
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            Died();
        }
    }

    void Died()
    {
        died = true;
        PlayerMovement.Instance.PlayerIsDied();        
        anim.runtimeAnimatorController = diedAnimatorController;
        UIController.Instance.ActiveWinLosePanel();             
    }


    void HitScale()
    {
        transform.DOScaleX(scaleAmount, scaleTime).OnComplete(() =>
        {
            transform.DOScale(defaultScale, scaleTime).OnComplete(() =>
            {
                transform.DOScaleZ(scaleAmount, scaleTime).OnComplete(() =>
                {
                    transform.DOScale(defaultScale, scaleTime);
                });
            });
        });

        //transform.DOScale(transform.localScale * scaleAmount, scaleTime).OnComplete(() =>
        //{
        //    transform.DOScale(defaultScale, scaleTime);
        //});

    }

    IEnumerator HitFlash()
    {
        m_Renderer.material = hitFlashMat;
        yield return new WaitForSeconds(hitFlashDuration);
        m_Renderer.material = defaultMat;
    }

    
}
