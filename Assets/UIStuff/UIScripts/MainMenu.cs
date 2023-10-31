using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject panel;
    PauseManager pauseManager;

    private void Awake() {
        pauseManager = GetComponent<PauseManager>();
    
    } 

    // Update is called once per frame
    void Update()
    {

        if( Input.GetKeyDown(KeyCode.Escape)){

            if (!panel.activeInHierarchy)
                OpenMenu();            
            else
            {
                CloseMenu();
            }


        }   

    }

    public void CloseMenu(){

        Time.timeScale = 1f;
        panel.SetActive(false);

    }

    public void OpenMenu(){

        Time.timeScale = 0f;
        panel.SetActive(true);

    }
}
