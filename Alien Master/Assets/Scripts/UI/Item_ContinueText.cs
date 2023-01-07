using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_ContinueText : MonoBehaviour
{
    [SerializeField] float scaleDuration;
    [SerializeField] float scaleFactor;
    Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
        DOScale();
    }

    void DOScale()
    {
        transform.DOScale(new Vector3(scaleFactor, scaleFactor, scaleFactor), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo).OnComplete(() =>
            {
                transform.DOScale(defaultScale, scaleDuration);
            });
    }




}
