using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyMachine
{
    /*Written by Mickey (Mitchell) Santomartino*/

    public float retreatRange; //range in which enemy will run away from player
    public float retreatSpeed; //speed at which enemy runs away

    public override IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && isColliding == false));

            StartCoroutine(Move());

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
            transform.position = Vector3.MoveTowards(transform.position, player.position, -1 * retreatSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
