using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    //Sound Updates *Jason
    public AudioClip damageSound;
    private AudioSource theAudio;


    public int maxHealth = 100;
    public int currentHealth = 100;
    public float heavyCoolDown = 10.0f;
    public float currCoolDown;
    public HealthBar healthBar;
    public HealthBar coolDownBar;

    public int maxXp = 3;
    public int curXp = 0;
    public XpBar xpBar;

    public int curLevel = 1;
    int[] targetXp = { 2, 4, 8, 11,13, 20,16,19,84};

    public int upgPoints = 0;

    // mult weapon * player.damage ?
    public int strength = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (DontDestroy.instance != null) {
            curLevel = DontDestroy.instance.curLevel;
            currentHealth = DontDestroy.instance.currentHealth;
            curXp = DontDestroy.instance.curXp;
        }
        
        Debug.Log(curLevel);
        Debug.Log(currentHealth);

        //gets Audio Source *Jason
        theAudio = GetComponent<AudioSource>();
        
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(currentHealth);

        currCoolDown = heavyCoolDown;
        coolDownBar.setMaxHealth((int)heavyCoolDown);

        // xpBar.setMaxXp(maxXp);
        xpBar.lvlUp(curLevel, targetXp[curLevel - 1]);
    }

    // Update is called once per frame
    void Update()
    {
       if (currCoolDown < heavyCoolDown)
        {
            currCoolDown += Time.deltaTime;
            coolDownBar.setHealth((int)currCoolDown);
        }
        Debug.Log(curLevel);

    }

    public void TakeDamage(int damage)
    {
        //plays damage Audio *Jason
        theAudio.PlayOneShot(damageSound,0.3F);

        currentHealth -= damage;
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
            FindObjectOfType<GameManager>().EndGame();
    }

     public void heal(int health)
    { 
        currentHealth += health;
        healthBar.setHealth(currentHealth);

    }

    public void gainXp(int xp)
    {
        curXp += xp;
        Debug.Log("curXp " + curXp);
        Debug.Log("curLevel " +curLevel);
        xpBar.setXp(curXp, targetXp[curLevel - 1]);

        if (curXp == targetXp[curLevel - 1])
        {
            StartCoroutine(xpHelper(.5f));

        }
    }

    IEnumerator xpHelper(float delay)
    {

        Debug.Log("Level UP");
        yield return new WaitForSeconds(delay);

        curLevel++;
        xpBar.lvlUp(curLevel, targetXp[curLevel - 1]);

        //upgrade points !!
        upgPoints += 3;

        curXp = 0;

    }

    
}