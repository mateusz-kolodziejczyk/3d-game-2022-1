using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shooting : MonoBehaviour
{
    private int ammo;
    [SerializeField] private int maxAmmo;
    public int Ammo { get => ammo; set => ammo = value; }
    
    public int MaxAmmo { get => maxAmmo; private set => maxAmmo = value; }

    [SerializeField] private float attackSpeed;

    private float shootingTimer = 0;
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }

    private void Start()
    {
        Ammo = MaxAmmo;
    }

    public bool Shoot()
    {
        Ammo--;
        if (Ammo <= 0)
        {
            return false;
        }
        return true;
    }

    public bool ReadyToShoot()
    {
        shootingTimer += Time.deltaTime;
        if (attackSpeed <= shootingTimer)
        {
            shootingTimer = 0;
            return true;
        }
        return false;
    }

    public void ResetShootingTimer()
    {
        shootingTimer = 0;
    }
    
}
