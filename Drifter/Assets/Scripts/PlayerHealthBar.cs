using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;

    //显示血量的文字
    public TextMeshProUGUI hpNumber;

    private PlayerScript player;

    public float hurtSpeed;



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void Update()
    {
        hpNumber.text = player.currentHp.ToString() + "/100";
        hpImage.fillAmount = player.currentHp / player.maxHp;
        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }
}
