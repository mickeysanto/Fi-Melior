using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public int maxHealth = 5;
    public int currentHealth;
    public HealthBar healthBar;

    public int xpToGive = 1;

    public GameObject player;

    private bool inRoutine = false;
    private float damageBlock = 0.5f;
    private float currDamageBlock = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentHealth <= 0 && !inRoutine)
        {
            inRoutine = true;
            StartCoroutine(Destroy(0.5f));
        }
        if (currDamageBlock > 0)
        {
            currDamageBlock -= Time.deltaTime;
        }
    }

    // returns true if enemy dies, false if not
    public void TakeDamage(int damage)
    {
        if (currDamageBlock <= 0)
        {
            currentHealth -= damage;
            healthBar.setHealth(currentHealth);
            currDamageBlock = damageBlock;
        }
    }

    IEnumerator Destroy(float delay)
    {
        Debug.Log("destroy");
        player.GetComponent<Player>().gainXp(xpToGive);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
