using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private GameObject projectile;

    [SerializeField] private Camera camera;

    [SerializeField] private float bulletForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = SpawnBullet();
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletForce);
            bullet.GetComponent<PlayerBullet>().Damage = damage;
        }
    }

    private GameObject SpawnBullet()
    {
        var cameraTransform = camera.transform;
        return Instantiate(projectile, transform.position, cameraTransform.rotation);
    }
}
