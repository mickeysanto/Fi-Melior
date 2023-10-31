using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject other;
    public string sceneName;
    private bool pprox = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pprox = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pprox = false;
        }
    }

    IEnumerator WaitForSceneLoad(string sceneName)
    {
        Player player = FindObjectOfType<Player>();
        DontDestroy.instance.currentHealth = player.currentHealth;
        DontDestroy.instance.curLevel = player.curLevel;
        DontDestroy.instance.curXp = player.curXp;
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);    
    }

    void Update() 
    {
        if (pprox)
        {
            StartCoroutine(other.GetComponent<SceneFader>().FadeOut(sceneName));
            StartCoroutine(WaitForSceneLoad(sceneName));
        }
    }
}
