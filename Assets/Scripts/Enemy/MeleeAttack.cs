using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private bool isEnemy = false;
    [SerializeField] float damageToNPC;
    public float DamageToNPC {get => damageToNPC; set => damageToNPC = value;}

    // Start is called before the first frame update
    void Start()
    {
        isEnemy = gameObject.CompareTag("enemy");
        Debug.Log(isEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack(Health otherHealth){
        otherHealth.HP -= damageToNPC * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy") && !isEnemy)
        {
            Attack(other.transform.parent.GetComponent<Health>());
        }
        else if (other.CompareTag("friendlyHitTrigger") && isEnemy)
        {
            Attack(other.transform.parent.GetComponent<Health>());
        }
    }
}
