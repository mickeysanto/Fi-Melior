using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller: Written by Jessica Nefflen
 * Sound: Written by Jason Zhang
 * Controller uses the InputManager class and converts the input collected by input manager
 * into actions. These actions control movement and the animator for the character. Each Action 
 * is a coroutine that either updates position or waits until the action's animation has completed. 
 * This class works in tandeem with the Animator Controller to ensure two actions can not occure at the 
 * same time. the Controller class uses the bool inRoutine to make sure no two coroutines can play at 
 * the same time. the animator controller has transition condition two block the animation returning to a 
 * different state until animation is done.
 */

//requiring components needed for class
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Controller : MonoBehaviour
{
    //setting up public Variables, public so they can be accessed outside of class
    //hidden since access not needed in inspector.
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public Rigidbody character;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 motion;
    

    //setting up speed public variables
    [Range(5,10)] public float walkingSpeed = 10.0f;
    [Range(10,15)] public float runningSpeed = 20.0f;
    [Range(3, 8)] public float crouchingSpeed = 5.0f;
    //public float turnAmount = 180.0f;
    public GameObject weaponParent;
    public float turnMultiplier = 100;
    public GameObject focus;
    public ParticleSystem partSystem;
    //public GameObject mainCamera;
    //Setting up sound variables *Jason
    public AudioClip leftClickAA;
    public AudioClip rightClickAA;
    private AudioSource theAudio;
    //setting up private variables
    private float speed;
    public bool inRoutine = false;
    private bool cursorLocked = false;
    private Vector3 relativeVector;
    private float turnDirection;
    private GameObject weapon;

    void Start()
    {
        //assigning varriables
        inputManager = GetComponent<InputManager>();
        character = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        foreach(Transform child in weaponParent.transform)
        {
            if (child.CompareTag("Weapon"))
            {
                weapon = child.gameObject;
            }
        }
        theAudio = GetComponent<AudioSource>();
        weapon.GetComponent<CapsuleCollider>().enabled = false;
        speed = walkingSpeed;
    }

    //Jump Coroutine: adds Impulse force to ridigid body to create jump movement, times out 
    //animation duration.
     IEnumerator Jump()
    {
        float elaspedTime = 0.0f;
        yield return new WaitForSeconds(0.108f);
        character.AddForce(new Vector3(0.0f, 4.5f, 0.0f), ForceMode.Impulse);
        motion = transform.right * (inputManager.horizontal) + transform.forward * (inputManager.vertical);

        while (elaspedTime <= 1.1f)
        {
            character.MovePosition(transform.position + (motion * Time.deltaTime * speed));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        inRoutine = false;
        animator.SetBool("jumping", false);

    }

    //Light Attack Coroutine: times out light attack animation
    IEnumerator LightAttack()
    {
        CapsuleCollider weaponCollider = weapon.GetComponent<CapsuleCollider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = true;
        
        //playing Light Attack Audio *Jason
        theAudio.PlayOneShot(leftClickAA,0.7F);

        yield return new WaitForSeconds(0.8f);
        
        inRoutine = false;
        animator.SetBool("lightAttack", false);
        weaponCollider.enabled = false;

    }

    IEnumerator HeavyAttack()
    {
        CapsuleCollider weaponCollider = weapon.GetComponent<CapsuleCollider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = true;
        
        //playing Heavy Attack Audio *Jason
        theAudio.PlayOneShot(rightClickAA,0.7F);

        yield return new WaitForSeconds(0.5f);
        if (partSystem != null)
        {
            partSystem.Play();
        }
        yield return new WaitForSeconds(0.3f);

        inRoutine = false;
        animator.SetBool("HeavyAttack", false);
        gameObject.GetComponent<Player>().currCoolDown = 0;
        weaponCollider.enabled = false;

    }

    //movement coroutine: sets animator vertical and horizontal floats
    //for the blendtree, and applies motion. 
    void Movement()
    {

        //if running double float inputted into animator controller and 
        //sett speed to running speed.
        int movMultiplier = 1;
        animator.SetBool("IsCrouching", inputManager.crouch);

        if (inputManager.crouch)
        {
            speed = crouchingSpeed;
        }
        else if (inputManager.running)
        {
            movMultiplier = 2;
            speed = runningSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }

        animator.SetFloat("Vertical", inputManager.vertical * movMultiplier);
        animator.SetFloat("Horizontal", inputManager.horizontal * movMultiplier);

        //apply motion
        motion = Vector3.Normalize(transform.right * (inputManager.horizontal) + transform.forward * (inputManager.vertical));


       if (motion != Vector3.zero)
        {
            character.MovePosition(transform.position + (motion * Time.deltaTime * speed));
            //character.MoveRotation(Quaternion.Euler(new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z)));
            //turnDirection = focus.transform.parent.eulerAngles.y / 180;
            transform.Rotate(transform.up * turnDirection * turnMultiplier * Time.deltaTime);
            focus.transform.parent.Rotate(transform.up * -turnDirection * turnMultiplier * Time.deltaTime);   
        }
        else
        {
            theAudio.Play();
        }
    }

    void MouseLook()
    {
        //if(Input.GetKeyDown(KeyCode.Tab)) cursorLocked = cursorLocked ? false : true;
        cursorLocked = inputManager.mouseLook;
        
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            relativeVector = transform.InverseTransformPoint(focus.transform.position);
            relativeVector /= relativeVector.magnitude;
            turnDirection = (relativeVector.x/relativeVector.magnitude);

            float newXRotation = focus.transform.eulerAngles.x - Input.GetAxis("Mouse Y");

            if (newXRotation <=80.0f || newXRotation >= 340.0f)
            {
                focus.transform.eulerAngles = new Vector3(newXRotation - Input.GetAxis("Mouse Y"), focus.transform.eulerAngles.y, 0);
            }

            focus.transform.parent.Rotate(transform.up * Input.GetAxis("Mouse X") * Time.deltaTime * 100);
        } 

        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()
    {
        //if not in coroutine, look for next corotine to start. 
        MouseLook();
        if (!inRoutine) {

            //if jumping start jump coroutine and set jump start condition in animator controller
            if (inputManager.jumping)
            {
                animator.SetBool("jumping", true);
                inRoutine = true;
                StartCoroutine(Jump());
            }

            //if light attack statrt light attack coroutine and set lattack start condition in
            //animator controller
            else if (inputManager.lightAttack)
            {
                animator.SetBool("lightAttack", true);
                inRoutine = true;
                StartCoroutine(LightAttack());
            }
            else if (inputManager.heavyAttack && gameObject.GetComponent<Player>().currCoolDown >= 15.0)
            {
                animator.SetBool("HeavyAttack", true);
                inRoutine = true;
                StartCoroutine(HeavyAttack());
            }

            //if no other coroutines to start, update movement.
            else
            {
                Movement();
            }
        }
    }
}
