using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercombatcontroller : MonoBehaviour
{
   [SerializeField]
   private bool combatenabled;
   [SerializeField]
   private float inputtimer, attack1radius, attack1damage;
   [SerializeField]
   private Transform attack1hitboxpos;
   [SerializeField]
   private LayerMask whatisdamageable;
   private bool gotinput,isattacking,isfirstattack,attack1;
   private float lastinputtime = Mathf.NegativeInfinity;
   private float[] attackdetails = new float[2];
   private Animator anim;
   private PlayerController PC;
   private playerstats PS;
   private void Start()
   {
       anim = GetComponent<Animator>();
       anim.SetBool("canattack",combatenabled);
       PC = GetComponent<PlayerController>();
       PS = GetComponent<playerstats>();
   }
   private void Update()
       {
           Checkcombatinput();
           Checkattack();
       }
   private void Checkcombatinput()
   {
       if (Input.GetMouseButtonDown(0))
       {
           if (combatenabled)
           {
                gotinput = true;
                lastinputtime = Time.time;

           }
       }
   }
   private void Checkattack()
   {
       if(gotinput)
       {
           if(!isattacking)
           {
               gotinput = false;
               isattacking = true;
               isfirstattack = !isfirstattack;
               anim.SetBool("attack1",true);
               anim.SetBool("firstattack",true);
               anim.SetBool("isattacking",isattacking);
           }
       }
       if(Time.time >= lastinputtime + inputtimer)
       {
           gotinput = false;
       }
   }
   private void checkattackhitbox(){
       Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1hitboxpos.position, attack1radius, whatisdamageable);
       attackdetails[0] = attack1damage;
       attackdetails[1] = transform.position.x;

       foreach (Collider2D collider in detectedObjects)
       {
           collider.transform.parent.SendMessage("Damage", attackdetails);
       }
   }
   private void FinishAttack1()
    {
        isattacking = false;
        anim.SetBool("isattacking",isattacking);
        anim.SetBool("attack1",false);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(attack1hitboxpos.position, attack1radius);
    }
    private void Damage(float[] attackdetails){
        int direction;

        PS.DecreaseHealth(attackdetails[0]);

        if(attackdetails[1] < transform.position.x){
            direction = 1;
        }else
        {
            direction = -1;
        }

        PC.Knockback(direction);
    }
}
