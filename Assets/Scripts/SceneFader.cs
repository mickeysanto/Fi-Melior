using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 2f;
    
    public void onClick(){
        StartCoroutine(FadeOut("Level1"));

    }
    public void QuitGame() 
    {
        Application.Quit();
    }
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1f;

        while (fadeCanvasGroup.alpha > 0f)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    public IEnumerator FadeOut(string sceneName)
    {
        fadeCanvasGroup.alpha = 0f;

        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);

    }
}