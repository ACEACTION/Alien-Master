using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectItem : MonoBehaviour
{
    [SerializeField] bool doItemScroller;
    [SerializeField] GameObject[] itemScroller;
    [SerializeField] float scrollTime;
    [SerializeField] float moveTime;
    Vector3[] originItemPos;

    void Start()
    {
        originItemPos = new Vector3[itemScroller.Length];
        for (int i = 0; i < itemScroller.Length; i++)
        {
            originItemPos[i] = itemScroller[i].transform.position;
        }
    }

    void Update()
    {
        if (doItemScroller)
        {
            doItemScroller = false;
            StartCoroutine(ScrollItem());
        }
    }

    
    IEnumerator ScrollItem()
    {
        for (int i = 0; i < itemScroller.Length; i++)
        {
            itemScroller[i].transform.position = originItemPos[i];
        }

        for (int i = 0; i < itemScroller.Length; i++)
        {
            yield return new WaitForSeconds(scrollTime);
            itemScroller[i].transform.DOMoveY(-5f, moveTime);
        }
    }

}
