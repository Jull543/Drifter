using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJudge : MonoBehaviour
{
    private GameObject[] monsterList;
    private GameObject player;
    private GameObject boss;
    public GameObject coin;
    public GameObject lifepotion;
    public GameObject fail;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");
    }

    void Update()
    {
        monsterList = GameObject.FindGameObjectsWithTag("Monster");
    }

    public void PlayerAttack1()
    {
        foreach (var monster in monsterList){

            EnemyController enemyController = monster.GetComponent<EnemyController>();

            if (enemyController.MonsterInAttackRange())
            {
                monster.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                enemyController.lastAttackTime = 0.8f;                 //重置攻击冷却时间

                //普攻下小怪血量减少
                enemyController.GetComponentInChildren<EnemyHealthBar>().hp -= 10f;
                if (enemyController.GetComponentInChildren<EnemyHealthBar>().hp <= 0)
                {
                    enemyController.Health = 0;
                }
            }
        }

        if (boss != null && boss.GetComponent<BossController>().MonsterInAttackRange())
        {
            boss.GetComponent<BossController>().BossHurt();

            //普攻下Boss血量减少（6）
            boss.GetComponent<EnemyHealthBar>().hp -= 30f;
            boss.GetComponent<BossController>().Health = boss.GetComponent<EnemyHealthBar>().hp;
            if (boss.GetComponent<EnemyHealthBar>().hp <= 0)
            {
                boss.GetComponent<BossController>().Health = 0;
            }

        }
    }

    public void PlayerAttack2()
    {
        foreach (var monster in monsterList)
        {
            EnemyController enemyController = monster.GetComponent<EnemyController>();
            if (enemyController.MonsterInAttackRange())
            {
                monster.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                enemyController.lastAttackTime = 0.8f;                 //重置攻击冷却时间

                //重击下怪兽血量减少
                enemyController.GetComponentInChildren<EnemyHealthBar>().hp -= 60f;
                if (enemyController.GetComponentInChildren<EnemyHealthBar>().hp <= 0)
                {
                    enemyController.Health = 0;
                }
            }
        }

        if (boss != null && boss.GetComponent<BossController>().MonsterInAttackRange())
        {
            boss.GetComponent<BossController>().BossHurt();

            //重击下Boss血量减少(14)
            boss.GetComponent<EnemyHealthBar>().hp -= 40f;
            boss.GetComponent<BossController>().Health = boss.GetComponent<EnemyHealthBar>().hp;
            if (boss.GetComponent<EnemyHealthBar>().hp <= 0)
            {
                boss.GetComponent<BossController>().Health = 0;
            }

        }
    }
    public void MonsterAttack1()
    {
        foreach (var monster in monsterList)
        {
            EnemyController enemyController = monster.GetComponent<EnemyController>();
            if (enemyController.TargetInAttackRange())
            {
                player.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                //小怪普攻下玩家血量减少
                player.GetComponent<PlayerScript>().currentHp -= 6f;
                if (player.GetComponent<PlayerScript>().currentHp <= 0)
                {
                    player.GetComponent<HeroKnight>().Health = 0;
                    Time.timeScale = 0;
                    fail.SetActive(true);
                }
            }
        }
    }
    public void MonsterAttack2()
    {
        foreach (var monster in monsterList)
        {
            EnemyController enemyController = monster.GetComponent<EnemyController>();
            if (enemyController.TargetInAttackRange())
            {
                player.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Hurt");

                //小怪重击下玩家血量减少
                player.GetComponent<PlayerScript>().currentHp -= 8f;
                if (player.GetComponent<PlayerScript>().currentHp <= 0)
                {
                    player.GetComponent<HeroKnight>().Health = 0;
                    Time.timeScale = 0;
                    fail.SetActive(true);
                }
            }
        }
    }
    public void BossAttack1()
    {
        if (boss.GetComponent<BossController>().TargetInAttackRange())
        {
            boss.GetComponent<BossController>().TargetHurt();

            //Boss普攻下玩家血量减少()
            player.GetComponent<PlayerScript>().currentHp -= 6f;
            if (player.GetComponent<PlayerScript>().currentHp <= 0)
            {
                fail.SetActive(true);
                Time.timeScale = 0;
                player.GetComponent<HeroKnight>().Health = 0;
            }
        }
    }
    public void BossAttack2()
    {
        if (boss.GetComponent<BossController>().TargetInAttackRange())
        {
            boss.GetComponent<BossController>().TargetHurt();

            //Boss魔法攻击下玩家血量减少
            player.GetComponent<PlayerScript>().currentHp -= 10f;
            if (player.GetComponent<PlayerScript>().currentHp <= 0)
            {
                fail.SetActive(true);
                Time.timeScale = 0;
                player.GetComponent<HeroKnight>().Health = 0;
            }
        }
    }

    public void BossAttack3()
    {
        if (boss.GetComponent<BossController>().TargetInAttackRange())
        {
            //Boss补血
            boss.GetComponent<EnemyHealthBar>().hp += 50f;
            boss.GetComponent<BossController>().Health = boss.GetComponent<EnemyHealthBar>().hp;
        }
    }

    public void CoinCreate()
    {
        foreach (var monster in monsterList)
        {
            EnemyController enemyController = monster.GetComponent<EnemyController>();

            if (enemyController.MonsterInAttackRange())
            {
                //怪物死后掉落金币
                Instantiate(coin, new Vector3(monster.transform.position.x+0.2f, monster.transform.position.y,0), Quaternion.identity);
                Instantiate(lifepotion, new Vector3(monster.transform.position.x - 0.2f, monster.transform.position.y, 0), Quaternion.identity);
                Instantiate(coin, new Vector3(monster.transform.position.x, monster.transform.position.y-0.2f,0), Quaternion.identity);
            }
        }
    }
}
