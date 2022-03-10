using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{

    private Health health;
    private void Start(){
        health = GetComponent<Health>();
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Collided! {other.tag}");

        if (other.CompareTag("enemy"))
        {
            health.HP -= health.MaxHP * 0.2f;
        }
    }

}
