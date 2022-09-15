using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 9;
    public float maxSpeed = 10;
    public float JumpPower = 100;
    public float turntimerset = 0.1f;
    public bool isgrounded;
    private bool isfacingRight = true;
    private bool isRunning;
    private bool canjump;
    private bool canMove;
    private bool canFlip;
    private bool canclimbledge = false;
    private bool ledgedetected;
    private bool istouchingwall;
    private bool iswallsliding;
    private bool istouchingledge;
    private bool knockback;
    public float groundCheckRadius;
    public float airdragmultiplier = 0.95f;
    public float wallcheckdistance;
    public float wallslidespeed;
    public float ledgeclimbxoffset1 = 0f;
    public float ledgeclimbyoffset1 = 0f;
    public float ledgeclimbxoffset2 = 0f;
    public float ledgeclimbyoffset2 = 0f;
    private float movementinputdirection;
    private float turntimer;
    private float knockbackstarttime;
    [SerializeField]
    private Vector2 knockbackspeed;
    [SerializeField]
    private float knockbackduration;
    private int facingdirection;
    private Rigidbody2D rb;
    private Physics2D physic2D;
    private Animator anim;
    public int maxHealth = 100;
    public int currentHealth;
    public Transform groundCheck;
    public Transform wallcheck;
    public Transform ledgecheck;
    public LayerMask whatisground;
    public LayerMask whatiswall;
    public float movementforceinair;
    public float variablejumpheightmultiplier = 0.5f;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CheckMovementDirection();
        checkifcanjump();
        Checkifwallsliding();
        checkledgeclimb();
        Updateanimations();
        checkinput();
        checkknockback();
    }
    private void Jump(){
        if (canjump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
    }
    private void checkifcanjump()
    {
        if (isgrounded && rb.velocity.y <= 0){
            canjump = true;
        }
        else if (istouchingwall && iswallsliding)
        {
            canjump = true;
        }
        else
        {
            canjump = false;
        }
    }
    private void checkinput()
    {
        movementinputdirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")){
            Jump();
        }
        if (Input.GetButtonUp("Jump")){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variablejumpheightmultiplier);
        }
        if (Input.GetButtonDown("Horizontal") && istouchingwall)
        {
            if(!isgrounded && movementinputdirection != facingdirection)
            {
                canMove = false;
                canFlip = false;
                
                turntimer = turntimerset;
            }
        }
        if (turntimer >= 0)
        {
            turntimer -= Time.deltaTime;

            if(turntimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }
    }
    private void Checkifwallsliding()
    {
        if (istouchingwall && !isgrounded && rb.velocity.y < 0 && !canclimbledge)
        {
            iswallsliding = true;
        }
        else
        {
            iswallsliding = false;
        }
    }
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackstarttime = Time.time;
        rb.velocity = new Vector2(knockbackspeed.x * direction, knockbackspeed.y);
    }
    private void checkknockback()
    {
        if(Time.time >= knockbackstarttime + knockbackduration && knockback){
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    private void checkledgeclimb()
    {
        if(ledgedetected && !canclimbledge)
        {
            canclimbledge = true;

            if (isfacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallcheckdistance) - ledgeclimbxoffset1, Mathf.Floor(ledgePosBot.y) + ledgeclimbyoffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallcheckdistance) + ledgeclimbxoffset2, Mathf.Floor(ledgePosBot.y) + ledgeclimbyoffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallcheckdistance) + ledgeclimbxoffset1, Mathf.Floor(ledgePosBot.y) + ledgeclimbyoffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallcheckdistance) - ledgeclimbxoffset2, Mathf.Floor(ledgePosBot.y) + ledgeclimbyoffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canclimbledge", canclimbledge);
        }

        if (canclimbledge)
        {
            transform.position = ledgePos1;
        }
    }
    public void Finishledgeclimb()
    {
        canclimbledge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgedetected = false;
        anim.SetBool("canclimbledge", canclimbledge);
    }
    private void Updateanimations()
    {
        anim.SetBool("isRunning",isRunning);
        anim.SetBool("isgrounded",isgrounded);
        anim.SetFloat("yvelocity",rb.velocity.y);
        anim.SetBool("iswallsliding", iswallsliding);
    }
    
    private void Checksurroundings(){
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatisground);
        istouchingwall = Physics2D.Raycast(wallcheck.position, transform.right, wallcheckdistance , whatisground);
        istouchingledge = Physics2D.Raycast(ledgecheck.position, transform.right, wallcheckdistance, whatisground);
        
        if(istouchingwall && !istouchingledge && !ledgedetected){
            ledgedetected = true;
            ledgePosBot = wallcheck.position;

        }
    }
    private void CheckMovementDirection()
    {
        if(isfacingRight && movementinputdirection < 0)
        {
            Flip();
        }
        else if (!isfacingRight && movementinputdirection > 0)
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) >= 0.01f){
            isRunning = true;
        }else
            isRunning = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            //     transform.position = respawnPoint;
        }
        //else if(collision.tag == "Checkpoint")
        //{
        //    respawnPoint = transform.position;
        //}
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //    respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            //    respawnPoint = transform.position;
        }
        else if (collision.tag == "2Level")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        else if (collision.tag == "Previous2Level")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        Gizmos.DrawLine(wallcheck.position, new Vector3(wallcheck.position.x + wallcheckdistance, wallcheck.position.y, wallcheck.position.z));
    }
    private void FixedUpdate() 
    {
        Checksurroundings();
        applymovement();
    }
    private void applymovement(){

        if(canMove && !knockback)
        {
            rb.velocity = new Vector2(speed * movementinputdirection, rb.velocity.y);
        }
        if(!isgrounded && !iswallsliding && movementinputdirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airdragmultiplier, rb.velocity.y);
        }
        if (iswallsliding)
        {
            if(rb.velocity.y < -wallslidespeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallslidespeed);
            }
        }
    }
    public int getfacingdirection()
    {
        return facingdirection;
    }
    private void Flip()
    {
        if (!iswallsliding && canFlip && !knockback)
        {
            facingdirection *= -1;
            isfacingRight = !isfacingRight;
            transform.Rotate(0.0f,180.0f,0.0f);
        }
    }
}