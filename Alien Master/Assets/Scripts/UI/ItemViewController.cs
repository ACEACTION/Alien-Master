using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemList
{
    public List<GameObject> types;
}


public class ItemViewController : MonoBehaviour
{

    public Transform cantBuyTextPoint;
    public bool closeItemViewAfterBuy;
    [SerializeField] List<ItemList> items;
    public GameObject itemCanvas;

    public static ItemViewController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SelectRandomItem();
    }

    public void SelectRandomItem()
    {
        foreach (ItemList i in items)
        {
            Instantiate(i.types[GetRandomIndex(i.types.Count)], transform);
        }
    }


    int GetRandomIndex(int length)
    {
        return Random.Range(0, length);
    }    


    public void CloseItemCanvas()
    {        
        itemCanvas.SetActive(false);
    }

}
