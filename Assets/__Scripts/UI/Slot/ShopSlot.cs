using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private ItemToolTipUpdate toolTipUpdate;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private Image itemImage;

    private int itemID =0;
    private int buyPrice;

    public void SetToolTipUpdater(ItemToolTip toolTip)
    {
        toolTipUpdate.SetToolTip(toolTip);
    }
    public void UpdateSlot()
    {

    }
    public void SetData(int id)         //���� ���� �ʱ� ����
    {
        itemID = id;
        ItemData data = ItemDataManager.Instance.FindItem(itemID);
        nameText.text = data.itemName.ToString();
    

        buyPriceText.text = data.price.ToString();

        itemImage.sprite = data.itemIcon;

        buyPrice = data.price;
        toolTipUpdate.SetData(itemID, false);
    }

    public void BuyItem()
    {
        Debug.Log(PlayerController.Instance._Inventory);
        if(PlayerController.Instance._Inventory.BuyItem(itemID,buyPrice))
        {
            return;
        }
        Debug.Log("���� ����");
    }
}
