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
        otherHealth.HP -= damageToNPC;
    }
}
