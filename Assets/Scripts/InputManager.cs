using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*InputManager: Written by Jessica Nefflen
 * Input Manager records the user input and sotres it in variables for the
 * controller to use. Code alos allows for different configurations to be set up: 
 * for example: a controller input set up could be created or an alternative keyboard
 * input configurations could be created. the code allows for these configurations to be switched 
 * for ease of testing and comparing. By default it will set for the basic keyboard configuration 
 * we have decided on.
 */
public class InputManager : MonoBehaviour
{

    //sound variables *Jason
    //setting up the enum for different configuations, and the varibale to controll it 
    // in inspector.
    internal enum InputType{Keyboard, Controller}
    [SerializeField] private InputType input;

    //setting input varibale to be accessed from controller.
    public float vertical, horizontal;
    public bool running, jumping, lightAttack,crouch,heavyAttack,mouseLook;
    void Start()
    {
        //assigning inital values to variables.
        running = false;
        jumping = false;
        lightAttack = false;
        heavyAttack = false;
        crouch = false;
        mouseLook = true;
    }

    void KeyBoardInput()
    {
        jumping = false;
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        } 

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            crouch = false;
            Debug.Log("jumping");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumping = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lightAttack = true;
            crouch = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            lightAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouch = !crouch;
        }
        if (Input.GetMouseButtonDown(1))
        {
            heavyAttack = true;
            crouch = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            heavyAttack = false;
        }
       //if (Input.GetKeyDown(KeyCode.Escape))
       // {
       //    if mouseLook = false {
        //    !mouseLook;
        //}

    }
    
    void Update()
    {
        if(input == InputType.Controller)
        {

        }else
        {
            KeyBoardInput();
        }
    }
}
