using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMachineV2 : MonoBehaviour
{
    /*Written by Mickey (Mitchell) Santomartino*/

    public float aggroRange = 5f; //range in which enemy will lock onto player to attack
    public float attackRange = 2f; //range in which enemy will attack the player
    public float percieveRange = 3f;
    public float movementSpeed = 1f; //movement speed of the enemy
    public float attackCooldown = 3f; //attack cooldown in seconds
    public bool attackPause; //enemey will actually stop after attack if true

    public bool isAttacking; //is true if enemy is currently in an attack
    public bool isColliding; //is true if enemy is colliding with a wall

    protected Transform player; //the player's transform
    protected float distance; //distance from player
    protected Rigidbody rb;
    protected Animator animator;
    /*protected Ray ray;
    protected RaycastHit rayHit; */
    protected Vector3 target;
    protected UnityEngine.AI.NavMeshAgent agent;
    

    public bool percievesPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        isAttacking = false;
        //ray = new Ray(transform.position, transform.forward);

        player = GameObject.FindWithTag("Player").transform;
        target = transform.position;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = target;

        animator = GetComponent<Animator>();

        StartCoroutine(Idle());
        StartCoroutine(AttackCooldown());
        
        //StartCoroutine(LookTo());
        //StartCoroutine(Collision());
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        /* ray.origin = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
        ray.direction = transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 10); */


        if (distance <= aggroRange && !player.GetComponent<InputManager>().crouch) 
        {
            if (!player.GetComponent<Controller>().inRoutine)
            percievesPlayer = true;
        }
        if (distance <= percieveRange && !IsBehind())
        {
            percievesPlayer = true;
        }
        if (distance > aggroRange)
        {
            percievesPlayer = false;
            agent.destination = transform.position;
        }

        /* if (Physics.Raycast(ray, out rayHit, .5f))
        {
            if (rayHit.collider.tag != "Player")
            {
                isColliding = true;
            }
        }
        else
        {
            isColliding = false;
        } */
    }

    //adds a cooldown for enemy attacks
    public IEnumerator AttackCooldown()
    {
        while (true)
        {
            yield return new WaitUntil(() => isAttacking);
            yield return new WaitForSeconds(attackCooldown);

            isAttacking = false;
        }
    }

    //moves enemy out of the way if it collides with an obstical
    /*     public IEnumerator Collision()
    {
        while (true)
        {
            yield return new WaitUntil(() => isColliding);

            while (isColliding)
            {
                rb.AddForce(transform.right * 4);
                yield return null;
            }

            isColliding = false;
        }
    } */

    public virtual IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && isColliding == false));



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

    //moves towards player
    public IEnumerator Move()
    {
        while (distance <= aggroRange && distance > attackRange)
        {
            
            //transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
            if (percievesPlayer)
            {
                Vector3 newPosition = player.position;
                if (Vector3.Distance(transform.position, player.position) > attackRange + 1) {
                    Vector2 point = Random.insideUnitCircle;
                    newPosition += new Vector3(point.x, 0, point.y);
                }

                if(Random.Range(0,1000) < 5) {
                    agent.speed = Random.Range(3.0f,4.0f);
                }
                
                agent.destination = newPosition;
                animator.SetBool("Moving", true);

                //Vector3 motion = (player.position - transform.position).normalized;
                //rb.MovePosition(transform.position + (motion * Time.deltaTime * movementSpeed));
            }
            yield return null;
        }
        agent.destination = transform.position;
        animator.SetBool("Moving", false);
    }

    public IEnumerator Attack()
    {
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(1.208f);
        animator.SetBool("Attacking", false);
        yield return null;
    }

    //makes enemy look at player 
    public IEnumerator LookTo()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && !isColliding));
            if (percievesPlayer)
            {
               transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }
        }
    } 

    private bool IsBehind()
    {
        Vector3 heading = player.position - transform.position;
        float inFront = Vector3.Dot(heading, transform.forward);

        if (inFront > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
