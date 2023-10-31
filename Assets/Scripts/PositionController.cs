using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionController : MonoBehaviour
{
    [SerializeField] public InputManager inputManager;
    [SerializeField] public Rigidbody character;
    [HideInInspector] public Vector3 motion;

    [Range(1, 5)] public float walkingSpeed = 5.0f;
    [Range(2, 10)] public float runningSpeed = 10.0f;
    private float speed;

    private bool inRoutine = false;
    private bool isJumping = false;


    void Start()
    {
        speed = walkingSpeed;
    }
    IEnumerator Jump()
    {
        character.AddForce(new Vector3(inputManager.horizontal * speed, 5.0f, inputManager.vertical * speed), ForceMode.Impulse);
        yield return new WaitForSeconds(1.208f);
        inRoutine = false;
    }

    void Movement()
    {
        motion = new Vector3(inputManager.horizontal, 0, inputManager.vertical);
        
    }

    void Update()
    {
        if (inputManager.running)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }

        if (inputManager.jumping)
        {
            inRoutine = true;
            StartCoroutine(Jump());
        }

        if (!inRoutine)
        {
            Movement();
        }

    }
}
