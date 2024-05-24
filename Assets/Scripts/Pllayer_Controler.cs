using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pllayer_Controler : MonoBehaviour
{

    private Rigidbody2D rb;
    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool CanJump;
    private float yvelocity;
    private bool isFalling;

    public int jumps = 3;

    private int AmountOfJumpsLeft;

    public float jumpForce = 16.0f;
    
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AmountOfJumpsLeft = jumps;
    }

    // Update is called once per frame
    void Update()
    {   
        int state = animator.GetInteger("State");
        CheckInput();
        ChecKMovementDirection();
        CheckIfCanJump();
        CheckIsFalling();

        updateJumpState();
        
        

        switch (state){
            case 0: 
                if(Input.GetKey("a") && Input.GetKey("space")|| Input.GetKey("d") && Input.GetKey("space")){
                    animator.SetInteger("State",2);
                }else if(Input.GetKey("space")){
                    animator.SetInteger("State",2);
                    
                }else if(Input.GetKey("a")|| Input.GetKey("d")){
                    animator.SetInteger("State",1);
                }else if(Input.GetKey("f")){
                    animator.SetInteger("State",3);
                }
                break;
            case 1:
                if(Input.GetKey("space")){
                    animator.SetInteger("State",2);
                }else if(!Input.GetKey("a") && !Input.GetKey("d")){
                    animator.SetInteger("State",0);
                }else if(Input.GetKey("f")){
                    animator.SetInteger("State",3);
                }
                break;
            case 2:
                if(isGrounded && Input.GetKey("a") || isGrounded && Input.GetKey("b")){
                    animator.SetInteger("State",1);
                }else if(isGrounded){
                    animator.SetInteger("State",0);
                }break;
            case 3:
                if(!Input.GetKey("f")){
                    animator.SetInteger("State",0);
                }else if(Input.GetKey("a")|| Input.GetKey("d")){
                    animator.SetInteger("State",1);
                }    
                break;
        }
    }

    private void FixedUpdate(){
        ApplyMovement();
        CheckSurroundings();
    }


    private void CheckSurroundings(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump(){
        if(isGrounded && rb.velocity.y <= 0){
            AmountOfJumpsLeft = jumps;//si el personaje toca el piso, el numero de veces que puede brincar se resetea
        }

        if(AmountOfJumpsLeft <= 0){
            CanJump = false;
        }else{
            CanJump = true;
        }
    }
    private void ChecKMovementDirection(){
        //si esta viendo a la derecha y empieza a moverse a la izquierda, girar
        if(isFacingRight && movementInputDirection < 0){
            Flip();
        }else if(!isFacingRight && movementInputDirection > 0){//si esta viendo a la izquierda y camina a la derecha, girar
            Flip();
        }
    }



    private void CheckInput(){
        movementInputDirection = Input.GetAxisRaw("Horizontal_Player1");
        if(Input.GetButtonDown("Jump_Player1")){
                Jump();
        }
    }

    private void Jump(){
        if(CanJump){
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            AmountOfJumpsLeft--;
        }
    }

    private void ApplyMovement(){
        rb.velocity = new Vector2(movementSpeed*movementInputDirection,rb.velocity.y);
    }

    private void Flip(){
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f,180.0f,0.0f);
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
    }

    private bool CheckIsFalling(){
        yvelocity = rb.velocity.y;
        if(yvelocity <= 16 && yvelocity >=0.1){
            isFalling = false;
        }else if(yvelocity < 0){
            isFalling = true;
        }else if(yvelocity == 0 && isGrounded){
            isFalling = false;
        }
        return isFalling;
    }

    void updateJumpState(){
        float blend = animator.GetFloat("Jump_State");
        switch(blend){
            case 0://en el piso 
                if(!isFalling){//si esta subiendo, animacion de subir
                    animator.SetFloat("Jump_State",1);
                }break;
            case 1://subiendo 
                if(isFalling){//si empieza a bajar, ya llegamos al punto mas alto 
                    animator.SetFloat("Jump_State",2);
                }break;
            case 2://punto mas alto 
                if(isFalling && yvelocity < -2){//si bajamos y nuestra velocidad vertical aumenta, animacion de caer
                    animator.SetFloat("Jump_State",3);
                 }break;
            case 3://cayendo
                if(isGrounded){//si tocamos piso, vovemos al inicio
                    animator.SetFloat("Jump_State",0);
                 }break;
        }
    }
}
