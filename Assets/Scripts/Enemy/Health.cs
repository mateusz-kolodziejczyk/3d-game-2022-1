using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   private float hp;
   [SerializeField] private float maxHP;

   private float deathCounter = 0;
   private float deathTime = 2;

   private void Update()
   {
      if (hp <= 0)
      {
         deathCounter += Time.deltaTime;
         if (deathCounter >= deathTime)
         {
            Destroy(gameObject);
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
