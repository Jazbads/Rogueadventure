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

   private CinemachineVirtualCamera CVC;

   private void Start(){
       
       CVC = GameObject.Find("playerCam").GetComponent<CinemachineVirtualCamera>();
       
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
           var playerTemp = Instantiate(player, respawnpoint);
           CVC.m_Follow = playerTemp.transform;
           respawn = false;
        }
   }
}
