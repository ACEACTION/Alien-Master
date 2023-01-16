using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Weapon_Item : MonoBehaviour
{
    [SerializeField] int weaponPrice;
    [SerializeField] WeaponName weaponName;
    float defaultScaleFactor;

    // references
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Weapon_Item_ScObj data;
    Button btn;

    private void Start()
    {
        Init();
        ShowItem();
    }

    void ShowItem()
    {
        defaultScaleFactor = transform.localScale.x;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(
            new Vector3(defaultScaleFactor, defaultScaleFactor, defaultScaleFactor)
            , 0.5f).SetDelay(.3f).OnComplete(DoItemScale);
    }


    void DoItemScale()
    {
        transform.DOScale(new Vector3(data.scaleFactor, data.scaleFactor, data.scaleFactor),
            data.scaleDuration).SetDelay(data.scaleDelay).SetLoops(-1, LoopType.Yoyo);
    }

    void Init()
    {
        priceTxt.text = weaponPrice.ToString();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectItem);
    }

    void SelectItem()
    {
        if (CanBuy())
        {
            BuyItem();
        }
        else
        {
            ShowCantBuyTex();
        }
    }

    private void ShowCantBuyTex()
    {
        var cantBuyText = Instantiate(data.cantBuyTxt,
                        ItemViewController.Instance.itemCanvas.transform);
        cantBuyText.transform.position = ItemViewController.Instance.cantBuyTextPoint.position;
        cantBuyText.transform.localScale = data.cantBuyTxt.transform.localScale;
        cantBuyText.transform.DOMove(cantBuyText.transform.position + new Vector3(0, 50f, 0), 1f)
           .SetEase(data.canBuyTextEaseType).OnComplete(() =>
           {
               Destroy(cantBuyText.gameObject);
           });
    }

    void BuyItem()
    {
        CoinManager.Instance.MinusCoins(weaponPrice);
        
        PlayerWeaponChanger.Instance.SetWeapon(weaponName);

        if (ItemViewController.Instance.closeItemViewAfterBuy)
            ItemViewController.Instance.CloseItemCanvas();
        else
        {
            CloseItem();
        }
    }

    void CloseItem()
    {
        btn.onClick.RemoveAllListeners();
        transform.DOScale(new Vector3(.01f, .01f, .01f), data.scaleDuration)
        .OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    bool CanBuy()
    {
        return CoinManager.Instance.GetTotalCoins() >= weaponPrice;
    }


}
