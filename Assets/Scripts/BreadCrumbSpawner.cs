using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject breadCrumb;
    [SerializeField] private float timeBetweenSpawns;
    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > timeBetweenSpawns)
        {
            Spawn();
            _timer = 0;
        }
    }

    private void Spawn()
    {
        Instantiate(breadCrumb, transform.position, Quaternion.identity);
    }
}
