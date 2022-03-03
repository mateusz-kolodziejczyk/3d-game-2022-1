using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ManageHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    private float _health = 100;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        setHealth(_health - Time.deltaTime);
    }

    public float getHealth()
    {
        return _health;
    }

    public void setHealth(float newHealth)
    {
        _health = newHealth;
        _animator.SetInteger("health", (int)_health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("health"))
        {
            setHealth(_health + 50);
            Destroy(other.gameObject);
        }
    }
}
