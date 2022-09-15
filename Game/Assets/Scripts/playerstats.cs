using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerstats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private GameObject
        deathchunkparticle,
        deathbloodparticle;
    
    private float currentHealth;
    private GameManager GM;
    private void Start(){
        
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void DecreaseHealth(float amount){
        currentHealth -= amount;
        if (currentHealth <= 0.0f){
            Die();
        }
    }
    private void Die(){
        Instantiate(deathchunkparticle, transform.position,deathchunkparticle.transform.rotation);
        Instantiate(deathbloodparticle, transform.position,deathbloodparticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);
    }
}
