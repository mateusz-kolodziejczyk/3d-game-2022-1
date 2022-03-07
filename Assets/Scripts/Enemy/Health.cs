using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField] private float maxHP;
   public float HP { get; set; }

   private void Start()
   {
      HP = maxHP;
   }
}
