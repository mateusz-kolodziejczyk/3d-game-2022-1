using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Damage { get; set; } = 0;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("enemy"))
        {
            var health = other.gameObject.GetComponent<Health>();
            health.HP -= Damage;
        }
    }
}
