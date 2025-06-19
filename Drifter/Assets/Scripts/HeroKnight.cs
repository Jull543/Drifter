using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed;
    [SerializeField] float      m_jumpForce;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Ground       m_groundSensor;
    private bool                m_grounded = false;
    private int                 m_currentJump = 0;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_stratspeed;
    private Scene               m_scene;
    private GameObject[] dialogs;

    [HideInInspector] public int Health;

    public bool doubleJump;
    public AudioClip[] audios;


    void Start ()
    {
        Health = 100;
        m_stratspeed = m_speed;
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Ground>();
        m_animator = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        m_scene = SceneManager.GetActiveScene();

    }

    void Update ()
    {
        dialogs = GameObject.FindGameObjectsWithTag("dialog");

        //控制攻击组的计时器
        m_timeSinceAttack += Time.deltaTime;

        //判断是否刚落到地面上
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //判断是否刚刚开始下落
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- 控制输入和移动 --
        float inputX = Input.GetAxis("Horizontal");

        // 控制人物朝向
        if (inputX > 0)
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
            
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }

        //控制人物在空中和地面的水平速度
        if (!m_groundSensor.State())
        {
            m_speed = m_stratspeed;
        }
        else
        {
            m_speed = m_stratspeed * 2f;
        }

        // 人物位置变化
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //设置参数"AirSpeedY"
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- 控制动画 --

        //Death
        if (Health==0)
        {
            m_animator.SetTrigger("Death");
            Destroy(gameObject, 2f);
        }           

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.5f && !ShowDailog() && m_scene.name!="Country")
        {
            m_currentAttack++;

            // 两种攻击之前循环切换
            if (m_currentAttack > 2)
                m_currentAttack = 1;

            //设置第一种攻击到第二种攻击的间隔时间
            if (m_timeSinceAttack > 1.5f)
                m_currentAttack = 1;

            if (m_currentAttack == 1)
            {
                //播放普攻音效
                this.GetComponent<AudioSource>().clip = audios[0];
                this.GetComponent<AudioSource>().Play();
            }
            else if(m_currentAttack == 2)
            {
                //播放重击音效
                this.GetComponent<AudioSource>().clip = audios[1];
                this.GetComponent<AudioSource>().Play();
            }
            m_animator.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;                           //重置攻击冷却时间
        }
          
        //Jump
        else if (Input.GetKeyDown("space"))
        {
            if (m_grounded)
            {
                m_currentJump = 2;
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            }
            else
            {
                if(m_currentJump == 1 && doubleJump)
                {
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                }
            }
            m_currentJump--;
        }


        //Run（Mathf.Epsilon最小正数）
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

   bool ShowDailog()
    {
        bool temp = false;
        foreach (var dialog in dialogs)
        {
            if (dialog.activeSelf == true)
            {
                temp=true;
            }
        }
        return temp;
    }
}
