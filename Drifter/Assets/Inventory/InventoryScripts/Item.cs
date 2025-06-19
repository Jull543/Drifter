using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item",menuName ="Inventory/New Item")]

//使用ScriptableObject
public class Item : ScriptableObject
{
    public string itemName; 
    public Sprite itemImage; 
    public int itemHeld; //同种物品的数量
    [TextArea] //多行文字
    public string itemInfo;
    public int itemPrice; //物品价格

    public bool equip;  //装备

}
