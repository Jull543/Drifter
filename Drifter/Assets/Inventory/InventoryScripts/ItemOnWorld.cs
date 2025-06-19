using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;   //物品的属性所属数据库
    public Inventory playerInventory;   //所属背包



    public void AddNewItem()
    {
        //如果背包中没有物体,背包内加入这个物体
        if(!playerInventory.itemlist.Contains(thisItem))
        {
            //playerInventory.itemlist.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);
            for (int i = 0; i < playerInventory.itemlist.Count; i++)
            {
                if(playerInventory.itemlist[i] == null)
                {
                    playerInventory.itemlist[i] = thisItem;
                    thisItem.itemHeld += 1;
                    break;
                }
            }
        }
        else
        {//背包中有该物体，则将该物体数目增加1
            thisItem.itemHeld += 1;
        }

        InventoryManager.RefreshItem();
    }

}
