using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public bool CanSmellPlayer()
    {
        return Smell();
    }

    public bool CanHearPlayer()
    {
        return Listen();
    }

    public bool CanSeePlayer()
    {
        return Look();
    }

    public bool CanSensePlayer()
    {
        return CanHearPlayer() || CanSmellPlayer() || CanSeePlayer();
    }
    
    private bool Smell()
    {
        GameObject[] allBCs = GameObject.FindGameObjectsWithTag("breadcrumb");
        float minDistance = 20 ;
        bool detectedBC = false;
        foreach (var o in allBCs)
        {
            if (Vector3.Distance(transform.position, o.transform.position) < minDistance)
            {
                detectedBC = true;
                break;
            }
        }
        return detectedBC;
    }
    private bool Listen()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance < 15;
    }

    private bool Look()
    {
        var ray = new Ray();
        RaycastHit hit;
        float castingDistance = 50;
        ray.origin = new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + Vector3.up * 0.7f;
        ray.direction = transform.forward * castingDistance;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            
        }

        return false;
    }
}
