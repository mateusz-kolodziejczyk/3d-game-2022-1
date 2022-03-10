using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

      if (gameObject.CompareTag("Player"))
      {
         UpdateHealthText();
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
      HP = MaxHP;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("health"))
      {
         HP = MaxHP;
         other.gameObject.SetActive(false);
      }
   }

   private void UpdateHealthText()
   {
      var healthText = GameObject.FindWithTag("healthtext");
      if(healthText != null && healthText.TryGetComponent(out Text t))
      {
         t.text = $"Health: {HP}/{MaxHP}";
      }
   }
   
   
}
