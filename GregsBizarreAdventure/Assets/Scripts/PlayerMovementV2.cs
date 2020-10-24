using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;

    private float horizontalMove = 0f, runSpeed = 40;
    private bool jump = false, crounch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
    }


    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crounch, jump);
        jump = false;
    }
}
