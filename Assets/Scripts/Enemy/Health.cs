using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField] private float maxHP;
   public float HP { get; set; }
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
