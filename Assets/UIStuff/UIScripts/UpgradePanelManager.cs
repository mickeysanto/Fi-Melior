using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public Player player;

    public TMP_Text upgPointsText;
    public TMP_Text healthText;
    public TMP_Text dmgText;
    public TMP_Text cdText;

    public int healthLevel = 0;
    public int dmgLevel = 0;
    public int cdLevel = 0;

    public Button healthButton;
    public Button dmgButton;
    public Button cdButton;

    public InputManager input;
    
    public void ClosePanel(){
        Time.timeScale = 1f;
        input.mouseLook = true;

        panel.SetActive(false);

    }

    public void OpenPanel(){
        Debug.Log("???????????????????????????????");
        input.mouseLook = false;
        Time.timeScale = 0.3f;

        upgPointsText.text = player.upgPoints.ToString();
        panel.SetActive(true);

    }

    void Update(){


        if(Input.GetKeyDown(KeyCode.Escape)){
            if (!panel.activeInHierarchy){
                OpenPanel();            
            }
            else
            {
                ClosePanel();
            }
        }   

         if(player.upgPoints <= 0){

            healthButton.interactable = false;
            dmgButton.interactable = false;
            cdButton.interactable = false;

        }

        else {

            if(healthLevel < 10)
                healthButton.interactable = true;
            if(dmgLevel < 10)
                dmgButton.interactable = true;
            if(cdLevel < 10)
             cdButton.interactable = true;
        }


    }

    public void UpgradeHealth(){

        //decrement upgrade points
        player.upgPoints--;
        upgPointsText.text = player.upgPoints.ToString();

        healthLevel++;
        

        //increment health point on up

        healthText.text = "Health: " + healthLevel.ToString() + "/10";

        //increment max health stat
        player.maxHealth += 10;
        player.healthBar.setMaxHealth(player.maxHealth);

        player.healthBar.setHealth(player.currentHealth);

    }


    

    public void UpgradeDamage(){

        //decrement upgrade points

        player.upgPoints--;
        upgPointsText.text = player.upgPoints.ToString();


        dmgLevel++;
        
        //increment health point on ui

        dmgText.text = "Damage: " + dmgLevel.ToString() + "/10";

        //increment damage stat
        player.strength += 1;

    }

    public void UpgradeCooldown(){

        //decrement upgrade points
        player.upgPoints--;
        upgPointsText.text = player.upgPoints.ToString();

        cdLevel++;
        
        //increment health point on ui
        cdText.text = "Cooldown: " + cdLevel.ToString() + "/10";

        //decrement cooldown stat
        player.heavyCoolDown -= .75f;

    }



}
