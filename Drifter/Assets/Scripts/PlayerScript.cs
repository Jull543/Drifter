using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //主角的血量设置
    [HideInInspector] public float maxHp = 100.0f;
    [HideInInspector] public float currentHp = 100.0f;

    //背包系统
    public GameObject myBag;
    bool isOpen;
    public Inventory playerInventory;   //所属背包
    public Item thisItem;   //物品的属性所属数据库

    //草药数据库
    public Item Item_herb1;
    public Item Item_herb2;
    public Item Item_herb3;
    public Item Item_herb4;
    //场景中草药
    public GameObject herb1;
    public GameObject herb2;
    public GameObject herb3;
    public GameObject herb4;

    //对话
    public GameObject dialog;


    //金币
    [Header("金币收集")]
    [Tooltip("金币数量显示")] public TextMeshProUGUI coinNumText; //显示金币数量的文字
    public int coinNum = 0;    //金币数量


    //UI上Boss血条的显示
    public GameObject BossHp;
    //Boss
    public GameObject Boss;

    private Scene scene;
    private GameObject end;


    //主角开始前的状态
    private void Awake()
    {
        currentHp = maxHp;
    }

    void Start()
    {

    }

    void Update()
    {
        //限制生命
        lifeLimited();
        
        //使用生命药水
        UseLifePotion();

        //检测背包开关
        OpenMyBag();

        //检测草药
        ExamHerbs();

        //检测结局
        ExamFinal();

        scene = SceneManager.GetActiveScene();
        end = GameObject.FindWithTag("End");

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //TODO:收集金币
        //有时间可增加：金币收集动画、吸附金币、金币飞起
        if (other.gameObject.tag == "Coin")
        {
            //other.transform.GetComponent<Animator>().SetTrigger("coinGet");
            Destroy(other.gameObject);
            coinNum += 1;
            coinNumText.text = coinNum.ToString();
            this.GetComponent<AudioSource>().clip = this.GetComponent<HeroKnight>().audios[7];
            this.GetComponent<AudioSource>().Play();
        }

        //收集药水
        //在角色生命为100以下时，拾取生命药水后，自动喝下药水
        if (other.gameObject.tag == "LifePotion")
        {
            //if (currentHp < 100)
            //{
            //    currentHp += 10;
            //}
            //else
            //{
                other.GetComponent<ItemOnWorld>().AddNewItem();
            //}
            Destroy(other.gameObject);
            this.GetComponent<AudioSource>().clip = this.GetComponent<HeroKnight>().audios[7];
            this.GetComponent<AudioSource>().Play();
        }


        if (other.gameObject.name == "BossZone")
        {
            BossHp.SetActive(true);
        }

    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "BossZone")
        {
            BossHp.SetActive(false);
        }
    }

    private void lifeLimited()
    {
        //判断生命值是否超过上下限
        if (currentHp < 0)
        {
            currentHp = 0;

        }
        if (currentHp > 100)
        {
            currentHp = 100;
        }
    }

    void OpenMyBag()
    {
        //按下B键开启背包或关闭背包
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            isOpen = myBag.activeSelf; //背包的状态为Inspector中是否勾选的状态
            myBag.SetActive(!isOpen);
            InventoryManager.RefreshItem();
        }
    }

    void UseLifePotion()
    {
        //回血药水
        if (playerInventory.itemlist.Contains(thisItem))    //角色背包里存在生命药水
        {
            if (thisItem.itemHeld > 0)  //持有量大于0
            {
                if (currentHp < 100) //此刻血量小于100
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))   //按下1回血
                    {
                        thisItem.itemHeld -= 1; //持有量减1
                        currentHp += 10.0f; //当前血量+10
                    }
                }
            }
            else   //持有量如果小于等于0
            {
                for (int i = 0; i < playerInventory.itemlist.Count; i++)    //遍历背包列表
                {
                    if (playerInventory.itemlist[i] == thisItem)    //将生命药水从列表中移除
                    {
                        playerInventory.itemlist[i] = null;
                    }
                }
            }
        }

    }

    void ExamHerbs()
    {
        if (playerInventory.itemlist.Contains(Item_herb1))
        {
            Destroy(herb1);
        }
        if (playerInventory.itemlist.Contains(Item_herb2))
        {
            Destroy(herb2);
        }
        if (playerInventory.itemlist.Contains(Item_herb3))
        {
            Destroy(herb3);
        }
        if (playerInventory.itemlist.Contains(Item_herb4))
        {
            Destroy(herb4);
        }
    }

    void ExamFinal()
    {
        if(playerInventory.itemlist.Contains(Item_herb1) && playerInventory.itemlist.Contains(Item_herb2) &&
            playerInventory.itemlist.Contains(Item_herb3) && playerInventory.itemlist.Contains(Item_herb4))
        {
            dialog.SetActive(true);
        }


    }


}
