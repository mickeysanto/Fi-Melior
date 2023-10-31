using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject[] popUps;
    private int i = 0;
    public GameObject aoeAttack;

    // Start is called before the first frame update
    void Start()
    {

        popUps[i].SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {

/*
        for (int j = 0; j < popUps.Length; j++)
        {
            if(j == i)
                popUps[i].SetActive(true);
            else
                popUps[i].SetActive(false);
        }

*/
        if(i == 0){

            if(Input.GetKeyDown(KeyCode.Space)){

                popUps[i].SetActive(false);

                i++;
                popUps[i].SetActive(true);
            }

        }
        else if(i == 1){

            if(Input.GetMouseButtonDown(0)){
                 popUps[i].SetActive(false);

                i++;
                popUps[i].SetActive(true);
            }

        }
        else if(i == 2){

            if(Input.GetKeyDown(KeyCode.LeftShift)){
                 popUps[i].SetActive(false);

                i++;
                popUps[i].SetActive(true);
            }

        }


        else if(i == 3){

            aoeAttack.SetActive(true);
            if(Input.GetMouseButtonDown(1)){
                
            }

        }
        
}

}