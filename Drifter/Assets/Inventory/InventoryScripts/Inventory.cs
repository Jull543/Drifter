using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//背包系统为列表
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemlist = new List<Item>(); 

}
