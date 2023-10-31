using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedV3 : EnemyMachineV3
{
    /*Written by Mickey (Mitchell) Santomartino*/

    public float retreatRange; //range in which enemy will run away from player
    public float retreatSpeed; //speed at which enemy runs away
    private bool isRetreating = false;

    [SerializeField]
    public GameObject projectile;

    public override IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange));

            StartCoroutine(Move());

            if (distance <= attackRange && isAttacking != true && !isRetreating)
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
                isRetreating = true;
                StartCoroutine(Retreat());
            }
        }
    }

    //moves away from player if they are within retreatRange
    private IEnumerator Retreat()
    {
        while (distance <= retreatRange)
        {
            Vector3 retreatPos = transform.position + (transform.position - player.transform.position);

            agent.destination = retreatPos;

            animator.SetBool("Moving", true);

            //transform.position = Vector3.MoveTowards(transform.position, player.position, -1 * retreatSpeed * Time.deltaTime);

            yield return null;
        }
        isRetreating = false;
        animator.SetBool("Moving", false);
    }

    public void fire()
    {
        float projectileSpeed = 15f;
        Vector3 startPos = new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x,player.position.y,player.position.z);
        Vector3 directionVector = playerPos - startPos;
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        //creates the projectile
        GameObject firedProjectile = Instantiate(projectile, startPos, targetRotation);
        Rigidbody projectileRigidbody = firedProjectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;
        StartCoroutine(DestroyProjectile(firedProjectile));
    }

    public IEnumerator DestroyProjectile(GameObject projectile) 
    {
        yield return new WaitForSeconds(2);
        Destroy(projectile);
    }

    public override IEnumerator Attack()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(1.208f);
        animator.SetBool("Attacking", false);
        fire();
        yield return null;
    }
}
