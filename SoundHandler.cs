using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioSource footstepSound;
    public AudioSource sprintingSound;
    public AudioSource jumpingSound;
    public AudioSource lightAttackSound;
    public AudioSource heavyAttackSound;

    public bool jumped;

    private void Start()
    {
        jumped = false;
        footstepSound.enabled = false;
        sprintingSound.enabled = false;

        jumpingSound.enabled = true;
        lightAttackSound.enabled = true;
        heavyAttackSound.enabled = true;
    }

    void Update()
    {
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetKey(KeyCode.Space);
        bool isLightAttacking = Input.GetKey(KeyCode.Mouse0);
        bool isHeavyAttacking = Input.GetKey(KeyCode.Mouse1);

        if (isWalking && !isJumping)
        {
            if (isSprinting)
            {
                sprintingSound.enabled = true;
                footstepSound.enabled = false;
            }
            else
            {
                sprintingSound.enabled = false;
                footstepSound.enabled = true;
            }
        }
        else
        {
            footstepSound.enabled = false;
            sprintingSound.enabled = false;
        }

        if (isJumping && !jumped)
        {
            Debug.Log("Jumped");
            jumpingSound.enabled = true;
            jumped = true;
        }

        if (isLightAttacking & !isHeavyAttacking)
        {
            lightAttackSound.Play();
        } else if (isHeavyAttacking & !isLightAttacking)
        {
            heavyAttackSound.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Encountered!");
        if (collision.gameObject.CompareTag("Ground") && jumped)
        {
            Debug.Log("Playing Jump");
            // jumpingSound.Play();
            jumped = false;
        }
    }
}
