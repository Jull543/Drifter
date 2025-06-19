using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sensor_Ground : MonoBehaviour
{

    private int m_ColCount = -1;
    private Scene m_scene;

    private float m_ColCountLadder1 = 0;
    private int m_ColCountWood1 = 0;
    private int m_ColCountWood2 = 0;
    private int m_ColCountSwitch1 = 0;
    private int m_ColCountSwitch2 = 0;
    private int m_ColCountSwitch3 = 0;
    private int m_ColCountSwitch4 = 0;

    [Header("机关--关卡1")]
    public GameObject m_Ladder1;
    public GameObject m_Ladder2;
    public GameObject m_Bridge2;
    public GameObject m_Bridge3;
    public GameObject m_Switch1;
    public GameObject m_Switch2;
    public GameObject m_Switch3;
    public GameObject m_Switch4;
    public GameObject m_Trap1;
    public GameObject m_Trap2;

    [Header("机关--关卡2")]
    public GameObject m_Ladder3;
    public GameObject KillZone;

    [Header("玩家")]
    public GameObject player;

    private Vector3 Ladder_position;
    private bool moveChange = false;
    private bool toStop1 = true;
    private float startY1 = 0;
    private float stopY1 = 2.8f;
    private float startX1 = 0;
    private float stopX1 = 2.8f;
    private float speed1 = 1f;
    private bool toStop2 = false;
    private float speed2 = 1f;
    private float stopX2 = -8;
    private float startX2 = -3;

    public bool State()
    {
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_ColCount++;
        switch (other.name)
        {
            case "Switch1":
                m_ColCountSwitch2 = 0;
                m_ColCountSwitch1 += 1;
                if (m_ColCountSwitch1 > 3)
                {
                    m_ColCountSwitch1 = m_ColCountSwitch1 - 3;
                }
                player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[6];
                player.GetComponent<AudioSource>().Play();
                break;
            case "Switch2":
                m_ColCountSwitch2 += 1;
                m_Switch2.transform.GetComponent<Animator>().SetTrigger("Switch2");
                if (m_ColCountSwitch2 > 2)
                {
                    m_ColCountSwitch2 = m_ColCountSwitch2 - 2;
                }
                player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[6];
                player.GetComponent<AudioSource>().Play();
                break;
            case "Switch3":
                m_ColCountSwitch4 = 0;
                m_ColCountSwitch3 = 1;
                m_Switch3.transform.GetComponent<Animator>().SetTrigger("Switch3");
                player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[6];
                player.GetComponent<AudioSource>().Play();
                break;
            case "Switch4":
                m_ColCountSwitch3 = 0;
                m_ColCountSwitch4 = 1;
                m_Switch4.transform.GetComponent<Animator>().SetTrigger("Switch4");
                player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[6];
                player.GetComponent<AudioSource>().Play();
                break;
            case "Ladder2":
                Ladder_position = m_Ladder2.transform.position;
                break;
        }

        //进入死亡区域
        if (other.tag == "KillZone")
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.name)
        {
            case "Ladder1":
                m_ColCountLadder1 += Time.deltaTime;
                break;
            case "Wood1":
                m_ColCountWood1 = 1;
                break;
            case "Wood2":
                m_ColCountWood2 = 1;
                break;
            case "Ladder2":
                moveChange = true;
                player.transform.position = new Vector3(m_Ladder2.transform.position.x+36.5f, player.transform.position.y, 0);
                break;
        }

        //刺刀
        if (other.tag == "Traps")
        {
            player.GetComponent<PlayerScript>().currentHp -= 1f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
        switch (other.name)
        {
            case "Switch1":
                if(m_ColCountSwitch1 == 3)
                {
                    m_ColCountSwitch1 = 0;
                }
                else
                {
                    m_ColCountSwitch1 = 2;
                }
                break;

            case "Ladder2":
                moveChange = false;
                break;
        }
    }

    void Update()
    {
        m_scene = SceneManager.GetActiveScene();

        //消失平台
        if (m_ColCountLadder1 > 0.8)
        {
            m_Ladder1.SetActive(false);
            if(m_ColCount == 1)
            {
                m_ColCount = m_ColCount - 1;
            }            
        }

        //出现平台
        if (m_ColCountWood1 == 1 && Input.GetKeyDown("space"))
        {
            m_Bridge2.SetActive(true);
        }

        if (m_ColCountWood2 == 1 && Input.GetKeyDown("space"))
        {
            m_Bridge3.SetActive(true);
        }

        //旋转机关1
        if (m_ColCountSwitch1 == 1)
        {
            m_Switch1.transform.localScale = new Vector3(-2.5f, 2.5f, 0);
            m_Trap1.transform.localRotation = Quaternion.Slerp(m_Trap1.transform.localRotation, new Quaternion(0, 0, -1, 1), 0.01f);
        }

        if (m_ColCountSwitch1 == 3)
        {
            m_Switch1.transform.localScale = new Vector3(2.5f, 2.5f, 0);
            m_Trap1.transform.localRotation = Quaternion.Slerp(m_Trap1.transform.localRotation, new Quaternion(0, 0, 0, 1), 0.01f);

        }

        if (m_ColCountSwitch2 == 1)
        {
            m_Trap1.transform.localRotation = Quaternion.Slerp(m_Trap1.transform.localRotation, new Quaternion(0, 0, -1, 1), 0.01f);
        }

        if (m_ColCountSwitch2 == 2)
        {
            m_Trap1.transform.localRotation = Quaternion.Slerp(m_Trap1.transform.localRotation, new Quaternion(0, 0, 0, 1), 0.01f);
        }


        //旋转机关2
        if (m_ColCountSwitch3 == 1)
        {
            m_Trap2.transform.localRotation = Quaternion.Slerp(m_Trap2.transform.localRotation, new Quaternion(0, 0, 0, 1), 0.08f);
            m_Trap2.transform.GetChild(1).gameObject.GetComponent<CompositeCollider2D>().isTrigger = false;
            m_Trap2.transform.GetChild(0).gameObject.GetComponent<CompositeCollider2D>().isTrigger = false;

        }

        if (m_ColCountSwitch4 == 1)
        {
            m_Trap2.transform.localRotation = Quaternion.Slerp(m_Trap2.transform.localRotation, new Quaternion(0, 0, 90, 1), 0.0005f);
            m_Trap2.transform.GetChild(1).gameObject.GetComponent<CompositeCollider2D>().isTrigger = true;
            m_Trap2.transform.GetChild(0).gameObject.GetComponent<CompositeCollider2D>().isTrigger = true;

        }

        //移动台
        if (moveChange)
        {
            Vector3 tempPosition = m_Ladder2.transform.position;
            if (toStop1)
            {
                tempPosition = Vector3.MoveTowards(tempPosition, new Vector3(stopX1, tempPosition.y, 0), speed1 * Time.deltaTime);
                m_Ladder2.transform.position = tempPosition;
                if (m_Ladder2.transform.position.x == stopX1)
                {
                    toStop1 = false;
                }
            }
            else if (!toStop1)
            {
                tempPosition = Vector3.MoveTowards(tempPosition, new Vector3(startX1, tempPosition.y, 0), speed1 * Time.deltaTime);
                m_Ladder2.transform.position = tempPosition;
                if (m_Ladder2.transform.position.x == startX1)
                {
                    toStop1 = true;
                }
            }
        }
        //TODO:之后需要修改
        else if(m_scene.name=="SampleScene")
        {
            Vector3 tempPosition = m_Ladder2.transform.position;
            if (toStop1)
            {
                tempPosition = Vector3.MoveTowards(tempPosition,new Vector3(tempPosition.x,stopY1,0), speed1 * Time.deltaTime);
                m_Ladder2.transform.position = tempPosition;
                if (m_Ladder2.transform.position.y == stopY1)
                {
                    toStop1 = false;
                }
            }
            else if (!toStop1)
            {
                tempPosition = Vector3.MoveTowards(tempPosition, new Vector3(tempPosition.x, startY1, 0), speed1 * Time.deltaTime);
                m_Ladder2.transform.position = tempPosition;
                if (m_Ladder2.transform.position.y == startY1)
                {
                    toStop1 = true;
                }
            }
        }

        //掉落--重新加载场景
        if (m_ColCount == -1)
        {
            SceneManager.LoadScene("SampleScene");
        }

        //TODO:改名字
        if (m_scene.name == "Dark")
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        Vector3 tempPosition = m_Ladder3.transform.position;
        if (toStop2)
        {
            tempPosition = Vector3.MoveTowards(tempPosition, new Vector3(stopX2, tempPosition.y, 0), speed2 * Time.deltaTime);
            m_Ladder3.transform.position = tempPosition;
            if (m_Ladder3.transform.position.x == stopX2)
            {
                toStop2 = false;
            }
        }
        else if (!toStop2)
        {
            tempPosition = Vector3.MoveTowards(tempPosition, new Vector3(startX2, tempPosition.y, 0), speed2 * Time.deltaTime);
            m_Ladder3.transform.position = tempPosition;
            if (m_Ladder3.transform.position.x == startX2)
            {
                toStop2 = true;
            }
        }
    }
}
