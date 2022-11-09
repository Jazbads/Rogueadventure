using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{
   [SerializeField]
   private Transform respawnpoint;
   [SerializeField]
   private GameObject player;
   [SerializeField]
   private float respawntime;
   private float respawntimestart;
   private bool respawn;
   public GameObject playerprefab;

   private CinemachineVirtualCamera CVC;

   private void Start(){
       
       }
    private void Update(){
        checkrespawn();
    }

   public void Respawn()
   {
       respawntimestart = Time.time;
       respawn = true;
   }

   private void checkrespawn()
   {
       if(Time.time >= respawntimestart + respawntime && respawn)
       {
           GameObject player = Instantiate(playerprefab, respawnpoint);
           CVC.m_Follow = player.transform;
           respawn = false;
        }
   }
}
