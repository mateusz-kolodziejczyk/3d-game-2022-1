using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private GameObject projectile;

    [SerializeField] private Camera camera;

    [SerializeField] private float bulletForce;

    [SerializeField] private int maxAmmo;
    private int ammo;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo > 0 && Input.GetMouseButtonDown(0))
        {
            var bullet = SpawnBullet();
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletForce);
            bullet.GetComponent<PlayerBullet>().Damage = damage;
            ammo --;
        }
        UpdateAmmoText();
    }

    private GameObject SpawnBullet()
    {
        var cameraTransform = camera.transform;
        return Instantiate(projectile, transform.position, cameraTransform.rotation);
    }

    private void UpdateAmmoText()
    {
        var ammoText = GameObject.FindWithTag("ammotext");
        if(ammoText != null && ammoText.TryGetComponent(out Text t))
        {
            t.text = $"Health: {ammo}/{maxAmmo}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ammo"))
        {
            ammo = maxAmmo;
            other.gameObject.SetActive(false);
        }
    }
}
