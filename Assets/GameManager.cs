
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameEnded = false;
    public float restartDelay = 1f;
    public GameObject deathScreen;

    public InputManager input;

    
    public void EndGame(){

        if (!gameEnded){
            gameEnded = true;
            Debug.Log("GAME OVER");
            input.mouseLook = false;
            deathScreen.SetActive(true);
            Time.timeScale = 0f;

        }
    }

    public void Restart(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;


    }

    public void GoToTitle(){

        SceneManager.LoadScene("StartScreen");
        Time.timeScale = 1f;

    }

     public void LoadGame() 
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame() 
    {
        Application.Quit();
        Debug.Log("Quit!");

    }

}
