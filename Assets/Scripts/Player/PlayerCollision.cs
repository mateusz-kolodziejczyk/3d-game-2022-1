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
        if (other.CompareTag("enemy"))
        {
            health.HP -= health.MaxHP * 0.2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectable"))
        {
            var o = GameObject.FindWithTag("GameController");
            if (o.TryGetComponent(out CollectableManager collectableManager))
            {
                collectableManager.Collect();
            }
            other.gameObject.SetActive(false);
        }
    }
}
