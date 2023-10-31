using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public void QuitGame() 
    {
        Application.Quit();
    }

    public void GoToTitle(){

        SceneManager.LoadScene("StartScreen");
        Time.timeScale = 1f;

    }
}
