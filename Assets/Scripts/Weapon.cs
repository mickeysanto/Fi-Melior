using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;
    public int range = 5;
    public int heavyMultiplier = 3;
    public bool player;
    //public float coolDownDuration = 0.0f;
    //public float coolDown;
    
    public Player myPlayer;

    private void Start()
    {
        //coolDown = coolDownDuration;
    }

    void OnTriggerEnter(Collider other)
    {
        //if (coolDown <= 0.0f)
        //{
            //coolDown = coolDownDuration;

        if (other.gameObject.CompareTag("Enemy") && player)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            EnemyMachineV3 eMachine = other.gameObject.GetComponent<EnemyMachineV3>();

            if (!eMachine.percievesPlayer)
            {
                enemy.TakeDamage(damage * 2 + myPlayer.strength);
            }
            else
            {
                enemy.TakeDamage(damage + myPlayer.strength);
            }
        }
        else if (other.gameObject.CompareTag("Player") && !player)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag("Floor") && player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for( int i = 0; i < enemies.Length; i++)
            {
               
                if (Vector3.Distance(transform.position, enemies[i].transform.position) < range)
                {
                    enemies[i].GetComponent<Enemy>().TakeDamage(damage*heavyMultiplier);
                }
            }
        }
        else if (other.gameObject.CompareTag("Floor") && !player)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
               
                if (Vector3.Distance(transform.position, player.transform.position) < range)
                {
                    player.GetComponent<Player>().TakeDamage(damage*heavyMultiplier);
                }
        }
        
        
    }

    private void Update()
    {
       // if (coolDown > 0.0f)
        //{
       //     coolDown -= Time.deltaTime;
       // }
    }
}
