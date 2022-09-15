using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
 {
     #region Public Variables
     
     
     
     public float attackDistance; //Minimum distance
     public float moveSpeed;
     public float timer; //Cooldown of attacks
     public Transform leftLimit;
     public Transform rightLimit;
     [HideInInspector] public Transform target;
     [HideInInspector] public bool inRange; //See if player in range
     public GameObject hotZone;
     public GameObject triggerArea;
     #endregion
 
     #region Private Variables
 
 
     private Animator animator;
     private float distance; //store distance between enemy and player
     private bool attackMode;
     
     private bool cooling; //check if enemy is cooling after attack
     private float intTimer;
     #endregion 
 
     void Awake()
     {
         SelectTarget();
         intTimer = timer;
         animator = GetComponent<Animator>();
     }
 
     // Update is called once per frame
     void Update()
     {
         if (!attackMode)
         {
             Move();
         }
 
         if(!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
         {
             SelectTarget();
         }
 
         if (inRange)
         {
 
             EnemyLogic();
         }
     }
 
   
 
     void EnemyLogic()
     {
         distance = Vector2.Distance(transform.position, target.position);
 
         if (distance > attackDistance)
         {
             StopAttack();
         }
         else if (attackDistance >= distance && cooling == false)
         {
             Attack();
         }
 
         if (cooling)
         {
             Cooldown();
             animator.SetBool("Attack", false);
         }
     }
 
     void Move()
     {
         animator.SetBool("canWalk", true);
 
         if (!animator.GetCurrentAnimatorStateInfo(0).IsName("enemy1Attack"))
         {
             Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
 
             transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
 
         }
     }
 
     void Attack()
     {
         timer = intTimer;
         attackMode = true;
         animator.SetBool("canWalk", false);
         animator.SetBool("Attack", true);
     }
 
     void Cooldown()
     {
         timer -= Time.deltaTime;
 
         if (timer <= 0 && cooling && attackMode)
         {
             cooling = false;
             timer = intTimer;
         }
     }
 
     void StopAttack()
     {
         cooling = false;
         attackMode = false;
         animator.SetBool("Attack", false);
     }
 
     
 
     public void TriggerCooling()
     {
         cooling = true;
     }
 
     private bool InsideofLimits()
     {
         return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
     }
     
     public void SelectTarget()
     {
         float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
         float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);
 
         if(distanceToLeft > distanceToRight)
         {
             target = leftLimit;
         }
         else
         {
             target = rightLimit;
         }
 
         Flip();
     }
 
     public void Flip()
     {
         Vector3 rotation = transform.eulerAngles;
         if(transform.position.x > target.position.x)
         {
             rotation.y = 180f;
         }
         else
         {
             rotation.y = 0f;
         }
 
         transform.eulerAngles = rotation;
     }
 }