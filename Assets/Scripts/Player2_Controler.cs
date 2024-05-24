using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Player2_Controler : MonoBehaviour
{

    private Rigidbody2D rb;
    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    private bool isFacingRight = false;
    public float jumpForce = 16.0f;
    private bool isGrounded;
    private bool CanJump;
    private float yvelocity;
    private bool isFalling;
    private int AmountOfJumpsLeft;
    public int jumps = 3;

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
                if(Input.GetKey("right") && Input.GetKey("up") || Input.GetKey("left") && Input.GetKey("up")) {
                    animator.SetInteger("State",2);
                }else if(Input.GetKey("up")){
                    animator.SetInteger("State",2);
                }else if(Input.GetKey("right") || Input.GetKey("left")){
                    animator.SetInteger("State",1);
                }else if(Input.GetKey("l")){
                    animator.SetInteger("State",3);
                }
                break;
            case 1:
                if(Input.GetKey("up")){
                    animator.SetInteger("State",2);
                }else if(!Input.GetKey("right") && !Input.GetKey("left")){
                    animator.SetInteger("State",0);
                }else if(Input.GetKey("l")){
                    animator.SetInteger("State",3);
                }
                break;
            case 2:
                if(isGrounded && Input.GetKey("left") || isGrounded && Input.GetKey("right")){
                    animator.SetInteger("State",1);
                }else if(isGrounded){
                    animator.SetInteger("State",0);
                }break;
            case 3:
                if(!Input.GetKey("l")){
                    animator.SetInteger("State",0);
                }else if(Input.GetKey("right") || Input.GetKey("left")){
                    animator.SetInteger("State",1);   
                }break;
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
        movementInputDirection = Input.GetAxisRaw("Horizontal_Player2");
        if(Input.GetButtonDown("Jump_Player2")){
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
            case 0:
                if(!isFalling){
                    animator.SetFloat("Jump_State",1);
                }break;
            case 1:
                if(isFalling){
                    animator.SetFloat("Jump_State",2);
                }break;
            case 2:
                if(isFalling && yvelocity < -2){
                    animator.SetFloat("Jump_State",3);
                 }break;
            case 3:
                if(isGrounded){
                    animator.SetFloat("Jump_State",0);
                 }break;
        }
    }
}