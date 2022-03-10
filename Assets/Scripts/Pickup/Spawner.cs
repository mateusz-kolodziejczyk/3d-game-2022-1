using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject spawnedObject = null;

    [SerializeField] private GameObject ItemPrefab;


    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = Instantiate(ItemPrefab, transform.position, transform.rotation);
        spawnedObject.SetActive(false);
    }


    public void Spawn(){
        spawnedObject.SetActive(true);
    }

    public bool IsSpawned(){
        return spawnedObject.activeSelf;
    }

}
