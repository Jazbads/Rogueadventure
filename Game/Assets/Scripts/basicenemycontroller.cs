using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicenemycontroller : MonoBehaviour
{
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }
    private State currentState;

    [SerializeField]
    private Transform
        groundcheck,
        wallcheck,
        touchdamagecheck;
    [SerializeField]
    private LayerMask 
        whatisground,
        whatisplayer;

    [SerializeField]
    private Vector2 knockbackspeed;
    [SerializeField]
    private GameObject
        hitparticle,
        deathchunkparticle,
        deathbloodparticle;
    
    private float 
        currentHealth,
        knockbackstartTime;
    
    private float[]attackdetails = new float[2];


    private int 
        facingdirection,
        damagedirection;

    private Vector2 
        movement,
        touchdamagebotleft,
        touchdamagetopright;
    [SerializeField]
    private float
        groundcheckdistance,
        wallcheckdistance,
        movementspeed,
        maxHealth,
        knockbackduration,
        lasttouchdamagetime,
        touchdamagecooldown,
        touchdamage,
        touchdamagewidth,
        touchdamageheight;

    private bool
        groundetected,
        walldetected;

    private GameObject alive;

    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();
        facingdirection = 1;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                updatewalkingstate();
                break;
            case State.Knockback:
                updateknockbackstate();
                break;
            case State.Dead:
                updatedeadstate();
                break;

        }
    }

    //walking state

    private void enterwalkingstate()
    {

    }

    private void updatewalkingstate()
    {
        groundetected = Physics2D.Raycast(groundcheck.position, Vector2.down, groundcheckdistance, whatisground);
        walldetected= Physics2D.Raycast(wallcheck.position, transform.right, wallcheckdistance, whatisground);

        checktouchdamage();

        if(!groundetected || walldetected)
        {
            flip();
        }
        else
        {
            movement.Set(movementspeed * facingdirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;

        }
    }

    private void exitwalkingstate()
    {
        
    }

    //knockbackstate

    private void enterknockbackstate()
    {
        knockbackstartTime = Time.time;
        movement.Set(knockbackspeed.x * damagedirection, knockbackspeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("knockback", true);
    }
    private void updateknockbackstate()
    {
        if(Time.time >= knockbackstartTime + knockbackduration)
        {
            switchstate(State.Walking);
        }
    }
    private void exitknockbackstate()
    {
        aliveAnim.SetBool("knockback", true);
    }

    //deadstate
    private void enterdeadstate()
    {
        Instantiate (deathchunkparticle, alive.transform.position, deathchunkparticle.transform.rotation);
        Instantiate (deathbloodparticle, alive.transform.position, deathbloodparticle.transform.rotation);
        Destroy(gameObject);
    }
    private void updatedeadstate()
    {

    }
    private void exitdeadstate()
    {
        
    }

    private void Damage(float[] attackdetails)
    {
        currentHealth -= attackdetails[0];

        Instantiate(hitparticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));

        if(attackdetails[1] > alive.transform.position.x)
        {
            damagedirection = -1;
        }
        else
        {
            damagedirection = 1;
        }

        if(currentHealth > 0.0f)
        {
            switchstate(State.Knockback);
        }
        else if(currentHealth <= 0.0f)
        {
            switchstate(State.Dead);
        }
    }
    private void flip()
    {
        facingdirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void checktouchdamage(){
        if( Time.time >= lasttouchdamagetime + touchdamagecooldown)
        {
            touchdamagebotleft.Set(touchdamagecheck.position.x - (touchdamagewidth / 2), touchdamagecheck.position.y - (touchdamageheight / 2));
            touchdamagetopright.Set(touchdamagecheck.position.x + (touchdamagewidth / 2), touchdamagecheck.position.y + (touchdamageheight / 2));
            Collider2D hit = Physics2D.OverlapArea(touchdamagebotleft, touchdamagetopright, whatisplayer);

            if(hit != null)
            {
                lasttouchdamagetime = Time.time;
                attackdetails[0] = touchdamage;
                attackdetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackdetails);
            }
        }
    }
    private void switchstate(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                exitwalkingstate();
                break;
            case State.Knockback:
                exitknockbackstate();
                break;
            case State.Dead:
                exitdeadstate();
                break;
        }
        switch (state)
        {
            case State.Walking:
                enterwalkingstate();
                break;
            case State.Knockback:
                enterknockbackstate();
                break;
            case State.Dead:
                enterdeadstate();
                break;
        }
        currentState = state;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundcheck.position, new Vector2 (groundcheck.position.x, groundcheck.position.y - groundcheckdistance));
        Gizmos.DrawLine(wallcheck.position, new Vector2 (wallcheck.position.x + wallcheckdistance, wallcheck.position.y));

        Vector2 botleft = new Vector2(touchdamagecheck.position.x - (touchdamagewidth / 2), touchdamagecheck.position.y - (touchdamageheight / 2));
        Vector2 botright = new Vector2(touchdamagecheck.position.x + (touchdamagewidth / 2), touchdamagecheck.position.y - (touchdamageheight / 2));
        Vector2 topright = new Vector2(touchdamagecheck.position.x + (touchdamagewidth / 2), touchdamagecheck.position.y + (touchdamageheight / 2));
        Vector2 topleft = new Vector2(touchdamagecheck.position.x - (touchdamagewidth / 2), touchdamagecheck.position.y + (touchdamageheight / 2));

        Gizmos.DrawLine(botleft, botright);
        Gizmos.DrawLine(botright, topright);
        Gizmos.DrawLine(topright, topleft);
        Gizmos.DrawLine(topleft, botleft);
    }
}
