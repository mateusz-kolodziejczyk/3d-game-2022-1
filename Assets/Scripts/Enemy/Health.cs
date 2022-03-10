using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
   private float hp;
   [SerializeField] private float maxHP;

   private float deathCounter = 0;
   private float deathTime = 3;

   private void Update()
   {
      if (hp <= 0 )
      {
         if(gameObject.CompareTag("Player")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         }
         else{
            deathCounter += Time.deltaTime;
            if (deathCounter >= deathTime)
            {
               Destroy(gameObject);
            }
         }

      }
   }

   public float HP
   {
      get => hp; set => hp = value; }
   public float MaxHP
   {
      get => maxHP;
      private set => maxHP = value;
   }
   private void Start()
   {
      HP = maxHP;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("health"))
      {
         HP = MaxHP;
      }
   }
   
   
}
