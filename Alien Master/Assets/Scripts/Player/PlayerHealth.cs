using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{

    [Header("Health")]
    public bool takeDmg;

    [Header("Hit Flash")]
    [SerializeField] float hitFlashDuration;
    [SerializeField] SkinnedMeshRenderer m_Renderer;
    [SerializeField] Material hitFlashMat;
    Material defaultMat;

    [Header("Hit Scale")]
    [SerializeField] float scaleAmount;
    [SerializeField] float scaleTime;
    Vector3 defaultScale;

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
        HitScale();
        StartCoroutine(HitFlash());
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
