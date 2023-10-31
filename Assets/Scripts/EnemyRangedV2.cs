using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedV2 : EnemyMachineV2
{
    /*Written by Mickey (Mitchell) Santomartino*/

    public float retreatRange; //range in which enemy will run away from player
    public float retreatSpeed; //speed at which enemy runs away
    private int wave = 0;
    //public GameObject[] waves;
    protected Enemy enemy;
    public GameObject prefab;
    
    void Awake() {
        enemy = gameObject.GetComponent<Enemy>();
        
        StartCoroutine(CheckHealth());
    }
    public override IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && isColliding == false));

            StartCoroutine(Move());
            StartCoroutine(LookTo());

            if (distance <= attackRange && isAttacking != true)
            {
                isAttacking = true;
                StartCoroutine(Attack());

                //enemy waits after attacking to make its next move if attackPause is enabled
                if (attackPause)
                {
                    yield return new WaitForSeconds(attackCooldown);
                }
            }
            else if (distance <= retreatRange)
            {
                StartCoroutine(Retreat());
            }
        }
    }

    //moves away from player if they are within retreatRange
    private IEnumerator Retreat()
    {
        while (distance <= retreatRange)
        {
            //Vector3 retreatPos = transform.position + (transform.position - player.transform.position);

            //agent.destination = retreatPos;
            
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPosition = transform.position + dirToPlayer;

            agent.destination = newPosition;

            //transform.position = Vector3.MoveTowards(transform.position, player.position, -1 * retreatSpeed * Time.deltaTime);

            yield return new WaitForSeconds(0.5f);
        }
    }
    void Summon(int waveNum) {
        Debug.Log("summoning");
         for (int i = 0; i <= 3; i++) {
            Vector3  enemyPos= new Vector3(Random.Range(-8.0f, 23.0f), 0f, Random.Range(37.0f,60.0f));
            GameObject newEnemy = Instantiate(prefab,enemyPos, transform.rotation) as GameObject;
         }
    }

    IEnumerator CheckHealth() {
        Debug.Log("checkingHealth");

        while(true) {
            if (enemy.currentHealth <= enemy.maxHealth*2/3 && wave == 0) {
                Summon(0);
                wave = 1;
            }
            else if (enemy.currentHealth <= enemy.maxHealth / 2 && wave == 1) {
                Summon(1);
                wave = 2;
            }
            else if (enemy.currentHealth <= enemy.maxHealth / 3 && wave == 2) {
                Summon(2);
                wave = 3;
            }
            yield return null;
        }

        
    }
}
