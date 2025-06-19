using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerClickHandler
{

    public int slotID; //空格ID 等于 物品ID
    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum; // 每个物品的数量
    public string slotInfo;

    public Inventory playerInventory;   //所属背包

    public GameObject itemInSlot;



    //点击物体显示物体描述
    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {

        }
    }


    public void SetupSlot(Item item)
    {
        if(item == null) //没有物品时
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.itemImage;
        //当持有物数量为1时，左下角数字不显示
        if (item.itemHeld == 1)
        {
            slotNum.text = null;
        }
        else
        {
            slotNum.text = item.itemHeld.ToString();
        }
        slotInfo = item.itemInfo;   //获得物品信息
    }


}
