using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Unity事件管理库 鼠标的每次点击被生成为一个事件

/*开始拖拽 IBeginDragHandler
 拖拽过程 IDragHandler
 结束拖拽 IEndDragHandler
*/
public class ItemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform originalParent; //原始父级
    public Inventory myBag; //为了连接数据库，需要生成变量来获取背包
    private int currentItemID; //当前物品ID

    public Slot slot;



    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //原始父级等于当前父级，置换两个格子的位置
        currentItemID = originalParent.GetComponent<Slot>().slotID; //当前物品的ID 就是 背包格子对应的ID
        transform.SetParent(transform.parent.parent);   //更改父级避免拖拽被其他格子挡住
        transform.position = eventData.position;    //获得鼠标的位置
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//鼠标当前射线
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //鼠标指向的图片是物品
            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
            {   //得到slot
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
                //itemlist的物品存储位置改变
                var temp = myBag.itemlist[currentItemID];
                //实现物品ID对调
                //当前存储位置的ID改变为鼠标点击的格子的ID
                myBag.itemlist[currentItemID] = myBag.itemlist[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                //鼠标点击格子的ID变为原存储格子的ID
                myBag.itemlist[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position; 
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;  //射线阻挡开启，不然无法再次选中移动的物品
                return;
            }

        if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
        {
            //如果鼠标指向的位置是空的，直接检测到Slot下面
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            //itemlist物品存储位置改变
            myBag.itemlist[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemlist[currentItemID];
            //当拖动到不是自己的其他空格子上时
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID)
                myBag.itemlist[currentItemID] = null;

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
    }


    //其他任何位置都归位
    transform.SetParent(originalParent);
    transform.position = originalParent.position;
    GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("当前物品的ID为" + currentItemID);
    }

}
