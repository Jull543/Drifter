using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum BossStates { GUARD, CHASE, DEAD}  //Boss状态机(3种状态)：守卫/追击/死亡
[RequireComponent(typeof(NavMeshAgent))]                //AI组件(没有则自动添加)

public class BossController : MonoBehaviour
{
    [HideInInspector] public BossStates bossStates;

    private GameObject boss;
    private GameObject trueBoss;
    private GameObject attackTarget;
    private GameObject player;

    [Header("范围设置")]
    [Tooltip("检测范围")] public float detectionRange;
    [Tooltip("小怪攻击范围")] public float attackRange;
    [Tooltip("主角攻击范围")] public float hurtRange;

    [Header("Boss死亡后出现的草药")]
    public GameObject herb;

    private NavMeshAgent agent;
    private float speed = 3;            //走路和追击速度不同

    //时间控制参数
    private float lookAtTime = 2.0f;
    private float remainLookAtTime;
    private float lastAttackTime;

    //血量参数
    [HideInInspector] public float Health;

    //位置参数
    private Vector3 guardPos;           //Boss初始位置

    //动画动制参数
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;

    private float bashNum;
    private Animator bossAnim;
    private Animator playerAnim;



    // Start is called before the first frame update
    void Start()
    {
         bossStates = BossStates.GUARD;
    }

    void Awake()
    {
        boss = this.gameObject.transform.GetChild(0).gameObject;
        trueBoss = boss.transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        bossAnim = trueBoss.GetComponent<Animator>();
        playerAnim = player.transform.GetChild(0).gameObject.GetComponent<Animator>();

        Health = 100;
        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health == 0)
        {
            isDead = true;
        }

        SwitchStates();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        boss.transform.position = new Vector3(transform.position.x, boss.transform.position.y, 0);
        trueBoss.transform.position = new Vector3(boss.transform.position.x+2.2f* boss.transform.localScale.x, trueBoss.transform.position.y, 0);
    }

    void SwitchAnimation()
    {
        bossAnim.SetBool("Walk", isWalk);
        bossAnim.SetBool("Chase", isChase);
        bossAnim.SetBool("Follow", isFollow);
        bossAnim.SetBool("Dead", isDead);
    }

    //切换状态
    void SwitchStates()
    {
        if (isDead)
        {
            bossStates = BossStates.DEAD;
        }
        else if (FoundPlayer())
        {
            bossStates = BossStates.CHASE;
        }
        switch (bossStates)
        {
            case BossStates.GUARD:
                AdjustOrientation();
                isChase = false;
                

                //判断是否在原位置（追踪拉脱后回到原位置）
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector2.SqrMagnitude(guardPos - transform.position) <= 1.5)
                    {

                        //回到原位置后动画切换
                        isWalk = false;

                        //回到原位置后恢复朝向
                        boss.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                break;
            
            case BossStates.CHASE:
                AdjustOrientation();
                isWalk = false;
                isChase = true;
                agent.speed = speed;

                if (!FoundPlayer())
                {
                    //拉脱回到上一个状态
                    isFollow = false;

                    //拉脱后停在当前位置观望一会儿
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        bossStates = BossStates.GUARD;
                    }
                }
                else
                {
                    //在攻击范围内攻击
                    if (TargetInAttackRange())
                    {
                        //停止追踪
                        agent.isStopped = true;
                        isFollow = false;

                        //攻击冷却时间结束后进行攻击
                        if (lastAttackTime < 0)
                        {
                            //重置攻击冷却时间
                            lastAttackTime = 1.8f;
                            bashNum = UnityEngine.Random.Range(0f, 1.0f);
                            Attack();
                        }
                    }
                    //不在攻击范围内追踪敌人
                    else
                    {
                        agent.isStopped = false;
                        isFollow = true;
                        agent.destination = attackTarget.transform.position;
                    }
                }
                break;
            case BossStates.DEAD:
                agent.enabled = false;
                player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[5];
                player.GetComponent<AudioSource>().Play();
                herb.SetActive(true);       //草药出现
                Destroy(gameObject, 2f);    //延时销毁
                break;
        }
    }


    void Attack()
    {
        //玩家在Boss的攻击范围内
        if (TargetInAttackRange())
        {
            //boss总血量为500
            if (Health < 100)
            {
                if(bashNum < 0.3)
                {
                    //补血
                    bossAnim.SetTrigger("Cast");
                    player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[4];
                    player.GetComponent<AudioSource>().Play();
                }

                if(0.3 < bashNum && bashNum < 0.6)
                {
                    //法术攻击
                    bossAnim.SetTrigger("Spell");
                    player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[3];
                    player.GetComponent<AudioSource>().Play();
                }
                else
                {
                    //普通攻击
                    bossAnim.SetTrigger("Attack");
                    player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[2];
                    player.GetComponent<AudioSource>().Play();
                }
            }

            else
            {
                if (bashNum < 0.4)
                {
                    //法术攻击
                    bossAnim.SetTrigger("Spell");
                    player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[3];
                    player.GetComponent<AudioSource>().Play();
                }
                else
                {
                    //普通攻击
                    bossAnim.SetTrigger("Attack");
                    player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[2];
                    player.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void TargetHurt()
    {
        playerAnim.SetTrigger("Hurt");
    }


    public void BossHurt()
    {
        bossAnim.SetTrigger("Hurt");

        //重置攻击冷却时间
        lastAttackTime = 1.0f;

    }

    //调整怪物朝向
    void AdjustOrientation()
    {
        //追踪状态下朝向玩家
        if (bossStates == BossStates.CHASE)
        {
            if (player.transform.position.x < boss.transform.position.x)
            {
                boss.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                boss.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        //巡逻或返回守卫点途中朝向目的地
        else
        {
            if (agent.destination.x < boss.transform.position.x)
            {
                boss.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                boss.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    //判断主角是否进入检测范围
    bool FoundPlayer()
    {
        if (Vector2.SqrMagnitude(player.transform.position - boss.transform.position) <= detectionRange)
        {
            attackTarget = player;
            return true;
        }
        attackTarget = null;
        return false;
    }

    //判断主角是否进入攻击范围
    public bool TargetInAttackRange()
    {
        if (Vector2.SqrMagnitude(player.transform.position - trueBoss.transform.position) <= attackRange)
        {
            return true;

        }
        return false;
    }

    //判断自身是否在主角的攻击范围
    public bool MonsterInAttackRange()
    {
        if (Vector2.SqrMagnitude(player.transform.position - boss.transform.position) <= hurtRange)
        {
            if ((attackTarget.transform.position.x - boss.transform.position.x) * attackTarget.transform.localScale.x < 0)
            {
                return true;
            }
        }
        return false;
    }
}
