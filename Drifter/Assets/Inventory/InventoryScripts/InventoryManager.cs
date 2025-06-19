using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    static InventoryManager instance;

    [Header("背包管理器")] 
    [Tooltip("背包所属者")]public Inventory myBag;   //是谁的背包，比如：主角自己、商人
    [Tooltip("背包网格")]public GameObject slotGrid; //背包格
    //[Tooltip("格子")] public Slot slotPrefab;
    public GameObject emptySlot;
    [Tooltip("格子描述")] public Text itemInformation;  //格子中物品的描述


    public List<GameObject> slots = new List<GameObject>();

    //单例模式
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
      
    }

    //在游戏一开始显示当前背包里的东西
    private void OnEnable()
    {
        RefreshItem();
        //默认物品描述什么都不显示
        instance.itemInformation.text = "";
    }
    
    //更新物体描述
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInformation.text = itemDescription;
    }


    //销毁并重新创建
    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear(); //清空列表
        }

        for (int i = 0; i < instance.myBag.itemlist.Count; i++)
        {
            //循环创建回Item列表中的所有Item
            //CreateNewItem(instance.myBag.itemlist[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));    //生成空格子
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);     //摆放好位置
            instance.slots[i].GetComponent<Slot>().slotID = i;  //获得每个格子的ID
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemlist[i]);

        }
    }


}
