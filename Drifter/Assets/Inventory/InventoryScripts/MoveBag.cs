using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Unity事件管理库

public class MoveBag : MonoBehaviour,IDragHandler
{
    //获得背包坐标
    RectTransform currentRect;  //当前背包的坐标

    public void OnDrag(PointerEventData eventData)
    {
        //中心锚点的位置加上鼠标移动的值
        currentRect.anchoredPosition += eventData.delta;
    }

    void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
}
