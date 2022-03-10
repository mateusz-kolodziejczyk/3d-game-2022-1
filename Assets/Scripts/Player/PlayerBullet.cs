using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Damage { get; set; } = 0;

    private float TimeToLive = 3;

    private float timeAlive = 0;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("enemy"))
        {
            var health = other.gameObject.GetComponent<Health>();
        
            other.gameObject.GetComponent<ReactAttacked>().WasAttacked = true;
            health.HP -= Damage;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > TimeToLive)
        {
            Destroy(gameObject);
        }

    }
}
