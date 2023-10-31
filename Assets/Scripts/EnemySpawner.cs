using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*Enemy Spawner Code written by: Jason Zhang*/

    //initializes fields with self explanatory names
    [SerializeField]
    private GameObject preFab;
    [SerializeField]
    private float spawnInterval = 3.0f;
    [SerializeField]
    private float maxSpawns = 2;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        StartCoroutine(spawnEnemy(spawnInterval, preFab));        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Coroutine to spawn enemies, spawns at the location of the empty game object
    private IEnumerator spawnEnemy(float interval, GameObject enemy){
        yield return new WaitForSeconds(interval);
        if(maxSpawns > i){
            GameObject newEnemy = Instantiate(enemy, transform.position,Quaternion.identity);
            i++;
            StartCoroutine(spawnEnemy(interval, enemy));
        }
    }
}
