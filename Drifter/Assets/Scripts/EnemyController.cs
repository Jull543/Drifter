using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }  //状态机(4种状态)：守卫/巡逻/追击/死亡
[RequireComponent(typeof(NavMeshAgent))]                //AI组件(没有则自动添加)

public class EnemyController : MonoBehaviour
{
    [HideInInspector] public EnemyStates enemyStates;

    private GameObject monster;
    private GameObject attackTarget;
    private GameObject player;

    [Header("范围设置")]
    [Tooltip("检测范围")] public float detectionRange;
    [Tooltip("小怪攻击范围")] public float attackRange;
    [Tooltip("主角攻击范围")] public float hurtRange;

    [Header("小怪属性设置")]
    [Tooltip("是否为守卫怪")] public bool isGuard = true;

    [Header("巡逻区域设置")]
    [Tooltip("左巡逻范围")] public float patrolLeft = 2;
    [Tooltip("右巡逻范围")] public float patrolRight = 2;

    private NavMeshAgent agent;
    private float speed = 3;            //巡逻和追击速度不同

    //时间控制参数
    private float lookAtTime = 2.0f;
    private float remainLookAtTime;
    [HideInInspector] public float lastAttackTime;

    //血量参数
    [HideInInspector] public int Health;

    //位置参数
    private Vector3 guardPos;           //初始守卫位置
    private Vector3 leftPoint;       //左巡逻点
    private Vector3 rightPoint;      //右巡逻点

    //动画动制参数
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;

    private float bashNum;
    private Animator monsterAnim;
    private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
        }
    }

    void Awake()
    {
        monster = this.gameObject.transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        playerAnim = GameObject.FindWithTag("Warrior").GetComponent<Animator>();

        Health = 100;
        speed = agent.speed;
        guardPos = transform.position;
        leftPoint = new Vector3(guardPos.x-patrolLeft, guardPos.y, 0);
        rightPoint = new Vector3(guardPos.x + patrolRight, guardPos.y, 0);
        agent.destination = leftPoint;
        remainLookAtTime = lookAtTime;
    }

    // Update is called once per frame
    void Update()
    {
        monsterAnim = monster.GetComponent<Animator>();

        if (Health == 0)
        {
            isDead = true;
        }
        SwitchStates();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        monster.transform.position = new Vector3 (transform.position.x,monster.transform.position.y,0);
    }

    void SwitchAnimation()
    {
        monsterAnim.SetBool("Walk", isWalk);
        monsterAnim.SetBool("Chase", isChase);
        monsterAnim.SetBool("Follow", isFollow);
        monsterAnim.SetBool("Dead", isDead);
    }

    //切换状态
    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                AdjustOrientation();
                isChase = false;

                //判断是否在原位置（追踪拉脱后回到原位置）
                if (transform.position!= guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector2.SqrMagnitude(guardPos - transform.position) <= 1.5)
                    {
                        //回到原位置后动画切换
                        isWalk = false;

                        //回到原位置后恢复朝向
                        monster.transform.localScale = new Vector3(5, 5, 1);
                    }
                }
                break;
            case EnemyStates.PATROL:
                AdjustOrientation();
                isChase = false;
                agent.speed = speed * 0.3f;

                //判断当前位置距巡逻点的距离
                if (transform.position.x < leftPoint.x)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = leftPoint;
                }
                else if(transform.position.x > rightPoint.x)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = rightPoint;
                }
                else
                {
                    isWalk = true;
                    agent.isStopped = false;
                }

                if (Vector2.SqrMagnitude(agent.destination- transform.position) <= 2)
                {
                    isWalk = false;

                    //到达目的地后观望一会儿
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    
                    //观望一会儿后向相反方向巡逻
                    else
                    {
                        isWalk = true;
                        remainLookAtTime = 2.0f;

                        if (Vector2.SqrMagnitude(agent.destination-leftPoint) <= 3)
                        {
                            agent.destination = rightPoint;
                        }
                        else if (Vector2.SqrMagnitude(agent.destination - rightPoint) <= 3)
                        {
                            agent.destination = leftPoint;
                        }
                    }
                }
                break;
            case EnemyStates.CHASE:
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
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                        agent.destination = leftPoint;
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
                            lastAttackTime = 1.5f;
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
            case EnemyStates.DEAD:
                agent.enabled = false;
                Destroy(gameObject, 2f);    //延时销毁
                break;
        }
    }


    void Attack()
    {
        //玩家在怪兽的攻击范围内
        if (TargetInAttackRange())
        {
            if (bashNum < 0.3)
            {
                //重击
                monsterAnim.SetTrigger("Bash");
            }
            else
            {
                //普通攻击
                monsterAnim.SetTrigger("Attack");
            }
        }
    }

    //调整怪物朝向
    void AdjustOrientation()
    {
        //追踪状态下朝向玩家
        if (enemyStates == EnemyStates.CHASE)
        {
            if (player.transform .position.x < monster.transform.position.x)
            {
                monster.transform.localScale = new Vector3(-5, 5, 1);
                monster.transform.GetChild(0).gameObject.transform.localScale = new Vector3(-0.16f, 0.16f, 0);
            }
            else
            {
                monster.transform.localScale = new Vector3(5, 5, 1);
                monster.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.16f, 0.16f, 0);
            }
        }
        //巡逻或返回守卫点途中朝向目的地
        else
        {
            if (agent.destination.x < monster.transform.position.x)
            {
                monster.transform.localScale = new Vector3(-5, 5, 1);
                monster.transform.GetChild(0).gameObject.transform.localScale = new Vector3(-0.16f, 0.16f, 0);
            }
            else
            {
                monster.transform.localScale = new Vector3(5, 5, 1);
                monster.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.16f, 0.16f, 0);
            }
        }
    }

    //判断主角是否进入检测范围
    bool FoundPlayer()
    {
        if (Vector2.SqrMagnitude(player.transform.position - monster.transform.position) <= detectionRange)
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
        if (Vector2.SqrMagnitude(player.transform.position - monster.transform.position) <= attackRange)
        {
            return true;
        }
        return false;
    }

    //判断自身是否在主角的攻击范围
    public bool MonsterInAttackRange()
    {

        if (Vector2.SqrMagnitude(player.transform.position - monster.transform.position) <= hurtRange)
        {
            if ((attackTarget.transform.position.x - monster.transform.position.x) * attackTarget.transform.localScale.x < 0)
            {
                return true;
            }
        }
        return false;
    }
}
