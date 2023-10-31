using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float duration = 3.0f;
    public GameObject prefab;
    private bool opened = false;
    
    void Start()
    {
        
    }
    void NewWeapon()
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        GameObject newWeapon = Instantiate(prefab, weapon.transform.parent);
        newWeapon.transform.parent = weapon.transform.parent;
        weapon.SetActive(false);
    }

    IEnumerator Heal()
    {
        GameObject playerPos = GameObject.FindGameObjectWithTag("Player");
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, playerPos.transform.position) <= 3.0f);

        Player player = playerPos.GetComponent<Player>();
       
        while (player.currentHealth < player.maxHealth)
        {
            playerPos.GetComponent<Player>().heal(1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Open()
    {
        float elaspedTime = 0.0f;
        float startRotation = transform.eulerAngles.x;
        float endRotation = 75.0f;
        while(elaspedTime <= duration)
        {
            float xRot = Mathf.Lerp(startRotation, endRotation, elaspedTime / duration);
            transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, transform.eulerAngles.z);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Heal());
        if (prefab != null)
        {
            NewWeapon();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length);

        if (enemies.Length == 0 && !opened)
        {
            opened = true;
            StartCoroutine(Open());
        }

    }
}
