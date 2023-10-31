using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyRangedV3
{
    /*Written by Mickey (Mitchell) Santomartino*/

    private int wave = 0;
    public GameObject minion;
    protected Enemy enemy;

    void Awake() {
        enemy = gameObject.GetComponent<Enemy>();
        StartCoroutine(CheckHealth());
    }

    public override IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange));

            StartCoroutine(Move());

            //if player is in attack range and their attack is not currently on cooldown, attack
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
        }
    }

    public override IEnumerator Attack()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(1.208f);
        animator.SetBool("Attacking", false);

        if(distance >= 4f) 
        {
            fire();
        }

        yield return null;
    }

    void Summon(int waveNum) {
        Debug.Log("summoning");
         for (int i = 0; i < 3; i++) {
            Vector3  enemyPos= new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), 0f, transform.position.z + Random.Range(-3.0f,3.0f));
            GameObject newEnemy = Instantiate(minion, enemyPos, Quaternion.identity);
         }
    }

    IEnumerator CheckHealth() {
        Debug.Log("checkingHealth");

        while(true) {
            if (enemy.currentHealth <= (enemy.maxHealth*2)/3 && wave == 0) {
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
