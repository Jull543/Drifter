using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;

    [HideInInspector] public float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float hurtSpeed = 0.005f;

    private void Start()
    {
        hp = maxHp; //游戏开始的时候，敌人的Hp为最大值
    }

    // Update is called once per frame
    void Update()
    {  
        hpImage.fillAmount = hp / maxHp;
        if(hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }

        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }
}
