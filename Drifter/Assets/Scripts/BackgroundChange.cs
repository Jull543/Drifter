using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundChange : MonoBehaviour
{
    private float sumTime;
    private int count;
    public Tilemap[] bgList;

    // Start is called before the first frame update
    void Start()
    {
        sumTime = 8;
        count = 1;
    }

    // Update is called once per frame
    void Update()
    {
        sumTime = sumTime - Time.deltaTime * count;
        if (sumTime < 0)
        {
            count = -1;
        }

        if (sumTime > 8)
        {
            count = 1;
        }

        foreach (var bg in bgList)
        {
            float alpha = sumTime / 8;
            bg.color = new Color(1, 1, 1, alpha);
        }
    }
}
